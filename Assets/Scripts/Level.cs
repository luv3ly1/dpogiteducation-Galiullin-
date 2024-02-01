using System.Collections.Generic;
using System.Linq;
using Control;
using ScriptableObjects;
using UnityEngine;
using UnityEngine.Events;
using System;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class Level : MonoBehaviour
{
    #region Level Init Data

    public static LevelData CurrentLevelData;

    #endregion

    public static Level Instance { get; private set; }

    [SerializeField] private PlacementObject[] all;
    [SerializeField] private PresetItems[] presets;
    [SerializeField] private Vector2Int minXZValues, maxXZValues;
    [SerializeField] private SwipeController swipeController;
    [SerializeField] private GameObject winUI;
    [SerializeField] private LevelData defaultLevel;
    [Header("Уровень")]
    [SerializeField] private Transform placePlane;
    [SerializeField] private Transform car;
    [SerializeField] private Transform wall1, wall2, roof;
    [SerializeField] public GameObject redPlane, greenPlane;
    [SerializeField] private bool isTest;

    public int Score { get; private set; }
    public int AllScore { get; private set; }

    public List<PlacementObject> AllPlaceObjects { get; private set; }

    public UnityEvent<int> scoreChanged;

    public Vector2Int MinXZValues => minXZValues;
    public Vector2Int MaxXZValues => maxXZValues;

    private void Awake()
    {
        Instance = this;

        if (isTest)
        {
            CurrentLevelData = new LevelData();
            CurrentLevelData.objects = all;
            CurrentLevelData.size = new Vector2Int(5, 3);
            CurrentLevelData.height = 3;
            CurrentLevelData.placementObjects = new PlacementObjectInCar[0];
        }
        else
        {
            CurrentLevelData = LevelData.Generate(all.ToList(), new Vector2Int(5, 3), 3);
        }

        Init();
        AllScore = AllPlaceObjects.Sum(o => o.Volume);
    }

    private void Start()
    {
        ShowStartAds();
    }
    
    private void ShowStartAds()
    {
        if(VKSDK.Instance._data.Score < 1) 
            return;

        var rand = Random.Range(0, 2);
        if(rand == 0)
            VKSDK.Instance.ShowingAd();
    }
    
    private void Init()
    {
        minXZValues = new Vector2Int(-(Mathf.Abs(CurrentLevelData.size.x) - 1), 0);
        maxXZValues = new Vector2Int(0, CurrentLevelData.size.y - 1);

        InitBorders();
        InitPlacementObject();

        swipeController.BuildLine();
    }

    private void InitPlacementObject()
    {
        AllPlaceObjects = new List<PlacementObject>();
        CurrentLevelData.objects.Shuffle();
        
        foreach (var placementObject in CurrentLevelData.objects)
        {
            var obj = Instantiate(placementObject, swipeController.transform);
            AllPlaceObjects.Add(obj);
        }
        
        foreach (var placementObject in CurrentLevelData.placementObjects)
        {
            var obj = Instantiate(placementObject.placementObject,
                new Vector3(placementObject.position.x, 0, placementObject.position.y), Quaternion.identity);
            obj.isCanDrag = false;
        }
    }

    private void InitBorders()
    {
        placePlane.localScale =
            new Vector3((Mathf.Abs(minXZValues.x + maxXZValues.x) + 1) * .1f, 1,
                (Mathf.Abs(minXZValues.y + maxXZValues.y) + 1) * .1f);

        wall1.localScale = new Vector3(CurrentLevelData.size.x + wall2.GetChild(0).localScale.z,
            CurrentLevelData.height, wall1.localScale.z);
        wall2.position = new Vector3(minXZValues.x, wall2.position.y, wall2.position.z);
        wall2.localScale = new Vector3(CurrentLevelData.size.y, CurrentLevelData.height, wall2.localScale.z);

        roof.localScale = new Vector3(CurrentLevelData.size.x + wall2.GetChild(0).localScale.z,
            CurrentLevelData.size.y + wall2.GetChild(0).localScale.z,
            roof.localScale.z);
        roof.position = new Vector3(roof.position.x, CurrentLevelData.height, roof.position.z);

        car.position = new Vector3(minXZValues.x, 0, maxXZValues.y + 1);
        car.localScale *= CurrentLevelData.size.y / 3f;

        Camera.main.fieldOfView = CurrentLevelData.cameraView;
    }

    public void OnObjectInLine(Transform newObject = null) =>
        swipeController.BuildLine(newObject);


    public void OnObjectPlaceChange()
    {
        Score = AllPlaceObjects.Where(o => o.IsPlacement && o.IsCorrectPlacement).Sum(o => o.Volume);

        scoreChanged?.Invoke(Score);

        if (Score >= AllPlaceObjects.Sum(o => o.Volume))
            Win();
    }

    public static void Exit()
    {
        SceneManager.LoadScene("Scenes/StartScene");
    }

    public void OnCheckHeight() =>
        roof.gameObject.SetActive(AllPlaceObjects.Count(o => o.IsPlacement && !o.IsCorrectPlacement) > 0);

    private void Win()
    {
        winUI.SetActive(true);
    }
}

public static class ListShuffler
{
    private static System.Random rng = new();

    public static void Shuffle<T>(this IList<T> list)
    {
        int n = list.Count;

        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }
}

[Serializable]
public class PresetItems
{
    public List<PlacementObject> items;
}
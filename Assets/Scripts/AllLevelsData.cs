using System;
using ScriptableObjects;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using YG;

public class AllLevelsData : MonoBehaviour
{
    public static LevelData[] AllLevels;
    public static int CurrentLevelNumber;
    
    [SerializeField] private LevelData[] levels;
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private TMP_Text scoreText2;

    public static int Score { get; private set; }

    private void Awake()
    {
        AllLevels = levels;
        
        //SetScoreText(VKSDK.Instance._data.Score);
    }

    private void Start()
    {
        VKSDK.Instance.onScoreLoaded.AddListener(SetScoreText);
        VKSDK.Instance.GettingData();
    }

    public void SetScoreText(int score)
    {
        scoreText.text = scoreText2.text = $"Грузов доставленно - {score}";
    }
    
    public static void LoadLevel()
    {
        SceneManager.LoadScene("LevelScene");
    }

    public void LoadTest()
    {
        SceneManager.LoadScene("LevelSceneTest");
    }
    
    public static void LoadLevel(int levelNumber)
    {
        
        if (levelNumber > AllLevels.Length - 1 || levelNumber < 0)
        {
            SceneManager.LoadScene("StartScene");
            return;
        }
        
        Level.CurrentLevelData = AllLevels[levelNumber];
        CurrentLevelNumber = levelNumber;
        SceneManager.LoadScene("LevelScene");
    }
}
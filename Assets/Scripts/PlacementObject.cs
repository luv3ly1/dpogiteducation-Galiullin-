using System.Linq;
using Control;
using UnityEngine;

public class PlacementObject : MonoBehaviour
{
    [SerializeField] private ObjectPlacementController[] variants;
    
    private int _currentVariant;
    private Transform parentPlaceState;
    public Transform standartParent;

    public bool isCanDrag = true;
    
    public ObjectPlacementController CurrentVariant => variants[_currentVariant];
    public bool IsPlacement => CurrentVariant.IsPlacement;
    public bool IsCorrectPlacement => CurrentVariant.IsCorrectPlacement;
    public int Volume => CurrentVariant.Volume;
    public Vector3Int Size => new (Mathf.RoundToInt(CurrentVariant._colliders.Max(c => c.center.x) + 0.5f), 
        Mathf.RoundToInt(CurrentVariant._colliders.Max(c => c.center.y) + 0.5f),
        Mathf.RoundToInt(CurrentVariant._colliders.Max(c => c.center.z) + 0.5f));

    private void OnValidate()
    {
        _currentVariant = 0;
        variants = GetComponentsInChildren<ObjectPlacementController>(true);

        //Size = new Vector3Int(Mathf.RoundToInt(CurrentVariant._colliders.Max(c => c.center.x) + 0.5f), 
        //    Mathf.RoundToInt(CurrentVariant._colliders.Max(c => c.center.y) + 0.5f),
        //    Mathf.RoundToInt(CurrentVariant._colliders.Max(c => c.center.z) + 0.5f));
    }

    private void Awake()
    {
        //Size = new Vector3Int(Mathf.RoundToInt(CurrentVariant._colliders.Max(c => c.center.x) + 0.5f), 
        //    Mathf.RoundToInt(CurrentVariant._colliders.Max(c => c.center.y) + 0.5f),
        //    Mathf.RoundToInt(CurrentVariant._colliders.Max(c => c.center.z) + 0.5f));
        standartParent = transform.parent;
        parentPlaceState = null;
        
        foreach (var objectPlacementController in variants)
        {
            objectPlacementController.gameObject.SetActive(false);
        }
        
        variants[_currentVariant].gameObject.SetActive(true);
    }

    public void OnInCar()
    {
        transform.parent = parentPlaceState;
    }

    public void OnOutCar()
    {
        transform.parent = standartParent;
    }

    public void OnPlace()
    {
        //transform.parent = parentPlaceState;
        SoundManager.Instance.Play(SoundManager.Instance.onBonusClip);
        Level.Instance.OnObjectInLine();
    }

    public void OnDeplace()
    {
        transform.parent = standartParent;
        SoundManager.Instance.Play(SoundManager.Instance.onDeplaceClip);
        Level.Instance.OnObjectInLine(transform);
    }
    
    public void ChangeVariant()
    {
        variants[_currentVariant].gameObject.SetActive(false);
            
        _currentVariant++;
        if (_currentVariant >= variants.Length)
            _currentVariant = 0;
        
        variants[_currentVariant].gameObject.SetActive(true);
        
        if(Level.Instance)
            Level.Instance.OnObjectInLine();
    }
}
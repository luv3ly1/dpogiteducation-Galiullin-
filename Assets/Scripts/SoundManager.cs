using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [SerializeField] public AudioClip onPlaceClip;
    [SerializeField] public AudioClip onDeplaceClip;
    [SerializeField] public AudioClip onBonusClip;
    
    public static SoundManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }
    
    public void Play(AudioClip clip)
    {
        var source = gameObject.AddComponent<AudioSource>();
        source.clip = clip;
        source.Play();
        StartCoroutine(DeleteSourceOnEnd(source));
    }

    private IEnumerator DeleteSourceOnEnd(AudioSource source)
    {
        while (source.isPlaying)
        {
            yield return null;
        }
        
        Destroy(source);
    }
}

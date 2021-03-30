
using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class AudioManager : MonoBehaviour
{
    private List<AudioSource> sources;
    private static AudioManager instance;
    
    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        sources = new List<AudioSource>();
    }

    public static void PlaySound(AudioObject sound)
    {
        AudioSource usableSource = null;
        
        // Use any available AudioSource
        foreach (AudioSource source in instance.sources)
        {
            if (!source.isPlaying)
            {
                usableSource = source;
                break;
            }
        }

        // Create a new one if none is available
        if (usableSource is null)
        {
            usableSource = instance.gameObject.AddComponent<AudioSource>();
            instance.sources.Add(usableSource);
        }
        
        usableSource.clip = sound.clips[Random.Range(0, sound.clips.Length)];
        usableSource.Play();
    }
}

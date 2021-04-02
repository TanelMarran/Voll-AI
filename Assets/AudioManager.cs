
using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

public class AudioManager : MonoBehaviour
{
    private List<AudioSource> sources;
    private static AudioManager instance;
    public bool useSound = true;
    
    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        sources = new List<AudioSource>();
    }

    public static void PlaySound(AudioObject sound)
    {
        PlaySound(sound, 1);
    }
    
    public static void PlaySound(AudioObject sound, float volume)
    {
        if (instance.useSound)
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
            usableSource.volume = sound.volume * volume;
            usableSource.pitch = 1 + Random.Range(-sound.pitchshift, sound.pitchshift);
            usableSource.Play();
        }
    }
}

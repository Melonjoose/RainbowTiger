using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    public Sound[] ostSounds, sfxSounds;
    public AudioSource ostSource, sfxSource;

    private void Awake()
    {
        // Ensure there is only one instance of AudioManager
        if (Instance == null)
        {
            Instance = this; // Set the static reference to this instance
        }
        else
        {
            Debug.LogWarning("Duplicate AudioManager detected and destroyed.");
            Destroy(gameObject); // Destroy duplicate instances
        }
    }

    public void Start()
    {

        PlayOST("OST_Main");
    }
    public void PlayOST(string name)
    {
        Sound s = Array.Find(ostSounds, x => x.name == name);

        if (s == null)
        {
            Debug.LogWarning("Sound Not Found: " + name);
            return;
        }

        else
        {
            ostSource.clip = s.clip;
            ostSource.volume = Mathf.Clamp(s.volume, 0f, 2f);
            ostSource.pitch = s.pitch;
            ostSource.Play();
        }
        
    }

    public void PlaySFX(string name)
    {
        Sound s = Array.Find(sfxSounds, x => x.name == name);

        if (s == null)
        {
            Debug.LogWarning("Sound Not Found: " + name);
            return;
        }

        else
        {
            sfxSource.pitch = s.pitch;
            sfxSource.PlayOneShot(s.clip, s.volume);
        }

        
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using JetBrains.Annotations;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    public Sound[] ostSounds, sfxSounds;
    public AudioSource ostSource, sfxSource;

  public bool isOSTMuted = false;
    public bool isSFXMuted = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogWarning("Duplicate AudioManager detected and destroyed.");
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            AudioManager.Instance.PlaySFX("SFX_Gum_Grapple");
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            AudioManager.Instance.PlaySFX("SFX_Gum_Inflate");
        }
    }

    public void Start()
    {
        PlayOST("OST_Main");
    }
    public void PlayOST(string name)
    {
        Sound s = Array.Find(ostSounds, x => x.name == name);


        if (isOSTMuted == false)
        {
            {
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
        }
    }

    public void StopOST()
    {
        if (ostSource.isPlaying)
        {
            ostSource.Stop();
        }
    }

    public void PlaySFX(string name)
    {
        Sound s = Array.Find(sfxSounds, x => x.name == name);
        if (isSFXMuted == false)
        {
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

    public void MuteOST()
    {
        if (isOSTMuted)
        {
            isOSTMuted = false;
            ostSource.Play();
        }
        else
        {
            // Mute and stop the OST
            isOSTMuted = true;
            if (ostSource.isPlaying)
            {
                ostSource.Stop();
            }
        }
    }

    public void MuteSFX()
    {
        isSFXMuted = !isSFXMuted;
    }
}

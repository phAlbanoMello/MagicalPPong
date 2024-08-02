using UnityEngine;
using System.Collections.Generic;

public class SoundManager : MonoBehaviour
{
    public List<SoundLibrary> soundLibraries;
    
    [Range(0f, 1f)]
    public float sfxVolume = 1.0f;

    private AudioSource audioSource;

    public void Init()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlaySound(string key)
    {
        SoundEntry soundEntry = GetSoundEntryByKey(key);
        if (soundEntry != null)
        {
            audioSource.clip = soundEntry.sound;
            audioSource.volume = sfxVolume;
            audioSource.pitch = 1.0f;
            audioSource.Play();
        }
        else
        {
            Debug.LogWarning("Sound with key " + key + " not found in any library.");
        }
    }

    private SoundEntry GetSoundEntryByKey(string key)
    {
        foreach (SoundLibrary library in soundLibraries)
        {
            foreach (SoundEntry entry in library.soundEntries)
            {
                if (entry.key == key)
                {
                    return entry;
                }
            }
        }
        return null;
    }

    public void SetSFXVolume(float volume)
    {
        sfxVolume = Mathf.Clamp01(volume);
    }
}


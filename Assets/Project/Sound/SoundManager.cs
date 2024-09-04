using UnityEngine;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{
    public List<SoundLibrary> soundLibraries;
    
    [Range(0f, 1f)]
    public float sfxVolume = 0f;

    private AudioSource audioSource;

    public void Init()
    {
        audioSource = GetComponent<AudioSource>();
        sfxVolume = GameSettings.Instance.Volume;
    }

    public void UpdateVolume()
    {
        sfxVolume = GameSettings.Instance.Volume;
    }

    public void PlaySound(string key)
    {
        SoundEntry soundEntry = GetSoundEntryByKey(key);
        if (soundEntry != null)
        {
            audioSource.clip = soundEntry.sound;
            audioSource.volume = soundEntry.volume * sfxVolume;
            audioSource.pitch = 1.0f;
            audioSource.Play();
        }
        else
        {
            Debug.LogWarning("Sound with key " + key + " not found in any library.");
        }
    }

    public void AddHoverSound(Button button, string soundID)
    {
        EventTrigger trigger = button.gameObject.AddComponent<EventTrigger>();

        EventTrigger.Entry entry = new EventTrigger.Entry
        {
            eventID = EventTriggerType.PointerEnter
        };
        entry.callback.AddListener((data) => { PlaySound(soundID); });
        trigger.triggers.Add(entry);
    }

    public void AddButtonSoundOnEvent(Button button, string soundID, EventTriggerType eventTriggerType){
        EventTrigger trigger = button.gameObject.AddComponent<EventTrigger>();

        EventTrigger.Entry entry = new EventTrigger.Entry
        {
            eventID = eventTriggerType
        };
        entry.callback.AddListener((data) => {
            if (!button.interactable)
                return;
            PlaySound(soundID); 
        });
        trigger.triggers.Add(entry);
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


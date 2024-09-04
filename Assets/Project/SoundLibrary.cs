using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class SoundEntry
{
    public string key;
    public AudioClip sound;
    public float volume;
}

[CreateAssetMenu(fileName = "NewSoundLibrary", menuName = "Sound Library", order = 51)]
public class SoundLibrary : ScriptableObject
{
    public List<SoundEntry> soundEntries;
}

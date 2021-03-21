using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AudioEventListener
{
    public enum AudioType
    {
        OneShot,
        Music,
        RandomOneShot
    }

    public AudioType type;

    public GameEvent gameEvent;
    public AudioClip[] audioClips;
    public float delay;
    [Range(0,1)]
    public float volume;
    [Tooltip("Time during which no other audio events of same type won't be played")]
    [Range(0, 1)]
    public float replayTreshold = 0.01f;
    [HideInInspector]
    public float currentReplayTreshold;
}

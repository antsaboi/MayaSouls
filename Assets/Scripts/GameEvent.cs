using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class GameEvent : ScriptableObject
{
    private List<GameEventListener> listeners = new List<GameEventListener>();
    private List<AudioEventListener> audioListeners = new List<AudioEventListener>();

    public void Raise()
    {
        for (int i = listeners.Count - 1; i >= 0; i--)
        {
            listeners[i].OnEventRaised();
        }

        if (AudioManager.Instance == null)
        {
            return;
        }
        for (int i = 0; i < audioListeners.Count; i++)
        {
            if (audioListeners[i].replayTreshold == 0 || Time.unscaledTime > audioListeners[i].currentReplayTreshold)
            {
                AudioManager.Instance.audioList.RaiseEvent(audioListeners[i]);
                audioListeners[i].currentReplayTreshold = Time.unscaledTime + audioListeners[i].replayTreshold;
            }
            else {
                Debug.Log("Audio event not played due to treshold");
            }
        }
    }

    public void ClearAudioListeners()
    {
        audioListeners.Clear();
    }

    public void RegisterListener(AudioEventListener listener)
    {
        listener.currentReplayTreshold = 0f;
        audioListeners.Add(listener);
    }

    public void Unregister(AudioEventListener listener)
    {
        listener.currentReplayTreshold = 0f;
        audioListeners.Remove(listener);
    }

    public void RegisterListener(GameEventListener listener)
    {
        listeners.Add(listener);
    }

    public void Unregister(GameEventListener listener)
    {
        listeners.Remove(listener);
    }
}

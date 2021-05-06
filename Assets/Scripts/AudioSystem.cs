using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioSystem : MonoBehaviour
{
    public static AudioSystem instance;
    public AudioClip menuMusic;
    public AudioClip gameMusic;

    AudioSource source;

    // Start is called before the first frame update
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else {
            Destroy(gameObject);
        }

        source = GetComponent<AudioSource>();
    }

    public void PlayOneShot(AudioClip clip, float delay = 0)
    {
        if (delay == 0)
        {
            source.PlayOneShot(clip);
        }
    }

    public void PlayMenuMusic()
    {
        source.clip = menuMusic;
        source.Play();
    }

    public void PlayGameMusic()
    {
        source.clip = gameMusic;
        source.Play();
    }
}

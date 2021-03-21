using UnityEngine;
using System.Collections;
#if UNITY_EDITOR
using UnityEditor;
#endif

[RequireComponent(typeof(AudioSource))]
public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;
    [Range(0,1)]
    public float startVolume;
    public AudioEventListenerList audioList;
    public AudioClip menuMusic, gameMusic;
    private AudioSource _source;
    private Coroutine playAudio, playMusic, playRandomAudio;

    //Unnecessary if lives under game manager
/*    public AudioManager Instance()
    {
        if (_instance != null)
        {
            Destroy(gameObject);
        } else {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }

        return _instance;
    }*/

    private void Awake()
    {
        Instance = this;
        audioList.RegisterEvents(this);
        _source = GetComponent<AudioSource>();
        _source.volume = startVolume;
        //PlayMenuMusic();
    }

    public void PlayClip(AudioClip clip)
    {
        _source.PlayOneShot(clip);
    }

    public void PlayGameMusic()
    {
        _source.clip = gameMusic;
        _source.Play();
    }

    public void PlayMenuMusic()
    {
        _source.clip = menuMusic;
        _source.Play();
    }

    public void PlayAudioShot(AudioEventListener listener, bool random = false)
    {
        for (int i = 0; i < listener.audioClips.Length; i++)
        {
            if (listener.audioClips[i] == null)
            {
                listener.gameEvent.Unregister(listener);
                return;
            }
        }

        if (random)
        {
            if (listener.delay == 0)
            {
                PlayRandomAudio(listener);
                return;
            }

            if (playRandomAudio != null) StopCoroutine(playRandomAudio);
            playRandomAudio = StartCoroutine(PlayRandomAudioDelayed(listener));
        }
        else
        {
            if (listener.delay == 0)
            {
                PlayAudio(listener);
                return;
            }

            if (playAudio != null) StopCoroutine(playAudio);
            playAudio = StartCoroutine(PlayAudioDelayed(listener));
        }
    }

    private void PlayAudio(AudioEventListener listener)
    {
        for (int i = 0; i < listener.audioClips.Length; i++)
        {
            _source.PlayOneShot(listener.audioClips[i], listener.volume);
        }
    }

    private void PlayRandomAudio(AudioEventListener listener)
    {
        AudioClip c = listener.audioClips[Random.Range(0, listener.audioClips.Length)];

        _source.PlayOneShot(c, listener.volume);
    }

    private IEnumerator PlayAudioDelayed(AudioEventListener listener)
    {
        yield return new WaitForSeconds(listener.delay);

        for (int i = 0; i < listener.audioClips.Length; i++)
        {
            _source.PlayOneShot(listener.audioClips[i], listener.volume);
        }
    }

    private IEnumerator PlayRandomAudioDelayed(AudioEventListener listener)
    {
        yield return new WaitForSeconds(listener.delay);

        AudioClip c = listener.audioClips[Random.Range(0, listener.audioClips.Length)];

        _source.PlayOneShot(c, listener.volume);
    }

    private void PlayMusicClip(AudioEventListener listener)
    {
        _source.clip = listener.audioClips[0];
        _source.volume = CalculateVolume(listener.volume);
        _source.Play();
    }

    public void PlayMusic(AudioEventListener listener)
    {
        if (listener.delay == 0)
        {
            PlayMusicClip(listener);
            return;
        }

        if (playMusic != null) StopCoroutine(playMusic);

        playMusic = StartCoroutine(PlayMusicDelayed(listener));
    }

    private IEnumerator PlayMusicDelayed(AudioEventListener listener)
    {
        yield return new WaitForSeconds(listener.delay);

        _source.clip = listener.audioClips[0];
        _source.volume = CalculateVolume(listener.volume);
        _source.Play();
    }

    private float CalculateVolume(float volume)
    {
        return volume * startVolume;
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(AudioManager))]
public class AudioManagerEditor : Editor
{
    AudioManager _base;

    private void OnEnable()
    {
        _base = (AudioManager)target;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
    }
}
#endif

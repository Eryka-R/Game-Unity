using UnityEngine;
using System.Collections;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance { get; private set; }
    private AudioSource soundSource;
    private AudioSource musicSource;

    private AudioClip previousMusicClip;
    private float previousMusicTime;
    private bool wasMusicPlayingBeforeOverride;
    private Coroutine temporaryMusicCoroutine;

    private void Awake()
    {
        soundSource = GetComponent<AudioSource>();
        musicSource = transform.GetChild(0).GetComponent<AudioSource>();

        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != this)
        {
            Destroy(gameObject);
            return;
        }

        float soundVolume = PlayerPrefs.GetFloat("SoundVolume", 1f);
        float musicVolume = PlayerPrefs.GetFloat("MusicVolume", 1f);

        soundSource.volume = soundVolume;
        musicSource.volume = musicVolume;
        
    }

    public void PlaySound(AudioClip _sound)
    {
        soundSource.PlayOneShot(_sound);
    }

    public void StopSound()
    {
        soundSource.Stop();
    }

    public void PauseSound()
    {
        soundSource.Pause();
    }

    public void ResumeSound()
    {
        soundSource.UnPause(); 
    }

    public void ChangeSoundVolume(float _change)
    {
        float baseVolume = 1f;
        ChangeSourceVolume(baseVolume, "SoundVolume", _change, soundSource);
    }

    public void ChangeMusicVolume(float _change)
    {
        float baseVolume = 1f;
        ChangeSourceVolume(baseVolume, "MusicVolume", _change, musicSource);
    }

    private void ChangeSourceVolume(float baseVolume, string volumeName, float _change, AudioSource source)
    { 
        float currentVolume = PlayerPrefs.GetFloat(volumeName, 1);
        bool currentVolumeIsMax = currentVolume == 1;
        currentVolume += _change;

        if (currentVolume < 0){
            currentVolume = 0;
        } else if (currentVolume > 1 && !currentVolumeIsMax) {
            currentVolume = 1;
        } else if (currentVolumeIsMax) {
            currentVolume = 0;
        }

        float finalVolume = currentVolume * baseVolume;

        source.volume = finalVolume;
        PlayerPrefs.SetFloat(volumeName, finalVolume);


    }

    public void PlayMusic(AudioClip clip, bool loop = true)
    {
        if (clip == null) return;

        musicSource.clip = clip;
        musicSource.loop = loop;
        musicSource.time = 0f;
        musicSource.Play();
    }

    public void StopMusic()
    {
        musicSource.Stop();
    }

    public void PauseMusic()
    {
        musicSource.Pause();
    }

    public void ResumeMusic()
    {
        musicSource.UnPause();
    }

    public void PlayTemporaryMusic(AudioClip temporaryClip, bool loopTemporary = false)
    {
        if (temporaryClip == null) return;

        if (temporaryMusicCoroutine != null)
        {
            StopCoroutine(temporaryMusicCoroutine);
            temporaryMusicCoroutine = null;
        }

        previousMusicClip = musicSource.clip;
        previousMusicTime = musicSource.time;
        wasMusicPlayingBeforeOverride = musicSource.isPlaying;

        musicSource.Stop();
        musicSource.clip = temporaryClip;
        musicSource.loop = loopTemporary;
        musicSource.time = 0f;
        musicSource.Play();

        if (!loopTemporary)
        {
            temporaryMusicCoroutine = StartCoroutine(RestorePreviousMusicWhenFinished(temporaryClip.length));
        }
    }

    public void RestorePreviousMusic()
    {
        if (temporaryMusicCoroutine != null)
        {
            StopCoroutine(temporaryMusicCoroutine);
            temporaryMusicCoroutine = null;
        }

        if (previousMusicClip == null)
        {
            musicSource.Stop();
            return;
        }

        musicSource.Stop();
        musicSource.clip = previousMusicClip;
        musicSource.loop = true;
        musicSource.time = previousMusicTime;

        if (wasMusicPlayingBeforeOverride)
        {
            musicSource.Play();
        }
    }

    private IEnumerator RestorePreviousMusicWhenFinished(float duration)
    {
        yield return new WaitForSecondsRealtime(duration);
        RestorePreviousMusic();
    }
}

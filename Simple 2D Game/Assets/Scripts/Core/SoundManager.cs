using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance { get; private set; }
    private AudioSource soundSource;
    private AudioSource musicSource;

    private void Awake()
    {
        soundSource = GetComponent<AudioSource>();
        musicSource = transform.GetChild(0).GetComponent<AudioSource>();

        // Keep the SoundManager alive across scenes
        if (instance != this){
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != this && instance != null){
            Destroy(gameObject);
        }

        ChangeSoundVolume(0);
        ChangeMusicVolume(0);
        
    }

    public void PlaySound(AudioClip _sound)
    {
        soundSource.PlayOneShot(_sound);
    }

    public void StopSound()
    {
        soundSource.Stop();
    }

    public void ChangeSoundVolume(float _change)
    {
        float baseVolume = 1f;
        ChangeSourceVolume(baseVolume, "SoundVolume", _change, soundSource);
    }

    private void ChangeSourceVolume(float baseVolume, string volumeName, float _change, AudioSource source)
    {
        float currentVolume = PlayerPrefs.GetFloat(volumeName, 1);
        currentVolume += _change;

        if (currentVolume < 0){
            currentVolume = 1;
        } else if (currentVolume > 1){
            currentVolume = 0;
        }

        float finalVolume = currentVolume * baseVolume;
        source.volume = finalVolume;
        PlayerPrefs.SetFloat(volumeName, currentVolume);


    }


    public void ChangeMusicVolume(float _change)
    {
        float baseVolume = 0.3f;
        ChangeSourceVolume(baseVolume, "MusicVolume", _change, musicSource);
    }

    public void PauseSound()
    {
        soundSource.Pause();
    }

    public void ResumeSound()
    {
        soundSource.UnPause(); 
    }

}

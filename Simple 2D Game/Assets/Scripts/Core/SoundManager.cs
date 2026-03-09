using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance { get; private set; }
    private AudioSource source;

    private void Awake()
    {
        source = GetComponent<AudioSource>();

        // Keep the SoundManager alive across scenes
        if (instance != this){
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != this && instance != null){
            Destroy(gameObject);
        }
        
    }

    public void PlaySound(AudioClip _sound)
    {
        source.PlayOneShot(_sound);
    }

    public void StopSound()
    {
        source.Stop();
    }

}

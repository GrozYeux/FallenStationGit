using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class SoundManager : AbstractSingleton<SoundManager>
{
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private AudioSource musicSource, effectSource;

    [Header("Music")]
    [SerializeField] public AudioClip noMusic;

    [Header("Movement")]
    [SerializeField] public AudioClip[] footStepAudioClips;

    [Header("Gun")]
    [SerializeField] public AudioClip shootClip;
    [SerializeField] public AudioClip lastShootClip;
    [SerializeField] public AudioClip[] noAmmoShootClips;
    [SerializeField] public AudioClip[] removeGunClips;
    [SerializeField] public AudioClip[] addGunClips;

    [Header("Warp")]
    [SerializeField] public AudioClip warpPastClip;
    [SerializeField] public AudioClip warpFutureClip;

    [Header("Pickup")]
    [SerializeField] public AudioClip[] pickupClips;

    [Header("Damage Sound")]
    [SerializeField] public AudioClip[] damageClips;

    public void PlaySound(AudioClip audio)
    {
        effectSource.PlayOneShot(audio);
    }

    public void PlayRandomSound(AudioClip[] audioArray)
    {
        effectSource.PlayOneShot(audioArray[Random.Range(0, audioArray.Length)]);
    }

    public void PlayRandomSound(AudioClip[] audioArray, float volume)
    {
        effectSource.PlayOneShot(audioArray[Random.Range(0, audioArray.Length)], volume);
    }

    public void PlayMusic(AudioClip music)
    {
        if (musicSource.isPlaying)
        {
            musicSource.Stop();
            musicSource.clip = default;
        }
        musicSource.clip = music;
        musicSource.Play();
    }

    public void StopMusic()
    {
        musicSource.Stop();
        musicSource.clip = default;
    }

    public void ChangeMasterVolume(float value)
    {
        audioMixer.SetFloat("MasterVolume", value);
    }

    public void ChangeMusicVolume(float value)
    {
        audioMixer.SetFloat("MusicVolume", value);
    }

    public void ChangeEffectVolume(float value)
    {
        audioMixer.SetFloat("SFXVolume", value);
    }

    private void Start()
    {
        PlayMusic(noMusic);
    }
}

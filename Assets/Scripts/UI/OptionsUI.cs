using System;
using UnityEngine;
using UnityEngine.UI;

public class OptionsUI : MonoBehaviour
{
    private AudioSource sampleAudioSource;

    [SerializeField]
    private Slider masterSlider;

    [SerializeField]
    private Slider musicSlider;

    [SerializeField]
    private Slider sfxSlider;

    [SerializeField]
    private AudioClip sampleSFX;

    public static EventHandler<float> OnMasterVolumeUpdated;
    public static EventHandler<float> OnMusicVolumeUpdated;
    public static EventHandler<float> OnSFXVolumeUpdated;

    private void OnEnable()
    {
        masterSlider.value = PlayerOptions.GetMasterVolume();
        musicSlider.value = PlayerOptions.GetMusicVolume();
        sfxSlider.value = PlayerOptions.GetSFXVolume();

        sampleAudioSource = GetComponent<AudioSource>();
    }

    public void SetMasterVolume(float newVolume)
    {
        PlayerOptions.SetMasterVolume(newVolume);
        OnMasterVolumeUpdated?.Invoke(this, newVolume);
    }

    public void SetMusicVolume(float newVolume)
    {
        PlayerOptions.SetMusicVolume(newVolume);
        OnMusicVolumeUpdated?.Invoke(this, newVolume);
    }

    public void SetSFXVolume(float newVolume)
    {
        PlayerOptions.SetSFXVolume(newVolume);

        OnSFXVolumeUpdated?.Invoke(this, newVolume);

        if (!sampleAudioSource)
        {
            return;
        }

        sampleAudioSource.clip = sampleSFX;
        sampleAudioSource.volume = PlayerOptions.GetSFXVolume();
        sampleAudioSource.Play();
    }
}

using UnityEngine;

public class MusicSource : MonoBehaviour
{
    private bool fade = false;

    private bool musicActive = false;
    private float targetVolume = 0f;
    private float maxVolume = 0.3f;
    private float fadeSpeed = 0.5f;

    [SerializeField]
    private AudioSource audioSource;

    private void Awake()
    {
        audioSource.volume = 0f;
    }

    private void Update()
    {
        if (!fade)
        {
            return;
        }

        FadeVolume();
    }

    private void FadeVolume()
    {
        audioSource.volume = Mathf.MoveTowards(
            audioSource.volume,
            targetVolume,
            fadeSpeed * Time.unscaledDeltaTime
        );

        if (Mathf.Abs(targetVolume - audioSource.volume) < 0.01f)
        {
            fade = false;
        }
    }

    public void SetMusicTrack(AudioClip musicClip)
    {
        audioSource.clip = musicClip;
        audioSource.Play();
    }

    public void StartMusic()
    {
        if (musicActive)
        {
            return;
        }

        targetVolume = maxVolume;
        fade = true;
        musicActive = true;
    }

    public void StopMusic()
    {
        if (!musicActive)
        {
            return;
        }

        targetVolume = 0f;
        fade = true;

        musicActive = false;
    }
}

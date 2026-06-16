using UnityEngine;

public class PlayerOptions
{
    private const string MASTER_VOLUME = "MasterVolume";
    private const string MUSIC_VOLUME = "MusicVolume";
    private const string SFX_VOLUME = "SFXVolume";

    private static float MASTER_VOLUME_DEF = 0.5f;
    private static float MUSIC_VOLUME_DEF = 0.5f;
    private static float SFX_VOLUME_DEF = 0.5f;

    public static void SetMasterVolume(float newVolume)
    {
        //MASTER_VOLUME_DEF = newVolume;
        PlayerPrefs.SetFloat(MASTER_VOLUME, newVolume);
        PlayerPrefs.Save();
    }

    public static void SetMusicVolume(float newVolume)
    {
        //MUSIC_VOLUME_DEF = newVolume;
        PlayerPrefs.SetFloat(MUSIC_VOLUME, newVolume);
        PlayerPrefs.Save();
    }

    public static void SetSFXVolume(float newVolume)
    {
        //SFX_VOLUME_DEF = newVolume;
        PlayerPrefs.SetFloat(SFX_VOLUME, newVolume);
        PlayerPrefs.Save();
    }

    public static float GetMasterVolume()
    {
        if (!PlayerPrefs.HasKey(MASTER_VOLUME))
        {
            return MASTER_VOLUME_DEF;
        }
        else
        {
            return PlayerPrefs.GetFloat(MASTER_VOLUME);
        }
    }

    public static float GetMusicVolume()
    {
        if (!PlayerPrefs.HasKey(MUSIC_VOLUME))
        {
            return MUSIC_VOLUME_DEF;
        }
        else
        {
            return PlayerPrefs.GetFloat(MUSIC_VOLUME);
        }
    }

    public static float GetSFXVolume()
    {
        if (!PlayerPrefs.HasKey(SFX_VOLUME))
        {
            return SFX_VOLUME_DEF;
        }
        else
        {
            return PlayerPrefs.GetFloat(SFX_VOLUME);
        }
    }
}

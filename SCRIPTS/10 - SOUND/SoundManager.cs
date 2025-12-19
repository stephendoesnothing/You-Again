using UnityEngine;
using System.Collections.Generic;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    [Header("Audio Sources")]
    public AudioSource musicSource;
    public AudioSource sfxSourcePrefab;

    [Header("Audio Clips")]
    public AudioClip backgroundMusic;
    public List<SoundEffect> soundEffects;

    private Dictionary<string, AudioClip> sfxDictionary;

    void Awake()
    {
        // Singleton
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        // Initialize SFX dictionary
        sfxDictionary = new Dictionary<string, AudioClip>();
        foreach (var sfx in soundEffects)
        {
            if (!sfxDictionary.ContainsKey(sfx.name))
                sfxDictionary.Add(sfx.name, sfx.clip);
        }

        // Play background music
        if (backgroundMusic != null)
        {
            musicSource.clip = backgroundMusic;
            musicSource.loop = true;
            musicSource.Play();
        }
    }

    // Play SFX at a position (2D or 3D)
    public void PlaySFX(string name, Vector3 position, float volume = 1f)
    {
        if (!sfxDictionary.ContainsKey(name))
        {
            Debug.LogWarning($"SoundManager: Sound '{name}' not found!");
            return;
        }

        AudioSource sfxInstance = Instantiate(sfxSourcePrefab, position, Quaternion.identity);
        sfxInstance.clip = sfxDictionary[name];
        sfxInstance.volume = volume;
        sfxInstance.Play();

        Destroy(sfxInstance.gameObject, sfxInstance.clip.length);
    }

    // Play SFX at the camera (2D)
    public void PlaySFX(string name, float volume = 1f)
    {
        PlaySFX(name, Camera.main.transform.position, volume);
    }

    // Control background music
    public void PlayMusic(AudioClip music, bool loop = true)
    {
        musicSource.clip = music;
        musicSource.loop = loop;
        musicSource.Play();
    }

    public void StopMusic()
    {
        musicSource.Stop();
    }

    public void SetMusicVolume(float volume)
    {
        musicSource.volume = volume;
    }

    public void SetSFXVolume(float volume)
    {
        sfxSourcePrefab.volume = volume;
    }

    public AudioSource loopingSFX;

    public void PlayLoopingSFX(string name)
    {
        if (loopingSFX != null && loopingSFX.isPlaying) return;

        if (!sfxDictionary.ContainsKey(name))
        {
            Debug.LogWarning($"Looping SFX '{name}' not found!");
            return;
        }

        loopingSFX = Instantiate(sfxSourcePrefab, Camera.main.transform.position, Quaternion.identity);
        loopingSFX.clip = sfxDictionary[name];
        loopingSFX.loop = true;
        loopingSFX.Play();
    }

    public void StopLoopingSFX()
    {
        if (loopingSFX != null)
        {
            loopingSFX.Stop();
            Destroy(loopingSFX.gameObject);
            loopingSFX = null;
        }
    }
}

[System.Serializable]
public class SoundEffect
{
    public string name;
    public AudioClip clip;
}

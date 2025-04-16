using UnityEngine;
using UnityEngine.Audio;

public class BackgroundMusic : MonoBehaviour
{
    private static BackgroundMusic instance;
    
    [Header("Audio Settings")]
    public AudioClip musicClip;
    [Range(0f, 1f)]
    public float volume = 0.5f;
    [Range(0.1f, 2f)]
    public float fadeOutDuration = 0.5f;

    private AudioSource musicSource;
    private bool isFadingOut = false;
    private bool isMusicMuted = false;

    private void Awake()
    {
        // Singleton pattern to ensure only one music player exists
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        
        instance = this;
        DontDestroyOnLoad(gameObject);

        // Check if music should be muted
        isMusicMuted = PlayerPrefs.GetInt("MusicMuted", 0) == 1;
        Debug.Log($"BackgroundMusic initialized. Music muted: {isMusicMuted}");

        // Set up audio source
        SetupAudioSource();
    }

    private void SetupAudioSource()
    {
        musicSource = gameObject.AddComponent<AudioSource>();
        musicSource.clip = musicClip;
        musicSource.volume = volume;
        musicSource.loop = true;
        musicSource.playOnAwake = false; // Changed to false to control playback manually
        musicSource.priority = 128;
        musicSource.spatialBlend = 0f; // 2D sound

        if (musicClip != null && !isMusicMuted)
        {
            musicSource.Play();
            Debug.Log("Started playing background music");
        }
        else if (isMusicMuted)
        {
            Debug.Log("Background music not started because it's muted in settings");
        }
        else
        {
            Debug.LogWarning("No music clip assigned to BackgroundMusic!");
        }
    }

    private void Update()
    {
        // Check if game is over
        if (GameManager.IsGameOver && !isFadingOut)
        {
            StartFadeOut();
        }
        
        // Check if music setting has changed
        bool currentMuteSetting = PlayerPrefs.GetInt("MusicMuted", 0) == 1;
        if (currentMuteSetting != isMusicMuted)
        {
            isMusicMuted = currentMuteSetting;
            ApplyMusicSettings();
        }
    }
    
    private void ApplyMusicSettings()
    {
        if (musicSource != null)
        {
            if (isMusicMuted)
            {
                if (musicSource.isPlaying)
                {
                    musicSource.Stop();
                    Debug.Log("Stopped background music due to mute setting change");
                }
            }
            else
            {
                if (!musicSource.isPlaying && !isFadingOut)
                {
                    musicSource.volume = volume;
                    musicSource.Play();
                    Debug.Log("Started background music due to unmute setting change");
                }
            }
        }
    }

    private void StartFadeOut()
    {
        if (!isFadingOut && musicSource != null && musicSource.isPlaying)
        {
            isFadingOut = true;
            StartCoroutine(FadeOutMusic());
        }
    }

    private System.Collections.IEnumerator FadeOutMusic()
    {
        float startVolume = musicSource.volume;
        float elapsedTime = 0f;

        while (elapsedTime < fadeOutDuration)
        {
            elapsedTime += Time.unscaledDeltaTime; // Use unscaled time in case game is paused
            float newVolume = Mathf.Lerp(startVolume, 0f, elapsedTime / fadeOutDuration);
            musicSource.volume = newVolume;
            yield return null;
        }

        musicSource.Stop();
        musicSource.volume = startVolume; // Reset volume for next time
        isFadingOut = false;
    }

    public void RestartMusic()
    {
        if (musicSource != null && !isMusicMuted)
        {
            isFadingOut = false;
            musicSource.volume = volume;
            musicSource.Play();
        }
    }

    private void OnValidate()
    {
        if (musicSource != null && !isFadingOut && !isMusicMuted)
        {
            musicSource.volume = volume;
        }
    }
}

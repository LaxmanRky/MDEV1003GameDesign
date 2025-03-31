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

        // Set up audio source
        SetupAudioSource();
    }

    private void SetupAudioSource()
    {
        musicSource = gameObject.AddComponent<AudioSource>();
        musicSource.clip = musicClip;
        musicSource.volume = volume;
        musicSource.loop = true;
        musicSource.playOnAwake = true;
        musicSource.priority = 128;
        musicSource.spatialBlend = 0f; // 2D sound

        if (musicClip != null)
        {
            musicSource.Play();
            Debug.Log("Started playing background music");
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
        if (musicSource != null)
        {
            isFadingOut = false;
            musicSource.volume = volume;
            musicSource.Play();
        }
    }

    private void OnValidate()
    {
        if (musicSource != null && !isFadingOut)
        {
            musicSource.volume = volume;
        }
    }
}

using UnityEngine;
using SmallShips;  // Add namespace for ExplosionController

public class AudioManager : MonoBehaviour
{
    private static AudioManager instance;
    public static AudioManager Instance 
    { 
        get 
        {
            if (instance == null)
            {
                instance = FindFirstObjectByType<AudioManager>();
                if (instance == null)
                {
                    GameObject go = new GameObject("AudioManager");
                    instance = go.AddComponent<AudioManager>();
                }
            }
            return instance;
        }
    }

    // References to existing audio sources
    private AudioSource backgroundMusic;
    private SmallShips.ExplosionController explosionController;
    
    private bool isMusicMuted = false;
    private bool isEffectsMuted = false;
    private float savedMusicVolume = 1f;
    private float savedEffectsVolume = 1f;

    private void Awake()
    {
        // Ensure only one instance exists
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);

        // Load saved mute states
        isMusicMuted = PlayerPrefs.GetInt("MusicMuted", 0) == 1;
        isEffectsMuted = PlayerPrefs.GetInt("EffectsMuted", 0) == 1;
        
        Debug.Log($"AudioManager initialized with saved settings. Music muted: {isMusicMuted}, Effects muted: {isEffectsMuted}");
        
        // Find audio sources
        FindAudioSources();
        
        // CRITICAL: Force-stop any background music if it should be muted
        if (isMusicMuted && backgroundMusic != null)
        {
            backgroundMusic.Stop();
            backgroundMusic.volume = 0f;
            Debug.Log("Background music forcibly stopped at initialization");
        }
        
        // Apply saved settings immediately
        ApplyMusicMute(isMusicMuted);
        ApplyEffectsMute(isEffectsMuted);
    }

    private void OnEnable()
    {
        // Find and apply states when enabled (this helps when scenes change)
        FindAudioSources();
        
        // CRITICAL: Force-stop any background music if it should be muted
        if (isMusicMuted && backgroundMusic != null)
        {
            backgroundMusic.Stop();
            backgroundMusic.volume = 0f;
            Debug.Log("Background music forcibly stopped on enable");
        }
        
        ApplyMusicMute(isMusicMuted);
        ApplyEffectsMute(isEffectsMuted);
    }
    
    // NEW: Direct method to force-stop music
    public void ForceStopMusic()
    {
        if (backgroundMusic != null)
        {
            backgroundMusic.Stop();
            backgroundMusic.volume = 0f;
            Debug.Log("Background music forcibly stopped");
        }
    }

    private void FindAudioSources()
    {
        // Find background music
        var bgObject = GameObject.Find("BackgroundMusic");
        if (bgObject != null)
        {
            backgroundMusic = bgObject.GetComponent<AudioSource>();
            
            if (backgroundMusic != null)
            {
                // Store initial music volume if not already set
                if (savedMusicVolume == 1f)
                {
                    savedMusicVolume = backgroundMusic.volume;
                }
                
                // CRITICAL: Disable auto-play
                backgroundMusic.playOnAwake = false;
                
                // If music should be muted, ensure it's not playing
                if (isMusicMuted)
                {
                    backgroundMusic.Stop();
                    backgroundMusic.volume = 0f;
                    Debug.Log("Background music stopped because it should be muted");
                }
                else
                {
                    // If music should be playing, start it
                    backgroundMusic.volume = savedMusicVolume;
                    if (!backgroundMusic.isPlaying)
                    {
                        backgroundMusic.Play();
                        Debug.Log("Background music started because it should be playing");
                    }
                }
            }
        }

        // Find spaceship for explosion audio
        var ship = GameObject.Find("Spaceship");
        if (ship != null)
        {
            explosionController = ship.GetComponent<SmallShips.ExplosionController>();
            if (explosionController != null)
            {
                // Get the AudioSource from the ExplosionController
                var audioSources = explosionController.GetComponents<AudioSource>();
                foreach (var source in audioSources)
                {
                    if (source.clip != null && source.clip.name.ToLower().Contains("explosion"))
                    {
                        savedEffectsVolume = source.volume;
                        break;
                    }
                }
            }
        }
    }

    public void ToggleMusic()
    {
        isMusicMuted = !isMusicMuted;
        
        // CRITICAL: Apply change immediately and forcefully
        if (isMusicMuted && backgroundMusic != null && backgroundMusic.isPlaying)
        {
            backgroundMusic.Stop();
            backgroundMusic.volume = 0f;
        }
        
        ApplyMusicMute(isMusicMuted);
        PlayerPrefs.SetInt("MusicMuted", isMusicMuted ? 1 : 0);
        PlayerPrefs.Save();
        Debug.Log($"Music toggled. Muted: {isMusicMuted}");
    }

    public void ToggleEffects()
    {
        isEffectsMuted = !isEffectsMuted;
        ApplyEffectsMute(isEffectsMuted);
        PlayerPrefs.SetInt("EffectsMuted", isEffectsMuted ? 1 : 0);
        PlayerPrefs.Save();
        
        // Debug log to check if toggle is working
        Debug.Log($"Effects toggled. Muted: {isEffectsMuted}");
    }

    private void ApplyMusicMute(bool mute)
    {
        // Find sources if not set
        if (backgroundMusic == null)
        {
            FindAudioSources();
        }

        if (backgroundMusic != null)
        {
            if (mute)
            {
                // If muting, stop the music and set volume to 0
                backgroundMusic.volume = 0f;
                backgroundMusic.Stop();
                Debug.Log("Music muted and stopped");
            }
            else
            {
                // If unmuting, start the music and restore volume
                backgroundMusic.volume = savedMusicVolume;
                if (!backgroundMusic.isPlaying)
                {
                    backgroundMusic.Play();
                    Debug.Log("Music unmuted and started");
                }
            }
        }
    }

    private void ApplyEffectsMute(bool mute)
    {
        // Find sources if not set
        if (explosionController == null)
        {
            FindAudioSources();
        }

        if (explosionController != null)
        {
            // Apply mute to all audio sources in the ExplosionController
            var audioSources = explosionController.GetComponents<AudioSource>();
            foreach (var source in audioSources)
            {
                if (source.clip != null && source.clip.name.ToLower().Contains("explosion"))
                {
                    source.volume = mute ? 0f : savedEffectsVolume;
                }
            }
        }
    }

    public bool IsMusicMuted() { return isMusicMuted; }
    public bool IsEffectsMuted() { return isEffectsMuted; }
}

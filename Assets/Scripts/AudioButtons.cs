using UnityEngine;
using UnityEngine.UI;

public class AudioButtons : MonoBehaviour
{
    public Toggle musicToggle;
    public Toggle effectsToggle;

    private AudioManager audioManager;

    void Start()
    {
        // Find AudioManager in the scene
        audioManager = FindFirstObjectByType<AudioManager>();
        
        if (audioManager == null)
        {
            Debug.LogError("AudioManager not found in scene! Creating one...");
            GameObject managerObj = new GameObject("AudioManager");
            audioManager = managerObj.AddComponent<AudioManager>();
        }

        if (musicToggle == null || effectsToggle == null)
        {
            Debug.LogError("Toggle references not set in AudioButtons!");
            return;
        }

        // Remove existing listeners to prevent double-toggling
        musicToggle.onValueChanged.RemoveAllListeners();
        effectsToggle.onValueChanged.RemoveAllListeners();
        
        // Add listeners to toggles
        musicToggle.onValueChanged.AddListener(OnMusicToggle);
        effectsToggle.onValueChanged.AddListener(OnEffectsToggle);

        // Set initial states (inverted because isOn=true means audio should be on)
        bool musicMuted = PlayerPrefs.GetInt("MusicMuted", 0) == 1;
        bool effectsMuted = PlayerPrefs.GetInt("EffectsMuted", 0) == 1;
        
        musicToggle.isOn = !musicMuted;
        effectsToggle.isOn = !effectsMuted;
        
        Debug.Log($"Initial toggle states - Music: {musicToggle.isOn} (muted: {musicMuted}), Effects: {effectsToggle.isOn} (muted: {effectsMuted})");
        
        // Force-stop music if it should be muted
        if (musicMuted && audioManager != null)
        {
            audioManager.ForceStopMusic();
        }
    }

    void OnMusicToggle(bool isOn)
    {
        if (audioManager != null)
        {
            // If toggle is ON, music should NOT be muted
            // If toggle is OFF, music should be muted
            bool shouldBeMuted = !isOn;
            bool currentlyMuted = audioManager.IsMusicMuted();
            
            Debug.Log($"Music toggle changed to {isOn}, should be muted: {shouldBeMuted}, currently muted: {currentlyMuted}");
            
            // Only toggle if the current state doesn't match what we want
            if (currentlyMuted != shouldBeMuted)
            {
                audioManager.ToggleMusic();
                
                // Force-stop music if we're muting
                if (shouldBeMuted)
                {
                    audioManager.ForceStopMusic();
                }
            }
            
            // Save the state directly to PlayerPrefs as a backup
            PlayerPrefs.SetInt("MusicMuted", shouldBeMuted ? 1 : 0);
            PlayerPrefs.Save();
        }
    }

    void OnEffectsToggle(bool isOn)
    {
        if (audioManager != null)
        {
            // If toggle is ON, effects should NOT be muted
            // If toggle is OFF, effects should be muted
            bool shouldBeMuted = !isOn;
            bool currentlyMuted = audioManager.IsEffectsMuted();
            
            Debug.Log($"Effects toggle changed to {isOn}, should be muted: {shouldBeMuted}, currently muted: {currentlyMuted}");
            
            // Only toggle if the current state doesn't match what we want
            if (currentlyMuted != shouldBeMuted)
            {
                audioManager.ToggleEffects();
            }
            
            // Save the state directly to PlayerPrefs as a backup
            PlayerPrefs.SetInt("EffectsMuted", shouldBeMuted ? 1 : 0);
            PlayerPrefs.Save();
        }
    }
}

using UnityEngine;

public class AudioManager : MonoBehaviour
{
    private static AudioManager instance;
    private AudioListener audioListener;

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

        // Immediately ensure we have an audio listener
        EnsureAudioListener();
    }

    private void OnEnable()
    {
        // Check again when enabled
        EnsureAudioListener();
    }

    private void EnsureAudioListener()
    {
        // First check if we already have an AudioListener in the scene
        AudioListener[] listeners = FindObjectsByType<AudioListener>(FindObjectsSortMode.None);
        
        if (listeners.Length == 0)
        {
            // No listeners found, add one to the main camera or create a new one
            Camera mainCamera = Camera.main;
            if (mainCamera != null)
            {
                audioListener = mainCamera.gameObject.AddComponent<AudioListener>();
                Debug.Log("Added AudioListener to main camera");
            }
            else
            {
                // Create a dedicated audio listener object
                GameObject listenerObject = new GameObject("Audio Listener");
                audioListener = listenerObject.AddComponent<AudioListener>();
                DontDestroyOnLoad(listenerObject);
                Debug.Log("Created dedicated Audio Listener object");
            }
        }
        else
        {
            // We found at least one listener
            audioListener = listeners[0];
            
            // Remove any extra listeners
            if (listeners.Length > 1)
            {
                Debug.LogWarning("Found multiple AudioListeners. Removing extras.");
                for (int i = 1; i < listeners.Length; i++)
                {
                    Destroy(listeners[i]);
                }
            }
        }
    }

    private void Update()
    {
        // Continuously ensure we have a working audio listener
        if (audioListener == null)
        {
            Debug.LogWarning("AudioListener was lost. Recreating...");
            EnsureAudioListener();
        }
    }
}

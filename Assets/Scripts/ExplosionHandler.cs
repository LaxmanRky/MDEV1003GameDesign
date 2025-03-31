using UnityEngine;
using System.Collections;
using UnityEngine.Events;

[RequireComponent(typeof(AudioSource))] // This ensures the AudioSource is always present
public class ExplosionHandler : MonoBehaviour
{
    [Header("Explosion Settings")]
    public float explosionDuration = 1.0f;  // Duration of explosion animation
    public UnityEvent onExplosionComplete;  // Event triggered when explosion is done

    [Header("Audio Settings")]
    public AudioClip explosionSound;        // Reference to the explosion sound effect
    [Range(0f, 1f)]
    public float volume = 1f;               // Volume control for the explosion

    private bool isExploding = false;
    private bool gameStopRequested = false;
    private AudioSource audioSource;

    private void Awake()
    {
        // Initialize event if needed
        if (onExplosionComplete == null)
            onExplosionComplete = new UnityEvent();

        // Get the required AudioSource component
        audioSource = GetComponent<AudioSource>();
        
        // Configure AudioSource for explosion sounds
        audioSource.playOnAwake = false;    // Don't play on start
        audioSource.spatialBlend = 0f;      // Make it 2D sound
        audioSource.volume = volume;         // Set initial volume
        audioSource.priority = 0;           // High priority for explosion sound
        
        if (explosionSound != null)
        {
            audioSource.clip = explosionSound;
        }
        else
        {
            Debug.LogWarning("ExplosionHandler: No explosion sound assigned!");
        }
    }

    public void TriggerExplosion()
    {
        if (!isExploding)
        {
            isExploding = true;
            StartCoroutine(ExplosionSequence());
        }
    }

    private IEnumerator ExplosionSequence()
    {
        Debug.Log("Starting explosion sequence...");

        // Play explosion sound
        if (explosionSound != null)
        {
            audioSource.PlayOneShot(explosionSound, volume);
            Debug.Log("Playing explosion sound...");
        }

        // Wait for explosion animation to complete
        yield return new WaitForSecondsRealtime(explosionDuration);
        
        Debug.Log("Explosion sequence complete!");
        isExploding = false;
        
        // Request game stop on next frame
        gameStopRequested = true;
    }

    private void Update()
    {
        if (gameStopRequested)
        {
            gameStopRequested = false;
            onExplosionComplete.Invoke();
        }
    }

    // This runs in the editor to keep the volume updated
    private void OnValidate()
    {
        if (audioSource != null)
        {
            audioSource.volume = volume;
        }
    }
}

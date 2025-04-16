using UnityEngine;
using SmallShips;  // Add namespace for ExplosionController
using System.Collections;

// Add our custom namespace
namespace SpaceVoyager
{
    public class SpaceshipController : MonoBehaviour
    {
        public float gravity = 2.5f;        // Moderate downward pull
        public float thrustPower = 7f;      // Strong, responsive upward thrust
        public float maxThrust = 9f;        // Controlled maximum velocity
        public float dragFactor = 0.96f;    // Smooth, slightly more aggressive deceleration

        private Rigidbody2D rb;
        private bool isThrusting = false;   // To check if thrust is being applied
        private bool isGameOver = false;    // Track game over state

        // Static property to check game state from other scripts
        public static bool IsGameOver { get; private set; }

        // Reference to the ExplosionController component
        private ExplosionController explosionController;
        private ExplosionHandler explosionHandler;
        private AudioSource[] explosionAudioSources;
        private bool isExploding = false;

        void Start()
        {
            // Load saved settings
            gravity = PlayerPrefs.GetFloat("Gravity", gravity);
            thrustPower = PlayerPrefs.GetFloat("ThrustPower", thrustPower);
            maxThrust = thrustPower + 2f; // Adjust max thrust based on thrust power

            // Automatically add Rigidbody2D if missing
            rb = GetComponent<Rigidbody2D>();
            if (rb == null)
            {
                rb = gameObject.AddComponent<Rigidbody2D>();
                Debug.LogWarning("Rigidbody2D was automatically added to the game object.");
            }
            
            // Try to get ExplosionController if not manually assigned
            explosionController = GetComponent<ExplosionController>();
            if (explosionController != null)
            {
                explosionController.keepAlive = true; // Set this to keep explosion visible
            }
            
            // Try to get or add ExplosionHandler
            explosionHandler = GetComponent<ExplosionHandler>();
            if (explosionHandler == null)
            {
                explosionHandler = gameObject.AddComponent<ExplosionHandler>();
                Debug.Log("ExplosionHandler was automatically added to the game object.");
            }
            
            // Set up the explosion complete event listener
            explosionHandler.onExplosionComplete.RemoveListener(OnExplosionComplete); // Remove any existing listener first
            explosionHandler.onExplosionComplete.AddListener(OnExplosionComplete);
            Debug.Log("Explosion complete event listener set up successfully.");
            
            rb.gravityScale = 0; // We manually handle gravity
            
            IsGameOver = false; // Reset static state when game starts

            // Store all audio sources that might be used for explosion
            explosionAudioSources = GetComponents<AudioSource>();
        }

        void Update()
        {
            // Prevent input and movement if game is over
            if (isGameOver) return;

            // Check for Input (Keyboard or Touch)
            if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow) || Input.GetMouseButton(0))
            {
                isThrusting = true;
            }
            else
            {
                isThrusting = false;
            }
        }

        void FixedUpdate()
        {
            // Prevent movement if game is over
            if (isGameOver || rb == null) return;

            // Apply custom gravity
            rb.linearVelocity += Vector2.down * gravity * Time.fixedDeltaTime;

            // Apply thrust if pressing the key or touching
            if (isThrusting)
            {
                rb.linearVelocity += Vector2.up * thrustPower * Time.fixedDeltaTime;
                
                // Limit max upward speed
                if (rb.linearVelocity.y > maxThrust)
                {
                    rb.linearVelocity = new Vector2(rb.linearVelocity.x, maxThrust);
                }
            }
            
            // Apply drag so that thrust feels smooth and natural
            rb.linearVelocity *= dragFactor;
        }

        void OnCollisionEnter2D(Collision2D collision)
        {
            // Check if the game is not already over and not already exploding
            if (!isGameOver && !isExploding)
            {
                // Set both instance and static game over states
                isGameOver = true;
                IsGameOver = true;
                isExploding = true;
                
                // Trigger game over
                Debug.Log("Game Over! Spaceship collided with an object.");

                // Stop physics movement and disable game controls
                rb.linearVelocity = Vector2.zero;
                rb.simulated = false;  // This stops physics simulation but keeps the object visible

                // Find and stop all asteroids (using non-deprecated method)
                var asteroids = FindObjectsByType<AsteroidMovement>(FindObjectsSortMode.None);
                foreach (var asteroid in asteroids)
                {
                    if (asteroid.enabled)  // Only process active asteroids
                    {
                        asteroid.enabled = false;  // This will stop the Update loop
                        var asteroidRb = asteroid.GetComponent<Rigidbody2D>();
                        if (asteroidRb != null)
                        {
                            asteroidRb.simulated = false;
                        }
                    }
                }

                // Check if effects are muted before playing explosion
                bool effectsMuted = AudioManager.Instance != null && AudioManager.Instance.IsEffectsMuted();
                
                // If effects are not muted, trigger explosion with sound
                if (!effectsMuted)
                {
                    // Trigger explosion if ExplosionController is available
                    if (explosionController != null)
                    {
                        explosionController.StartExplosion();
                    }
                }
                else
                {
                    // If effects are muted, mute all explosion-related audio sources first
                    if (explosionAudioSources != null)
                    {
                        foreach (var source in explosionAudioSources)
                        {
                            if (source != null)
                            {
                                source.volume = 0f;
                            }
                        }
                    }
                    
                    // Then trigger the explosion (for visual effect only)
                    if (explosionController != null)
                    {
                        explosionController.StartExplosion();
                    }
                }

                // Always use ExplosionHandler since we auto-add it in Start
                Debug.Log("Triggering explosion handler...");
                explosionHandler.TriggerExplosion();

                // Start looking for the explosion object to add our handler
                StartCoroutine(FindAndAddExplosionHandler());
            }
        }

        private IEnumerator FindAndAddExplosionHandler()
        {
            GameObject explosionObj = null;
            float searchTime = 0f;
            float maxSearchTime = 2f;

            // Keep searching for the explosion object until we find it or timeout
            while (explosionObj == null && searchTime < maxSearchTime)
            {
                // Look for an object with "explosion" in its name that was recently created
                var explosions = FindObjectsByType<Animator>(FindObjectsSortMode.None);
                foreach (var anim in explosions)
                {
                    if (anim.gameObject.name.ToLower().Contains("explosion") && 
                        anim.gameObject.GetComponent<ExplosionAnimationHandler>() == null)
                    {
                        explosionObj = anim.gameObject;
                        Debug.Log($"Found explosion object: {explosionObj.name}");
                        
                        // Add our handler component
                        explosionObj.AddComponent<ExplosionAnimationHandler>();
                        Debug.Log("Added ExplosionAnimationHandler to explosion object");
                        yield break;
                    }
                }

                searchTime += 0.1f;
                yield return new WaitForSeconds(0.1f);
            }

            if (explosionObj == null)
            {
                Debug.LogWarning("Could not find explosion object");
            }
        }

        private void OnExplosionComplete()
        {
            Debug.Log("Explosion complete event received");
        }

        private void StopGame()
        {
            // Removed Time.timeScale = 0 to keep music playing
            Debug.Log("Game stopped, explosion frozen.");
        }

        private void OnDestroy()
        {
            // Clean up event listener when object is destroyed
            if (explosionHandler != null)
            {
                explosionHandler.onExplosionComplete.RemoveListener(OnExplosionComplete);
            }
        }
    }
}

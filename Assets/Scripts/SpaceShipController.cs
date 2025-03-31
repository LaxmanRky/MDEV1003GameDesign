using UnityEngine;
using SmallShips;  // Add namespace for ExplosionController

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

    void Start()
    {
        // Automatically add Rigidbody2D if missing
        rb = GetComponent<Rigidbody2D>();
        if (rb == null)
        {
            rb = gameObject.AddComponent<Rigidbody2D>();
            Debug.LogWarning("Rigidbody2D was automatically added to the game object.");
        }
        
        // Try to get ExplosionController if not manually assigned
        explosionController = GetComponent<ExplosionController>();
        
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
        // Check if the game is not already over
        if (!isGameOver)
        {
            // Set both instance and static game over states
            isGameOver = true;
            IsGameOver = true;
            
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

            // Trigger explosion if ExplosionController is available
            if (explosionController != null)
            {
                explosionController.StartExplosion();
            }

            // Always use ExplosionHandler since we auto-add it in Start
            Debug.Log("Triggering explosion handler...");
            explosionHandler.TriggerExplosion();
        }
    }

    private void OnExplosionComplete()
    {
        Debug.Log("Explosion complete, stopping game...");
        StopGame();
    }

    private void StopGame()
    {
        Debug.Log("Game stopped. Setting timeScale to 0.");
        Time.timeScale = 0;
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

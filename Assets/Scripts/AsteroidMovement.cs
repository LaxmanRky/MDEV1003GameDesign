using UnityEngine;

public class AsteroidMovement : MonoBehaviour
{
    public float moveSpeed = 6f;
    public bool useScaleAnimation = false;
    public float scaleMin = 0.8f;
    public float scaleMax = 1.2f;
    public float scaleDuration = 2f;

    private Rigidbody2D rb;
    private bool isStopped = false;
    private Vector3 originalScale;
    private float scaleTimer = 0f;
    private CircleCollider2D circleCollider;
    private BoxCollider2D boxCollider;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        if (rb == null)
        {
            rb = gameObject.AddComponent<Rigidbody2D>();
        }

        // Store original scale
        originalScale = transform.localScale;
        
        // Set up physics
        rb.gravityScale = 0;
        rb.interpolation = RigidbodyInterpolation2D.Interpolate; // Use interpolation for smoother movement
        
        // Get colliders
        circleCollider = GetComponent<CircleCollider2D>();
        boxCollider = GetComponent<BoxCollider2D>();
        
        // If no collider exists, add a circle collider
        if (circleCollider == null && boxCollider == null)
        {
            circleCollider = gameObject.AddComponent<CircleCollider2D>();
        }

        // Configure collider(s)
        if (circleCollider != null)
        {
            circleCollider.isTrigger = false;
        }
        if (boxCollider != null)
        {
            boxCollider.isTrigger = false;
        }
    }

    void Update()
    {
        if (SpaceshipController.IsGameOver)
        {
            if (!isStopped)
            {
                StopAsteroid();
            }
            return;
        }

        // Handle movement
        transform.position += Vector3.left * moveSpeed * Time.deltaTime;

        // Handle scale animation
        if (useScaleAnimation)
        {
            scaleTimer += Time.deltaTime;
            float scaleProgress = Mathf.PingPong(scaleTimer / scaleDuration, 1f);
            float currentScale = Mathf.Lerp(scaleMin, scaleMax, scaleProgress);
            transform.localScale = originalScale * currentScale;

            // Update collider size if it's a circle collider
            if (circleCollider != null)
            {
                circleCollider.radius = circleCollider.radius * currentScale;
            }
        }

        // Destroy if offscreen
        if (transform.position.x < -15)
        {
            Destroy(gameObject);
        }
    }

    private void StopAsteroid()
    {
        isStopped = true;
        if (rb != null)
        {
            rb.simulated = false;
        }
    }

    // Debug visualization
    void OnDrawGizmos()
    {
        // Draw collision boundary
        Gizmos.color = Color.yellow;
        if (circleCollider != null)
        {
            Gizmos.DrawWireSphere(transform.position, circleCollider.radius * transform.localScale.x);
        }
        else if (boxCollider != null)
        {
            Gizmos.DrawWireCube(transform.position, boxCollider.size * transform.localScale.x);
        }
    }
}

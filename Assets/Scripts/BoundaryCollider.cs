using UnityEngine;

public class BoundaryCollider : MonoBehaviour
{
    void Start()
    {
        // Make sure there's a BoxCollider2D
        BoxCollider2D boxCollider = GetComponent<BoxCollider2D>();
        if (boxCollider == null)
        {
            boxCollider = gameObject.AddComponent<BoxCollider2D>();
            Debug.Log($"Added BoxCollider2D to {gameObject.name}");
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // Log when something hits the boundary
        Debug.Log($"Collision detected with boundary {gameObject.name}. Colliding object: {collision.gameObject.name}");
    }
}

using UnityEngine;

public class BackgroundScroller : MonoBehaviour
{
    public float scrollSpeed = 2f;
    public float resetPosition = -20f;  // When to reset the position
    public float startPosition = 20f;   // Where to reset to

    void Update()
    {
        // Move the background
        transform.Translate(Vector3.left * scrollSpeed * Time.deltaTime);

        // If the background has moved too far left
        if (transform.position.x <= resetPosition)
        {
            // Reset its position
            Vector3 newPos = transform.position;
            newPos.x = startPosition;
            transform.position = newPos;
        }
    }
}

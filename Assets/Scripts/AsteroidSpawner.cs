using UnityEngine;

public class AsteroidSpawner : MonoBehaviour
{
    public GameObject[] asteroidPrefabs;
    
    [Header("Spawn Position")]
    public float minY = -4f;                 // Minimum Y position for spawning
    public float maxY = 4f;                  // Maximum Y position for spawning

    public void SpawnAsteroid(int asteroidType)
    {
        if(asteroidType >= 0 && asteroidType < asteroidPrefabs.Length)
        {
            // Set random Y position
            Vector3 spawnPos = transform.position;
            spawnPos.y = Random.Range(minY, maxY);
            transform.position = spawnPos;

            GameObject asteroid = Instantiate(asteroidPrefabs[asteroidType], transform.position, transform.rotation);
            
            // Randomly decide if the asteroid should use scale animation
            AsteroidMovement movement = asteroid.GetComponent<AsteroidMovement>();
            if (movement != null)
            {
                movement.useScaleAnimation = Random.value > 0.5f;
            }
        }
        else
        {
            Debug.LogWarning($"Invalid asteroid type index: {asteroidType}");
        }
    }
}

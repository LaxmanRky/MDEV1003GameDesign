using UnityEngine;

public class AsteroidSpawner : MonoBehaviour
{
    public GameObject[] asteroidPrefabs;
    
    [Header("Spawn Settings")]
    public float initialSpawnRate = 2.0f;    // Time between spawns at start
    public float minimumSpawnRate = 0.5f;    // Fastest spawn rate possible
    public float difficultyRamp = 0.1f;      // How much to decrease spawn time every difficultyInterval
    public float difficultyInterval = 10f;    // How often to increase difficulty (in seconds)
    
    [Header("Spawn Position")]
    public float minY = -4f;                 // Minimum Y position for spawning
    public float maxY = 4f;                  // Maximum Y position for spawning

    private float currentSpawnRate;
    private float nextSpawnTime;
    private float nextDifficultyIncrease;

    void Start()
    {
        currentSpawnRate = initialSpawnRate;
        nextSpawnTime = Time.time + currentSpawnRate;
        nextDifficultyIncrease = Time.time + difficultyInterval;
    }

    void Update()
    {
        if (GameManager.IsGameOver) return;

        // Check if it's time to spawn
        if (Time.time >= nextSpawnTime)
        {
            SpawnRandomAsteroid();
            nextSpawnTime = Time.time + currentSpawnRate;
        }

        // Check if it's time to increase difficulty
        if (Time.time >= nextDifficultyIncrease)
        {
            IncreaseDifficulty();
            nextDifficultyIncrease = Time.time + difficultyInterval;
        }
    }

    void SpawnRandomAsteroid()
    {
        // Random Y position
        Vector3 spawnPos = transform.position;
        spawnPos.y = Random.Range(minY, maxY);
        transform.position = spawnPos;

        // Random asteroid type
        int asteroidType = Random.Range(0, asteroidPrefabs.Length);
        SpawnAsteroid(asteroidType);
    }

    public void SpawnAsteroid(int asteroidType)
    {
        if(asteroidType >= 0 && asteroidType < asteroidPrefabs.Length)
        {
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

    void IncreaseDifficulty()
    {
        // Decrease spawn rate (make asteroids spawn faster)
        currentSpawnRate = Mathf.Max(minimumSpawnRate, currentSpawnRate - difficultyRamp);
        Debug.Log($"Difficulty increased! New spawn rate: {currentSpawnRate:F2} seconds");
    }

    // Call this when restarting the game
    public void ResetDifficulty()
    {
        currentSpawnRate = initialSpawnRate;
        nextSpawnTime = Time.time + currentSpawnRate;
        nextDifficultyIncrease = Time.time + difficultyInterval;
    }
}

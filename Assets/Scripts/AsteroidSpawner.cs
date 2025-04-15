using UnityEngine;

public class AsteroidSpawner : MonoBehaviour
{
    public GameObject[] asteroidPrefabs;

    public void SpawnAsteroid(int asteroidType)
    {
        if(asteroidType >=0 && asteroidType < asteroidPrefabs.Length)
        {
            Instantiate(asteroidPrefabs[asteroidType], transform.position,transform.rotation);
        }
        else
        {
            Debug.Log("Invalid enemy type index");

        }
}
}

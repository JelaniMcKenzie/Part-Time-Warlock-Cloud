using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [Header("Door Settings")]
    public GameObject doorPrefab;
    public Transform doorSpawnPoint1;
    public Transform doorSpawnPoint2;

    [Header("Enemy Settings")]
    public GameObject[] enemyPrefabs; // Array of enemy prefabs to spawn
    public Transform[] enemySpawnPoints; // Array of spawn points for enemies

    private List<GameObject> spawnedEnemies = new List<GameObject>();
    private GameObject door1;
    private GameObject door2;

    void Start()
    {
        SpawnDoors();
        SpawnEnemies();
    }

    void Update()
    {
        CheckEnemies();
    }

    private void SpawnDoors()
    {
        if (doorPrefab != null && doorSpawnPoint1 != null && doorSpawnPoint2 != null)
        {
            door1 = Instantiate(doorPrefab, doorSpawnPoint1.position, Quaternion.identity);
            door2 = Instantiate(doorPrefab, doorSpawnPoint2.position, Quaternion.identity);
        }
        else
        {
            Debug.LogWarning("Door prefab or spawn points are not assigned.");
        }
    }

    private void SpawnEnemies()
    {
        if (enemyPrefabs.Length > 0 && enemySpawnPoints.Length > 0)
        {
            foreach (Transform spawnPoint in enemySpawnPoints)
            {
                // Pick a random enemy from the array
                GameObject randomEnemy = enemyPrefabs[Random.Range(0, enemyPrefabs.Length)];

                // Spawn the enemy at the current spawn point
                GameObject spawnedEnemy = Instantiate(randomEnemy, spawnPoint.position, Quaternion.identity);

                // Add the spawned enemy to the list
                spawnedEnemies.Add(spawnedEnemy);
            }
        }
        else
        {
            Debug.LogWarning("No enemies or spawn points assigned.");
        }
    }

    private void CheckEnemies()
    {
        // Remove null references (in case an enemy is destroyed)
        spawnedEnemies.RemoveAll(enemy => enemy == null);

        // If all enemies are defeated, despawn the doors
        if (spawnedEnemies.Count == 0 && door1 != null && door2 != null)
        {
            Destroy(door1);
            Destroy(door2);

            Debug.Log("All enemies defeated! Doors despawned.");
        }
    }
}

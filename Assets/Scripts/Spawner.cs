using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    // List of enemy prefabs to spawn
    public List<GameObject> EnemyPrefabs;

    // The spawn point
    public Transform[] spawnPoints; // Assign points in your scene

    // Initial time between spawns (in seconds)
    public float initialSpawnInterval = 2.0f;

    // Minimum time between spawns (in seconds)
    public float minimumSpawnInterval = 0.5f;

    // Rate at which the spawn interval decreases (per second)
    public float spawnIntervalDecreaseRate = 0.1f;

    // Internal timer to track spawn cooldown
    private float spawnTimer = 0.0f;

    // Current spawn interval
    public float currentSpawnInterval;

    // Distance range for spawning
    public float MinDistance = 100f;
    public float MaxDistance = 500f;

    //0 == rock, 1== item
    public int spawnerType;

    void Start()
    {
        currentSpawnInterval = initialSpawnInterval;
        SpawnRandomEnemy();
    }

    void Update()
    {
        spawnTimer += Time.deltaTime;

        // Check if the spawn timer has reached or exceeded the current spawn interval
        if (spawnTimer >= currentSpawnInterval)
        {
            // Reset the spawn timer
            spawnTimer = 0.0f;

            // Spawn the enemy
            SpawnRandomEnemy();

            // Reduce the spawn interval, but don't go below the minimum spawn interval
            currentSpawnInterval = Mathf.Max(currentSpawnInterval - spawnIntervalDecreaseRate * Time.deltaTime, minimumSpawnInterval);
        }
    }

    private void SpawnRandomEnemy()
    {
        if (EnemyPrefabs == null || EnemyPrefabs.Count == 0)
        {
            Debug.LogError("EnemyPrefabs list is empty.");
            return;
        }
        foreach(Transform spawnp in spawnPoints)
        {
            // Choose a random enemy prefab from the list
            GameObject enemyPrefab = EnemyPrefabs[Random.Range(0, EnemyPrefabs.Count)];

            // Generate a random forward direction
            Vector3 randomForwardDirection = spawnp.forward + new Vector3(
                Random.Range(-1.0f, 1.0f),
                Random.Range(-1.0f, 1.0f),
                Random.Range(-1.0f, 1.0f)
            ).normalized;

            // Generate a random distance within the specified range
            float randomDistance = Random.Range(MinDistance, MaxDistance);

            // Calculate the spawn position
            Vector3 spawnPosition = spawnp.position + randomForwardDirection * randomDistance;

            // Instantiate the enemy at the calculated position
            Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);

        }
       
    }

    void OnDrawGizmosSelected()
    {
        if (spawnPoints == null) return;

        Gizmos.color = Color.green; // Set gizmo color

        foreach (Transform spawnp in spawnPoints)
        {
            if (spawnp != null)
            {
                Gizmos.DrawWireSphere(spawnp.position, MinDistance); // Draw the minimum distance sphere
                Gizmos.DrawWireSphere(spawnp.position, MaxDistance); // Draw the maximum distance sphere
            }
        }
    }
}

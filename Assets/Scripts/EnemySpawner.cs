using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField]
    [Tooltip("The enemy prefab to spawn")]
    private GameObject enemyPrefab;

    [SerializeField]
    [Tooltip("The minimum time for the enemy to appear")]
    private float minimumSpawnTime = 0f;

    [SerializeField]
    [Tooltip("The maximum time for the enemy to appear")]
    private float maximumSpawnTime = 10f;

    private float timeUntilSpawn; // Countdown timer for the next enemy spawn

    void Start()
    {
        // Initialize the time until the first spawn
        SetTimeUntilSpawn();
    }

    void Update()
    {
        // Decrease the spawn timer
        timeUntilSpawn -= Time.deltaTime;

        // If the timer reaches zero, spawn an enemy and reset the timer
        if (timeUntilSpawn <= 0)
        {
            SpawnEnemy();
            SetTimeUntilSpawn(); // Reset the spawn timer
        }
    }

    // Spawns an enemy at the spawner's position
    private void SpawnEnemy()
    {
        Instantiate(enemyPrefab, transform.position, Quaternion.identity);
    }

    // Randomly sets the time until the next spawn between the minimum and maximum values
    private void SetTimeUntilSpawn()
    {
        timeUntilSpawn = Random.Range(minimumSpawnTime, maximumSpawnTime);
    }
}

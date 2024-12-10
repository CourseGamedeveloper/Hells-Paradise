using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject enemyPrefab; // The enemy prefab to spawn
    [SerializeField][Tooltip("The minimum time for the enemy to appear")] private float minimumSpawnTime = 0f; // Minimum spawn interval
    [SerializeField][Tooltip("The maximum time for the enemy to appear")] private float maximumSpawnTime = 10f; // Maximum spawn interval

    private float timeUntilSpwan; // Countdown timer for the next enemy spawn

    void Start()
    {
        // Initialize the time until the first spawn
        SetTimeSpwan();
    }

    void Update()
    {
        // Decrease the spawn timer
        timeUntilSpwan -= Time.deltaTime;

        // If the timer reaches zero, spawn an enemy and reset the timer
        if (timeUntilSpwan <= 0)
        {
            Instantiate(enemyPrefab, transform.position, Quaternion.identity); // Spawn the enemy at the spawner's position
            SetTimeSpwan(); // Reset the spawn timer
        }
    }

    // Randomly sets the time until the next spawn between the minimum and maximum values
    private void SetTimeSpwan()
    {
        timeUntilSpwan = Random.Range(minimumSpawnTime, maximumSpawnTime);
    }
}
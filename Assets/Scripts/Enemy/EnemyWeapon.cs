using UnityEngine;

/// <summary>
/// Handles enemy weapon interactions and applies damage to the player upon collision.
/// </summary>
public class EnemyWeapon : MonoBehaviour
{
    [SerializeField]
    [Tooltip("Damage dealt to the player by this enemy.")]
    private float damage;

    private PlayerController playerController;

    private void Awake()
    {
        playerController = FindAnyObjectByType<PlayerController>();
    }

    /// <summary>
    /// Detects collision with the player and applies damage.
    /// </summary>
    /// <param name="other">The collider of the object that enters the trigger zone.</param>
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Enemy hit the player");
            playerController.Take_Damage(damage);
        }
    }
}

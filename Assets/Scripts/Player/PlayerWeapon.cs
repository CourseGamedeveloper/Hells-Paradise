using UnityEngine;

/// <summary>
/// Handles the player's weapon mechanics, including damage, fire effect, and enemy interaction.
/// </summary>
public class PlayerWeapon : MonoBehaviour
{
    [SerializeField]
    [Tooltip("The player's sword damage.")]
    private float Damage;

    public GameObject fireEffect;
    private PlayerController playerController;

    private void Awake()
    {
        playerController = FindAnyObjectByType<PlayerController>();
        fireEffect.SetActive(false);
    }

    /// <summary>
    /// Detects collision with an enemy and applies damage.
    /// </summary>
    /// <param name="other">The collider of the object that enters the trigger zone.</param>
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy")) // Detect the specific enemy hit
        {
            EnemyController enemy = other.GetComponent<EnemyController>();
            if (enemy != null)
            {
                enemy.TakeDamage(Damage); // Apply damage to the correct enemy
            }
        }
    }

    /// <summary>
    /// Updates the player's weapon damage.
    /// </summary>
    /// <param name="_AddDamage">Amount of damage to add.</param>
    private void UpdatePlayerDamage(float _AddDamage)
    {
        Damage += _AddDamage;
    }

    /// <summary>
    /// Activates or deactivates the fire effect and modifies weapon stats accordingly.
    /// </summary>
    /// <param name="useEffect">True to activate the fire effect, false to deactivate it.</param>
    public void SetFireEffect(bool useEffect)
    {
        fireEffect.SetActive(useEffect);

        if (useEffect)
        {
            playerController.Heal(10f);
            UpdatePlayerDamage(20f);
        }
        else
        {
            UpdatePlayerDamage(-20f);
        }
    }
}

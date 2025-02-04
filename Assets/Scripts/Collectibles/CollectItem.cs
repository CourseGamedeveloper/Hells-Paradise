using UnityEngine;

/// <summary>
/// Handles collectible items such as health, mana, elixirs, and keys.
/// </summary>
public class CollectItem : MonoBehaviour
{
    [Header("Collectible Settings")]
    [Tooltip("Rotation speed of the collectible.")]
    [SerializeField] private float rotationSpeed;

    [Tooltip("Type of the collectible.")]
    [SerializeField] private CollectibleType itemType;

    [Tooltip("Amount to add for health or mana.")]
    [SerializeField] private float valueAmount;

    private GameManager gameManager;
    private PlayerController player;
    private UIManager uiManager;
    private Prison prison;

    private void Awake()
    {
        prison = FindAnyObjectByType<Prison>();
        uiManager = FindAnyObjectByType<UIManager>();
        player = FindAnyObjectByType<PlayerController>();
        gameManager = FindAnyObjectByType<GameManager>();
    }

    private void Update()
    {
        // Rotate the collectible continuously
        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        // Check if the player collides with the collectible
        if (other.CompareTag("Player"))
        {
            if (player != null)
            {
                ApplyEffect(player);
            }

            // Destroy the item after collection
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// Applies the effect of the collected item to the player.
    /// </summary>
    /// <param name="player">The player who collected the item.</param>
    private void ApplyEffect(PlayerController player)
    {
        switch (itemType)
        {
            case CollectibleType.AddHealth:
                player.Heal(valueAmount);
                break;
            case CollectibleType.AddMana:
                player.AddMana(valueAmount);
                break;
            case CollectibleType.ElixirLife:
                gameManager.WinGame();
                break;
            case CollectibleType.Key:
                uiManager.SetKeyText();
                prison.setIsHaveKey();
                break;
        }
    }

    /// <summary>
    /// Defines the different types of collectible items.
    /// </summary>
    public enum CollectibleType
    {
        AddHealth,
        AddMana,
        ElixirLife,
        Key
    }
}

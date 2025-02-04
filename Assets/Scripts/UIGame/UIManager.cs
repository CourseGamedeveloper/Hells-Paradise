using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [Header("Health Settings")]
    public Slider HealthSlider; // Reference to the UI Slider for health
    [SerializeField]
    [Tooltip("Max Health for the Player")]
    private float HealthMax = 100f; // Maximum health for the player

    private float currentHealth; // Player's current health

    private void Start()
    {
        // Initialize current health and set slider values
        currentHealth = HealthMax;
        InitializeHealthBar();
    }
    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.V)) 
        {
            TakeDamage(10);
        }
    }

    private void InitializeHealthBar()
    {
        if (HealthSlider != null)
        {
            HealthSlider.maxValue = HealthMax;
            HealthSlider.value = currentHealth;
        }
        else
        {
            Debug.LogError("HealthSlider is not assigned in the Inspector!");
        }
    }

    private void UpdateHealthBar()
    {
        if (HealthSlider != null)
        {
            HealthSlider.value = currentHealth;
        }
    }

    /// <summary>
    /// Reduces the player's health and updates the slider.
    /// </summary>
    /// <param name="damage">The amount of damage taken.</param>
    public void TakeDamage(float damage)
    {
        currentHealth = Mathf.Clamp(currentHealth - damage, 0, HealthMax);
        UpdateHealthBar();
        if (currentHealth <= 0)
        {
            OnPlayerDeath();
        }
    }

    /// <summary>
    /// Heals the player and updates the slider.
    /// </summary>
    /// <param name="healAmount">The amount of health restored.</param>
    public void Heal(float healAmount)
    {
        currentHealth = Mathf.Clamp(currentHealth + healAmount, 0, HealthMax);
        UpdateHealthBar();
    }

    /// <summary>
    /// Triggered when the player's health reaches zero.
    /// </summary>
    private void OnPlayerDeath()
    {
        Debug.Log("Player has died.");
        // Add logic for game over, respawn, etc.
    }
}

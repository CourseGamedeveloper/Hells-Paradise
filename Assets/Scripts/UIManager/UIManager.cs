using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

/// <summary>
/// Manages the UI elements, including health/mana bars, key notifications, and enemy tracking.
/// </summary>
public class UIManager : MonoBehaviour
{
    [Header("Health Settings")]
    public Slider HealthSlider; // Reference to the UI Slider for health
    public Slider ManaSlider;

    private PlayerController playerController;
    public float Player_currentHealth; // Player's current health
    public float Player_currentMana;

    [SerializeField]
    [Tooltip("Canvas Game Objects for the Keys")]
    public GameObject Keys;
    public bool isUsedMana;
    public GameObject TextMana;
    public GameObject TextUseMana;
    public GameObject TextKey;
    private PlayerWeapon weapon;

    [Header("Enemy Tracking")]
    public TextMeshProUGUI EnemyCounterText;
    private Dictionary<int, int> villageEnemies = new Dictionary<int, int>(); // Number of enemies in each village
    private int currentVillage = 1; // The current village 

    private void Awake()
    {
        TextUseMana.SetActive(false);
        TextMana.SetActive(false);
        isUsedMana = false;
        playerController = FindAnyObjectByType<PlayerController>();
        weapon = FindAnyObjectByType<PlayerWeapon>();

        // Initialize health and mana bars
        Player_currentHealth = playerController.HealthMax;
        InitializeHealthBar();
        InitializeManaBar();

        if (Keys != null)
        {
            Keys.SetActive(true);
        }

        // Define enemy count per village
        villageEnemies[1] = 7;
        villageEnemies[2] = 6;
        villageEnemies[3] = 8;
        villageEnemies[4] = 3;
    }

    private void Update()
    {
        UpdateManaBar();
        UpdateIsUsedMana();
        CheckMana();
    }

    /// <summary>
    /// Initializes the player's health bar in the UI.
    /// </summary>
    private void InitializeHealthBar()
    {
        if (HealthSlider != null)
        {
            HealthSlider.maxValue = playerController.HealthMax;
            HealthSlider.value = Player_currentHealth;
        }
        else
        {
            Debug.LogError("HealthSlider is not assigned in the Inspector!");
        }
    }

    /// <summary>
    /// Initializes the player's mana bar in the UI.
    /// </summary>
    private void InitializeManaBar()
    {
        if (ManaSlider != null)
        {
            ManaSlider.maxValue = playerController.ManaMax;
            ManaSlider.value = Player_currentMana;
        }
    }

    /// <summary>
    /// Updates the health bar UI based on the player's current health.
    /// </summary>
    public void UpdateHealthBar(float currentHealth, float maxHealth)
    {
        if (HealthSlider != null)
        {
            HealthSlider.maxValue = maxHealth;
            HealthSlider.value = currentHealth;
        }
    }

    /// <summary>
    /// Updates the mana bar UI.
    /// </summary>
    public void UpdateManaBar()
    {
        if (ManaSlider != null)
        {
            if (Player_currentMana < playerController.ManaMax && !isUsedMana)
            {
                Player_currentMana += (10f * Time.deltaTime);
                ManaSlider.value = Player_currentMana;
            }
            else if (isUsedMana && Player_currentMana >= 0)
            {
                Player_currentMana -= 10f * Time.deltaTime;
                ManaSlider.value = Player_currentMana;
            }
        }
    }

    /// <summary>
    /// Updates the player's mana status and activates abilities if necessary.
    /// </summary>
    private void UpdateIsUsedMana()
    {
        if (Input.GetKeyUp(KeyCode.A) && Player_currentMana > playerController.ManaMax)
        {
            isUsedMana = true;
            weapon.SetFireEffect(true);
            TextMana.SetActive(true);
        }
        else if (isUsedMana && Player_currentMana <= 0f)
        {
            isUsedMana = false;
            weapon.SetFireEffect(false);
            TextMana.SetActive(false);
        }
    }

    /// <summary>
    /// Checks if the player has enough mana to use abilities.
    /// </summary>
    private void CheckMana()
    {
        if (Player_currentMana >= 100)
        {
            TextUseMana.SetActive(true);
        }
        else
        {
            TextUseMana.SetActive(false);
        }
    }

    /// <summary>
    /// Displays the key text UI for a limited time.
    /// </summary>
    public void SetKeyText()
    {
        TextKey.SetActive(true);
        StartCoroutine(HideTextAfterDelay(10));
    }

    /// <summary>
    /// Hides the key text UI after a delay.
    /// </summary>
    private IEnumerator HideTextAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        TextKey.SetActive(false);
    }

    /// <summary>
    /// Hides the enemy counter UI after a delay.
    /// </summary>
    private IEnumerator HideEnemyCounterText()
    {
        yield return new WaitForSeconds(5);
        EnemyCounterText.gameObject.SetActive(false);
    }

    /// <summary>
    /// Updates the enemy counter UI.
    /// </summary>
    public void UpdateEnemyCounter()
    {
        if (EnemyCounterText != null && villageEnemies[currentVillage] > 0)
        {
            EnemyCounterText.text = $"Village {currentVillage} - Enemies Left: {villageEnemies[currentVillage]}";
        }
    }

    /// <summary>
    /// Called when an enemy is defeated to update the UI and enemy count.
    /// </summary>
    public void EnemyDefeated(int villageId)
    {
        if (!villageEnemies.ContainsKey(villageId)) return;

        villageEnemies[villageId]--;
        if (villageEnemies[villageId] <= 0)
        {
            EnemyCounterText.text = $"All enemies in Village {villageId} defeated!";
            StartCoroutine(HideEnemyCounterText());
        }
        else
        {
            UpdateEnemyCounter();
        }
    }

    /// <summary>
    /// Sets the current village ID when the player enters a village.
    /// </summary>
    public void SetCurrentVillage(int villageId)
    {
        if (!villageEnemies.ContainsKey(villageId)) return;

        currentVillage = villageId;
        ShowEnemyCounterText();
    }

    /// <summary>
    /// Displays the enemy counter UI if there are remaining enemies in the village.
    /// </summary>
    public void ShowEnemyCounterText()
    {
        if (villageEnemies[currentVillage] > 0)
        {
            EnemyCounterText.gameObject.SetActive(true);
            UpdateEnemyCounter();
        }
    }
}

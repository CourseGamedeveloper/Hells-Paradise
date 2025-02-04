using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

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
    private Dictionary<int, int> villageEnemies = new Dictionary<int, int>(); // Number of enemy for every village
    private int currentVillage = 1; // the current village 

    private void Awake()
    {
        TextUseMana.SetActive(false);
        TextMana.SetActive(false);
        isUsedMana = false;
        playerController = FindAnyObjectByType<PlayerController>();
        weapon = FindAnyObjectByType<PlayerWeapon>();   
        // Initialize current health and set slider values
        Player_currentHealth = playerController.HealthMax;
        InitializeHealthBar();
        InitalizeManaBar();
        
        if (Keys != null)
        {
            Keys.SetActive(true);
          
        }
        villageEnemies[1] = 7; //number of the enemy in village 1
        villageEnemies[2] = 6; //number of the enemy in village 2
        villageEnemies[3] = 8;//number of the enemy in village 3
        villageEnemies[4] = 3;//number of the enemy in the village 4 -> castle

       
    }
    private void Update()
    {
        UpdateManaBar();
        UpdateIsUsedMana();
        CheckMana();

    }
    
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
    private void InitalizeManaBar()
    {
        if (ManaSlider != null) 
        {
            ManaSlider.maxValue = playerController.ManaMax;
            ManaSlider.value = Player_currentMana;
        }
        
                

    }

    public void UpdateHealthBar(float currentHealth, float maxHealth)
    {
        if (HealthSlider != null)
        {
            HealthSlider.maxValue = maxHealth;
            HealthSlider.value = currentHealth;
        }
    }
    public void UpdateManaBar()
    {
        if (ManaSlider != null)
        {
            if (Player_currentMana < playerController.ManaMax && !isUsedMana )
            {
                Player_currentMana += (10f * Time.deltaTime);
                //Debug.Log(Player_currentMana);
                
                ManaSlider.value = Player_currentMana;
                
            }
            else if(isUsedMana&& Player_currentMana>=0) 
            {
                Player_currentMana -= 10f*Time.deltaTime;
                //Debug.Log(Player_currentMana);
                ManaSlider.value = Player_currentMana;
            }
        }
    }




    private void UpdateIsUsedMana()
    {
        if (Input.GetKeyUp(KeyCode.A) && Player_currentMana >playerController.ManaMax)
        {
            isUsedMana = true;
            weapon.SetFireEffect(true);
           TextMana.SetActive(true);

        }
        else if(isUsedMana&& Player_currentMana <= 0f)
        {
            isUsedMana = false;
            weapon.SetFireEffect(false);  
            TextMana.SetActive(false);
        }


    }
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
    public void SetKeyText()
    {
        TextKey.SetActive(true);
        StartCoroutine(HideTextAfterDelay(10));
    }
    private IEnumerator HideTextAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        TextKey.SetActive(false); // כיבוי הטקסט
    }
    private IEnumerator HideEnemyCounterText()
    {
        yield return new WaitForSeconds(5); // זמן המתנה
        EnemyCounterText.gameObject.SetActive(false); // כיבוי הטקסט
    }
    // 📌 עדכון מספר האויבים במסך
    public void UpdateEnemyCounter()
    {
        if (EnemyCounterText != null && villageEnemies[currentVillage]>0)
        {
            EnemyCounterText.text = $"Village {currentVillage} - Enemies Left: {villageEnemies[currentVillage]}";
        }
    }

    // 📌 קריאה לפונקציה כאשר אויב נהרג
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

    // 📌 פונקציה לעדכון הכפר הנוכחי של השחקן
    public void SetCurrentVillage(int villageId)
    {
        if (!villageEnemies.ContainsKey(villageId)) return;

        currentVillage = villageId;
        ShowEnemyCounterText();
    }
    public void ShowEnemyCounterText()
    {
        if (villageEnemies[currentVillage] > 0)
        {
            EnemyCounterText.gameObject.SetActive(true);
            UpdateEnemyCounter(); // עדכון הטקסט עם מספר האויבים הנוכחי
        }
    }
}

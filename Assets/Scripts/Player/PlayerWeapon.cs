using UnityEngine;

public class PlayerWeapon : MonoBehaviour
{
   
    [SerializeField]
    [Tooltip("the player swoard Damage")]
    private float Damage;
    public GameObject fireEffect;
    private PlayerController playerController;

    private void Awake()
    {
        playerController=FindAnyObjectByType<PlayerController>();
        fireEffect.SetActive(false);
       
        
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy")) //  Detect the specific enemy you hit
        {
            EnemyController enemy = other.GetComponent<EnemyController>(); //  Get THAT enemy
            if (enemy != null)
            {
                enemy.TakeDamage(Damage); //  Apply damage to the correct enemy
            }
        }
    }
    private void UpdatePlayerDamage(float _AddDamage)
    {
        Damage += _AddDamage;
    }
    public void SetFireEffect(bool useEffect)
    {
        if (useEffect)
        {
           
            fireEffect.SetActive(useEffect);
            playerController.Heal(10f);
            UpdatePlayerDamage(20f);
        }
        else 
        {
            
            fireEffect.SetActive(useEffect);
            UpdatePlayerDamage(-20f);
        }
    }

}

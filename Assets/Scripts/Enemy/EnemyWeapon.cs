using UnityEngine;

public class EnemyWeapon : MonoBehaviour
{
    [SerializeField]
    [Tooltip("Damage dealt to the player by this enemy.")]
    private float damage;
    private PlayerController playerController;
    private void Awake()
    {
        playerController=FindAnyObjectByType<PlayerController>();
    }
   
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            Debug.Log("Enemy hit The player");
            playerController.Take_Damage(damage);

        }
    }
    
}

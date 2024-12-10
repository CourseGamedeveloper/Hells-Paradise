using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    private Animator animator;
    public Transform attack_point;
    public float attack_range = 0.5f;
    public LayerMask enemyLayers;
    private int attackDamage = 30;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animator = GetComponent<Animator>();
        if (animator == null)
        {
            Debug.LogError("Animator component not found on the player object.");

        }

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // Fire bullet on mouse click
        {
            Attack();
        }

    }
    private void Attack()
    {
        if (animator != null)
        {
            //play an attack animation
            animator.SetTrigger("attack");

            //Detect enemies in range of attack
           Collider2D[] HitEnemy=Physics2D.OverlapCircleAll(attack_point.position,attack_range,enemyLayers);

            //Damage enemy
            foreach (Collider2D enemyCollider in HitEnemy)
            {
                Enemy enemy = enemyCollider.GetComponent<Enemy>();

                if (enemy != null) // Ensure the enemy component exists
                {
                    enemy.Take_Damage(attackDamage);
                }
            }
        }

    }
    private void OnDrawGizmosSelected()
    {
        if (attack_point==null)
        {
            return;
        }
        Gizmos.DrawWireSphere(attack_point.position,attack_range);
    }
}

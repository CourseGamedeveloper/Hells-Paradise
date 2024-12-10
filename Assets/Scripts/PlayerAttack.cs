using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    private Animator animator;
    public Transform attackPoint;
    public float attackRange = 0.5f;
    public LayerMask enemyLayers;
    private int attackDamage = 30;

    // Start is called before the first execution of Update after the MonoBehaviour is created
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
            // Play an attack animation
            animator.SetTrigger("attack");

            // Detect enemies in range of attack
            Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);

            // Damage enemies
            foreach (Collider2D enemyCollider in hitEnemies)
            {
                Enemy enemy = enemyCollider.GetComponent<Enemy>();
                if (enemy != null) // Ensure the enemy component exists
                {
                    enemy.TakeDamage(attackDamage); // Updated method name to follow C# naming conventions
                }
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
        {
            return;
        }
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}

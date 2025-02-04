using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

/// <summary>
/// Controls enemy behavior, including patrolling, chasing, and attacking the player.
/// </summary>
public class EnemyController : MonoBehaviour
{
    [Header("Enemy Settings")]
    [SerializeField] private float detectionRange;
    [SerializeField] private float attackRange;
    [SerializeField] private float patrolSpeed;
    [SerializeField] private float chaseSpeed;
    [SerializeField] private Transform[] patrolPoints;
    public Slider HealthSlider; // Reference to the UI Slider for health

    private Animator animator;
    private NavMeshAgent agent;
    private Transform player;
    private bool isAttacking = false;
    private bool isDead = false;

    [SerializeField] public float EnemyHealthMax;

    [Header("Village Settings")]
    [SerializeField] private Vector3 villageCenter;
    [SerializeField] private float villageRadius = 30f;
    public int villageId;

    private enum EnemyState { Idle, Patrol, Chase, Attack }
    private EnemyState currentState = EnemyState.Patrol;
    private float enemy_currentHealth;
    private UIManager uiManager;

    private void Awake()
    {
        uiManager = FindAnyObjectByType<UIManager>();
        enemy_currentHealth = EnemyHealthMax;
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();

        if (animator == null)
            Debug.LogError("Animator component is missing!");

        if (agent == null)
            Debug.LogError("NavMeshAgent component is missing!");
    }

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        if (player == null)
            Debug.LogError("Player not found!");

        agent.speed = patrolSpeed;
        SetRandomPatrolDestination();
    }

    private void Update()
    {
        if (isDead || player == null) return;

        switch (currentState)
        {
            case EnemyState.Patrol:
                HandlePatrol();
                break;
            case EnemyState.Chase:
                HandleChase();
                break;
            case EnemyState.Attack:
                HandleAttack();
                break;
        }

        animator.SetBool("isAttack", isAttacking);
    }

    /// <summary>
    /// Handles enemy patrol behavior.
    /// </summary>
    private void HandlePatrol()
    {
        if (isDead || player == null) return;

        animator.SetBool("walk", true);
        animator.SetBool("run", false);

        if (!agent.pathPending && agent.remainingDistance < 0.5f)
        {
            SetRandomPatrolDestination();
        }

        if (Vector3.Distance(transform.position, player.position) < detectionRange)
        {
            currentState = EnemyState.Chase;
            agent.speed = chaseSpeed;
        }
    }

    /// <summary>
    /// Handles enemy chase behavior.
    /// </summary>
    private void HandleChase()
    {
        animator.SetBool("walk", false);
        animator.SetBool("run", true);

        if (Vector3.Distance(player.position, villageCenter) <= villageRadius)
        {
            agent.SetDestination(player.position);
        }
        else
        {
            currentState = EnemyState.Patrol;
            agent.speed = patrolSpeed;
            SetRandomPatrolDestination();
        }

        if (Vector3.Distance(transform.position, player.position) < attackRange)
        {
            currentState = EnemyState.Attack;
        }
    }

    /// <summary>
    /// Handles enemy attack behavior.
    /// </summary>
    private void HandleAttack()
    {
        agent.ResetPath();
        animator.SetBool("run", false);
        animator.SetBool("walk", false);

        if (!isAttacking)
        {
            isAttacking = true;
            int attackType = Random.Range(1, 3);
            animator.SetTrigger(attackType == 1 ? "attack1" : "attack2");
            Invoke(nameof(ResetAttack), 1.5f);
        }

        if (Vector3.Distance(transform.position, player.position) > attackRange)
        {
            currentState = EnemyState.Chase;
            isAttacking = false;
        }
    }

    /// <summary>
    /// Selects a random patrol destination within the village radius.
    /// </summary>
    private void SetRandomPatrolDestination()
    {
        Vector3 randomPoint = villageCenter + new Vector3(
            Random.Range(-villageRadius, villageRadius),
            0,
            Random.Range(-villageRadius, villageRadius)
        );

        if (NavMesh.SamplePosition(randomPoint, out NavMeshHit hit, villageRadius, NavMesh.AllAreas))
        {
            agent.SetDestination(hit.position);
        }
    }

    private void ResetAttack()
    {
        isAttacking = false;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(villageCenter, villageRadius);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }

    /// <summary>
    /// Applies damage to the enemy.
    /// </summary>
    public void TakeDamage(float damage)
    {
        enemy_currentHealth -= damage;
        enemy_currentHealth = Mathf.Clamp(enemy_currentHealth, 0, EnemyHealthMax);

        UpdateEnemyHealthBar(enemy_currentHealth, EnemyHealthMax);

        if (enemy_currentHealth <= 0)
        {
            Die();
        }
    }

    /// <summary>
    /// Handles enemy death.
    /// </summary>
    private void Die()
    {
        if (isDead) return;

        isDead = true;
        animator.SetTrigger("die");
        agent.enabled = false;

        if (uiManager != null)
        {
            uiManager.EnemyDefeated(villageId);
        }

        StartCoroutine(WaitAndDestroy());
    }

    private IEnumerator WaitAndDestroy()
    {
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        float animationLength = animator.runtimeAnimatorController.animationClips
            .FirstOrDefault(clip => clip.name == "die")?.length ?? 2f;

        yield return new WaitForSeconds(animationLength);
        Destroy(gameObject);
    }

    /// <summary>
    /// Updates the enemy health bar UI.
    /// </summary>
    private void UpdateEnemyHealthBar(float currentHealth, float maxHealth)
    {
        if (HealthSlider != null)
        {
            HealthSlider.maxValue = maxHealth;
            HealthSlider.value = currentHealth;
        }
    }
}

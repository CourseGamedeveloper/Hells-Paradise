using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

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
    [SerializeField] private Vector3 villageCenter; // מרכז הכפר
    [SerializeField] private float villageRadius = 30f; // רדיוס הכפר

    private enum EnemyState { Idle, Patrol, Chase, Attack }
    private EnemyState currentState = EnemyState.Patrol; // Start in Patrol state
    private float enemy_currentHealth;
    private UIManager uiManager;
    [Header("Village Settings")]
    public int villageId; // מזהה את הכפר שבו האויב נמצא

    private void Awake()
    {
        uiManager=FindAnyObjectByType<UIManager>();
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
        // Find the player by tag
        player = GameObject.FindGameObjectWithTag("Player").transform;
        if (player == null)
            Debug.LogError("Player not found!");
        

        // Start patrolling
        agent.speed = patrolSpeed;
        SetRandomPatrolDestination();
    }

    private void Update()
    {
        if (isDead||player==null) return; // Skip all logic if the enemy is dead

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

    private void HandlePatrol()
    {
        if (isDead||player==null) return;

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

    private void HandleChase()
    {
        animator.SetBool("walk", false);
        animator.SetBool("run", true);

        // Check if the player is inside the village radius
        if (Vector3.Distance(player.position, villageCenter) <= villageRadius)
        {
            agent.SetDestination(player.position);
        }
        else
        {
            currentState = EnemyState.Patrol; // Return to patrol if player leaves the village
            agent.speed = patrolSpeed;
            SetRandomPatrolDestination();
        }

        if (Vector3.Distance(transform.position, player.position) < attackRange)
        {
            currentState = EnemyState.Attack;
        }
    }

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

    private void SetRandomPatrolDestination()
    {
        Vector3 randomPoint = villageCenter + new Vector3(
            Random.Range(-villageRadius, villageRadius),
            0,
            Random.Range(-villageRadius, villageRadius)
        );

        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomPoint, out hit, villageRadius, NavMesh.AllAreas))
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


    private void UpdateEnemyHealthBar(float currentHealth, float maxHealth)
    {
        if (HealthSlider != null)
        {
            HealthSlider.maxValue = maxHealth;
            HealthSlider.value = currentHealth;
        }
    }
}

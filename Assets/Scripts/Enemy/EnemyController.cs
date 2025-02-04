using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    [Header("Enemy Settings")]
    [SerializeField]
    private float detectionRange = 15f;

    [SerializeField]
    private float attackRange = 2f;

    [SerializeField]
    private float patrolSpeed = 2f;

    [SerializeField]
    private float chaseSpeed = 4f;

    [SerializeField]
    private Transform[] patrolPoints;

    private Animator animator;
    private NavMeshAgent agent;
    private Transform player;
    private int currentPatrolIndex = 0;

    private bool isAttacking = false;

    private enum EnemyState { Idle, Patrol, Chase, Attack, Stunned }
    private EnemyState currentState = EnemyState.Idle;

    private void Awake()
    {
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
        currentState = EnemyState.Patrol;
        agent.speed = patrolSpeed;
        MoveToNextPatrolPoint();
    }

    private void Update()
    {
        switch (currentState)
        {
            case EnemyState.Idle:
                HandleIdle();
                break;
            case EnemyState.Patrol:
                HandlePatrol();
                break;
            case EnemyState.Chase:
                HandleChase();
                break;
            case EnemyState.Attack:
                HandleAttack();
                break;
            case EnemyState.Stunned:
                HandleStunned();
                break;
        }
        animator.SetBool("isAttack",isAttacking);
    }

    private void HandleIdle()
    {
        animator.SetBool("walk", false);
        animator.SetBool("run", false);
    }

    private void HandlePatrol()
    {
        animator.SetBool("walk", true);
        animator.SetBool("run", false);

        if (!agent.pathPending && agent.remainingDistance < 0.5f)
        {
            MoveToNextPatrolPoint();
        }

        // Check if the player is in detection range
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

        agent.SetDestination(player.position);

        // If within attack range, switch to attack
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

        // Attack animations
        if (!isAttacking)
        {
            isAttacking = true;
            int attackType = Random.Range(1, 3); // Randomly choose between attack1 and attack2
            animator.SetTrigger(attackType == 1 ? "attack1" : "attack2");
            Invoke(nameof(ResetAttack), 1.5f); // Cooldown before next attack
        }

        // If the player is out of attack range, go back to chasing
        if (Vector3.Distance(transform.position, player.position) > attackRange)
        {
            currentState = EnemyState.Chase;
            isAttacking = false;
        }
    }

    private void HandleStunned()
    {
        animator.SetBool("walk", false);
        animator.SetBool("run", false);
        animator.SetTrigger("stun");

        // Example: Transition to patrol after 3 seconds
        Invoke(nameof(ResumePatrol), 3f);
    }

    private void MoveToNextPatrolPoint()
    {
        if (patrolPoints.Length == 0)
        {
            currentState = EnemyState.Idle;
            return;
        }

        agent.destination = patrolPoints[currentPatrolIndex].position;
        currentPatrolIndex = (currentPatrolIndex + 1) % patrolPoints.Length;
    }

    private void ResetAttack()
    {
        isAttacking = false;
    }

    private void ResumePatrol()
    {
        currentState = EnemyState.Patrol;
        agent.speed = patrolSpeed;
        MoveToNextPatrolPoint();
    }

    private void OnDrawGizmosSelected()
    {
        // Draw detection range
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRange);

        // Draw attack range
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}

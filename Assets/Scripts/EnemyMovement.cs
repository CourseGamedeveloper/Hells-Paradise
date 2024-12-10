using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField]
    [Tooltip("Speed of the enemy movement")]
    private float speed;

    private Rigidbody2D _rigidbody;
    private Animator _animator;

    [Tooltip("This GameObject is the target that the enemy will attack")]
    public GameObject Player;

    private float distance;

    void Start()
    {
        _animator = GetComponent<Animator>();
        _rigidbody = GetComponent<Rigidbody2D>();
        Player = GameObject.FindWithTag("Player"); // Assign the Player GameObject dynamically
    }

    void Update()
    {
        // Calculate the distance between the enemy and the player
        distance = Vector2.Distance(transform.position, Player.transform.position);

        if (distance <= 8f && distance >= 3f)
        {
            _animator.SetBool("walking", true);

            // Calculate the direction to the player
            Vector2 direction = (Player.transform.position - transform.position).normalized;

            FlipTowardsPlayer(direction);

            // Move the enemy towards the player
            _rigidbody.velocity = direction * speed;
        }
        else if (distance < 3f && distance >= 0)
        {
            _animator.SetBool("walking", false);
            _animator.SetTrigger("attack");

            // Stop the enemy's movement
            _rigidbody.velocity = Vector2.zero;
        }
        else
        {
            _animator.SetBool("walking", false);
            _rigidbody.velocity = Vector2.zero;
        }
    }

    private void FlipTowardsPlayer(Vector2 direction)
    {
        // Check if the enemy needs to flip based on the x-direction
        if (direction.x > 0)
        {
            // Face right
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
        else if (direction.x < 0)
        {
            // Face left (flip horizontally)
            transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
    }
}

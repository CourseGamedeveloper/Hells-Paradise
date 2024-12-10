using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D body;
    [SerializeField]
    [Tooltip("Speed for the player movement")]
    private float speed = 5f;

    private Vector3 localScale;
    private Animator animator;
    private bool grounded;

    private void Start()
    {
        body = GetComponent<Rigidbody2D>();
        localScale = transform.localScale; // Initialize the local scale
        animator = GetComponent<Animator>();

        if (animator == null)
        {
            Debug.LogError("Animator component not found on the player object.");
        }
    }

    private void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal"); // Correct spelling: Horizontal
        body.velocity = new Vector2(horizontalInput * speed, body.velocity.y); // Updated to `velocity` instead of `linearVelocity`

        // Flip the player based on movement direction
        if (horizontalInput > 0.01f)
        {
            transform.localScale = new Vector3(localScale.x, localScale.y, localScale.z); // Face right
        }
        else if (horizontalInput < -0.01f)
        {
            transform.localScale = new Vector3(-localScale.x, localScale.y, localScale.z); // Face left
        }

        if (Input.GetKey(KeyCode.Space) && grounded)
        {
            Jump();
        }

        // Set Animator parameters
        animator.SetBool("run", Mathf.Abs(horizontalInput) > 0);
        animator.SetBool("grounded", grounded);
    }

    // Makes the player jump
    private void Jump()
    {
        body.AddForce(Vector2.up * 300);
        animator.SetTrigger("jump");
        grounded = false;
    }

    // Detect when the player lands on the ground
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            grounded = true;
        }
    }
}

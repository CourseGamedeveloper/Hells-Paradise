using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{

    [SerializeField]
    [Tooltip("Speed of the player movement.")]
    private float speed = 5f;

    [SerializeField]
    [Tooltip("Jump force for the player.")]
    private float jumpForce = 10f;

    [SerializeField]
    [Tooltip("Speed Run of the player movement.")]
    private float speedRun = 10f;

    private CharacterController characterController;
    private Vector3 velocity;
    private Animator animator;

    private bool isGround;


    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
        if (characterController == null)
        {
            Debug.LogError("CharacterController component not found on the player!");
        }
        animator = GetComponent<Animator>();
    }



    private void Update()
    {
        HandleMovement();
        HandleJump();

        animator.SetBool("isGround", characterController.isGrounded);


    }

    private void HandleMovement()
    {
        // קלט התנועה
        float z = Input.GetAxis("Vertical");

        // שינוי המהירות אם לוחצים על Shift
        bool isRunning = Input.GetKey(KeyCode.LeftShift);
        float currentSpeed = isRunning ? speedRun : speed;

        // כיוון התנועה
        Vector3 direction = new Vector3(0, 0, z).normalized;

        // שמירת המהירות הנוכחית
        Vector3 moveVelocity = transform.TransformDirection(direction) * currentSpeed;

        // הזזה של השחקן
        characterController.Move(moveVelocity * Time.deltaTime);

        // עדכון אנימציה
        bool isWalking = z != 0;
        animator.SetBool("walking", isWalking);
        animator.SetBool("Run", isRunning); // Run מופעל רק אם הולכים ולוחצים על Shift
    }
    private void HandleJump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && characterController.isGrounded)
        {
            velocity.y = jumpForce;
            animator.SetTrigger("jump");
        }

        // יישום כוח המשיכה
        if (!characterController.isGrounded)
        {
            velocity.y += Physics.gravity.y * Time.deltaTime;
        }
        else
        {
            velocity.y = 0; // אפס את המהירות האנכית כשהשחקן על הקרקע
        }

        characterController.Move(velocity * Time.deltaTime);
    }





}
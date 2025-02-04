using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{

    [SerializeField]
    [Tooltip("Speed of the player movement.")]
    private float speed ;

    [SerializeField]
    [Tooltip("Jump force for the player.")]
    private float jumpForce;

    [SerializeField]
    [Tooltip("Speed Run of the player movement.")]
    private float speedRun;

    [SerializeField]
    [Tooltip("Max Health for the Player")]
    public float HealthMax; // Maximum health for the player

    [SerializeField]
    [Tooltip("Mana Health for the Player")]
    public float ManaMax; // Maximum Mana for the player

   
    [SerializeField]
    [Tooltip("Gravity force applied to the player.")]
    private float gravity; // Custom gravity value
   
    [SerializeField]
    [Tooltip("How high the player can jump.")]
    private float jumpHeight; // Jump height in units
   

    [SerializeField]
    private AudioSource footstepAudioSource; // audioSouce for walking and running

    [SerializeField]
    [Tooltip("walking sound effect")]
    private AudioClip walkingSound; 

    [SerializeField]
    [Tooltip("// running Sound effect")]
    private AudioClip runningSound; 

    private CharacterController characterController;
    private Vector3 velocity;
    private Animator animator;
    GameManager gameManager;
    public UIManager manager;
    private Rigidbody rb;

    private void Awake()
    {

        rb = GetComponent<Rigidbody>();
        characterController = GetComponent<CharacterController>();
        gameManager=FindAnyObjectByType<GameManager>();

        if (characterController == null)
        {
            Debug.LogError("CharacterController component not found on the player!");
        }
        animator = GetComponent<Animator>();
    }



    private void FixedUpdate()
    {
        HandleMovement();
        animator.SetBool("isGround", characterController.isGrounded);

       
    }

    private void HandleMovement()
    {
        if (!characterController.enabled) return; 
        // Horizontal movement input
        float z = Input.GetAxis("Vertical");

        // Determine running speed
        bool isRunning = Input.GetKey(KeyCode.LeftShift);
        float currentSpeed = isRunning ? speedRun : speed;

        // Calculate horizontal movement direction
        Vector3 direction = new Vector3(0, 0, z).normalized;

        // Apply horizontal movement
        Vector3 moveVelocity = transform.TransformDirection(direction) * currentSpeed;

        // Apply vertical movement (gravity or jump)
        if (characterController.isGrounded)
        {
            // Reset vertical velocity when grounded
            velocity.y = -2f;

            // Handle jump input
            if (Input.GetKeyDown(KeyCode.Space))
            {
                velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity); // Jump calculation
                animator.SetTrigger("jump");
            }
        }
        else
        {
            // Apply gravity when not grounded
            velocity.y += gravity * Time.deltaTime;
        }

        // Combine horizontal and vertical velocities
        moveVelocity.y = velocity.y;

        // Move the character
        characterController.Move(moveVelocity * Time.deltaTime);
       
        // Update animations
        bool isWalking = z != 0;
        animator.SetBool("walking", isWalking);
        animator.SetBool("Run", isRunning);
        HandleFootstepSound(isWalking, isRunning);


    }
    private void HandleFootstepSound(bool isWalking, bool isRunning)
    {
        if (isWalking)
        {
            if (!footstepAudioSource.isPlaying) //check if the sound playing
            {
                footstepAudioSource.clip = isRunning ? runningSound : walkingSound;
                footstepAudioSource.Play();
            }
        }
        else
        {
            footstepAudioSource.Stop(); // if the player not walking stop the sound effect
        }
    }


    public void Take_Damage(float damage)
    {
        manager.Player_currentHealth -= damage;
        manager.Player_currentHealth = Mathf.Clamp(manager.Player_currentHealth, 0, HealthMax);

        // Update the health bar
        manager.UpdateHealthBar(manager.Player_currentHealth, HealthMax);

        // Check if the player is dead
        if (manager.Player_currentHealth <= 0)
        {
           
            StartCoroutine(Die());
        }
    }


    /// <summary>
    /// Heals the player and updates the slider.
    /// </summary>
    /// <param name="healAmount">The amount of health restored.</param>
    public void Heal(float amount)
    {
        manager.Player_currentHealth = Mathf.Clamp(manager.Player_currentHealth + amount, 0, HealthMax);
        Debug.Log($"Player healed by {amount}. Current health: {manager.Player_currentHealth}");
        manager.UpdateHealthBar(manager.Player_currentHealth, HealthMax);

    }
    public void AddMana(float amount)
    {
        manager.Player_currentMana = Mathf.Clamp(manager.Player_currentMana + amount, 0, ManaMax);
        Debug.Log($"Player mana increased by {amount}. Current mana: {manager.Player_currentMana}");
    }
    
    private IEnumerator Die()
    {
        characterController.enabled = false;
        rb.isKinematic = false; // ביטול מצב קינמטי, כדי שהדמות תיפול
        rb.useGravity = true; // הפעלת כוח הכבידה
        animator.SetTrigger("Die");
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        float animationLength = animator.runtimeAnimatorController.animationClips
            .FirstOrDefault(clip => clip.name == "Die")?.length ?? 2f;

        yield return new WaitForSeconds(animationLength);
        gameManager.GameOver();
        Destroy(gameObject);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Village"))
        {
            int villageId = other.GetComponent<VillageIdentifier>().villageId;
            Debug.Log("The player in village " + villageId);
            manager.SetCurrentVillage(villageId);
        }
    }





}
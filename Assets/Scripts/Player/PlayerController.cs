using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

/// <summary>
/// Controls player movement, jumping, animations, health, and interactions with the environment.
/// </summary>
public class PlayerController : MonoBehaviour
{
    [SerializeField]
    [Tooltip("Speed of the player movement.")]
    private float speed;

    [SerializeField]
    [Tooltip("Jump force for the player.")]
    private float jumpForce;

    [SerializeField]
    [Tooltip("Speed run of the player movement.")]
    private float speedRun;

    [SerializeField]
    [Tooltip("Max health for the player.")]
    public float HealthMax;

    [SerializeField]
    [Tooltip("Max mana for the player.")]
    public float ManaMax;

    [SerializeField]
    [Tooltip("Gravity force applied to the player.")]
    private float gravity;

    [SerializeField]
    [Tooltip("How high the player can jump.")]
    private float jumpHeight;

    [SerializeField]
    private AudioSource footstepAudioSource;

    [SerializeField]
    [Tooltip("Walking sound effect.")]
    private AudioClip walkingSound;

    [SerializeField]
    [Tooltip("Running sound effect.")]
    private AudioClip runningSound;

    private CharacterController characterController;
    private Vector3 velocity;
    private Animator animator;
    private GameManager gameManager;
    public UIManager manager;
    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        characterController = GetComponent<CharacterController>();
        gameManager = FindAnyObjectByType<GameManager>();

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

    /// <summary>
    /// Handles player movement, running, jumping, and animations.
    /// </summary>
    private void HandleMovement()
    {
        if (!characterController.enabled) return;

        float z = Input.GetAxis("Vertical");

        bool isRunning = Input.GetKey(KeyCode.LeftShift);
        float currentSpeed = isRunning ? speedRun : speed;

        Vector3 direction = new Vector3(0, 0, z).normalized;
        Vector3 moveVelocity = transform.TransformDirection(direction) * currentSpeed;

        if (characterController.isGrounded)
        {
            velocity.y = -2f;

            if (Input.GetKeyDown(KeyCode.Space))
            {
                velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
                animator.SetTrigger("jump");
            }
        }
        else
        {
            velocity.y += gravity * Time.deltaTime;
        }

        moveVelocity.y = velocity.y;
        characterController.Move(moveVelocity * Time.deltaTime);

        bool isWalking = z != 0;
        animator.SetBool("walking", isWalking);
        animator.SetBool("Run", isRunning);
        HandleFootstepSound(isWalking, isRunning);
    }

    /// <summary>
    /// Handles player footstep sounds based on movement state.
    /// </summary>
    private void HandleFootstepSound(bool isWalking, bool isRunning)
    {
        if (isWalking)
        {
            if (!footstepAudioSource.isPlaying)
            {
                footstepAudioSource.clip = isRunning ? runningSound : walkingSound;
                footstepAudioSource.Play();
            }
        }
        else
        {
            footstepAudioSource.Stop();
        }
    }

    /// <summary>
    /// Applies damage to the player and updates the health bar.
    /// </summary>
    /// <param name="damage">Amount of damage received.</param>
    public void Take_Damage(float damage)
    {
        manager.Player_currentHealth -= damage;
        manager.Player_currentHealth = Mathf.Clamp(manager.Player_currentHealth, 0, HealthMax);

        manager.UpdateHealthBar(manager.Player_currentHealth, HealthMax);

        if (manager.Player_currentHealth <= 0)
        {
            StartCoroutine(Die());
        }
    }

    /// <summary>
    /// Heals the player and updates the health UI.
    /// </summary>
    /// <param name="amount">Amount of health restored.</param>
    public void Heal(float amount)
    {
        manager.Player_currentHealth = Mathf.Clamp(manager.Player_currentHealth + amount, 0, HealthMax);
        Debug.Log($"Player healed by {amount}. Current health: {manager.Player_currentHealth}");
        manager.UpdateHealthBar(manager.Player_currentHealth, HealthMax);
    }

    /// <summary>
    /// Increases the player's mana.
    /// </summary>
    /// <param name="amount">Amount of mana restored.</param>
    public void AddMana(float amount)
    {
        manager.Player_currentMana = Mathf.Clamp(manager.Player_currentMana + amount, 0, ManaMax);
        Debug.Log($"Player mana increased by {amount}. Current mana: {manager.Player_currentMana}");
    }

    /// <summary>
    /// Handles player death, animations, and game over sequence.
    /// </summary>
    private IEnumerator Die()
    {
        characterController.enabled = false;
        rb.isKinematic = false;
        rb.useGravity = true;
        animator.SetTrigger("Die");

        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        float animationLength = animator.runtimeAnimatorController.animationClips
            .FirstOrDefault(clip => clip.name == "Die")?.length ?? 2f;

        yield return new WaitForSeconds(animationLength);
        gameManager.GameOver();
        Destroy(gameObject);
    }

    /// <summary>
    /// Detects when the player enters a village and updates the UI.
    /// </summary>
    /// <param name="other">The collider of the object that the player interacts with.</param>
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Village"))
        {
            int villageId = other.GetComponent<VillageIdentifier>().villageId;
            Debug.Log("The player is in village " + villageId);
            manager.SetCurrentVillage(villageId);
        }
    }
}

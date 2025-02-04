using UnityEngine;
/// <summary>
/// Manages player attack actions, including animations and sound effects.
/// </summary>
public class PlayerAttack : MonoBehaviour
{
    private Animator animator;
    private bool isAttack = false;

    [Tooltip("Audio source for the sword sound effect.")]
    public AudioSource AudioSwordEffect;

    [Tooltip("Sound effect for the sword.")]
    public AudioClip SwordSoundEffect;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.D))
        {
            isAttack = true;
            HandleAttack(0);
        }

        if (Input.GetMouseButtonDown(0))
        {
            HandleAttack(1);
        }

        if (Input.GetMouseButtonDown(1))
        { 
            HandleAttack(2);
        }

        isAttack = false;
        animator.SetBool("isAttack", isAttack);
    }

    /// <summary>
    /// Handles different attack animations based on the attack index.
    /// </summary>
    /// <param name="attackIndex">The attack type index.</param>
    private void HandleAttack(int attackIndex)
    {
        switch (attackIndex)
        { 
            case 0:
                animator.SetTrigger("armada");
                animator.SetTrigger("mmaKick");
                break;
            case 1:
                animator.SetTrigger("sword1");
                animator.SetTrigger("sword2");
                HandleSwordSoundEffect();
                break;
            case 2:
                animator.SetTrigger("slash1");
                HandleSwordSoundEffect();
                break;
        }
    }

    /// <summary>
    /// Plays the sword attack sound effect.
    /// </summary>
    private void HandleSwordSoundEffect()
    {  
        if (AudioSwordEffect != null && SwordSoundEffect != null)
        {
            AudioSwordEffect.PlayOneShot(SwordSoundEffect);
        }
    }
}

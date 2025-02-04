using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    private Animator animator;
    private bool isAttack=false;

    [Tooltip("audio source for the sowrd sound effect")]
    public AudioSource AudioSwordEffect;

    [Tooltip("sound effect for the sowrd ")]
    public AudioClip SowrdSoundEffect;
   

    private void Awake()
    {
        
        animator = GetComponent<Animator>();
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.D))
        {
            isAttack = true;
            handleAttack(0);
        }
        if (Input.GetMouseButtonDown(0))
        {
            handleAttack(1);
        }
        if (Input.GetMouseButtonDown(1))
        { 
            handleAttack(2);
        }
        isAttack = false;
        animator.SetBool("isAttack",isAttack);
    }
    private void handleAttack(int attackIndex)
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
                HandleSowrdSoundEffect();

                break;
            case 2:
                animator.SetTrigger("slash1");
                HandleSowrdSoundEffect();
                break;
        }
    }
    private void HandleSowrdSoundEffect()
    {  
        AudioSwordEffect.PlayOneShot(SowrdSoundEffect); 
    }
    
   

}

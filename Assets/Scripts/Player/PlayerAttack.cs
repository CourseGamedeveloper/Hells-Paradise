using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    private Animator animator;
    private bool isAttack=false;
   
    void Start()
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
                animator.SetTrigger("sword3");
                break;
            case 2:
                animator.SetTrigger("slash1");
                break;
        }
    }
    

}

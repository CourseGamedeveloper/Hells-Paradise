using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int maxHealt = 100;
    int currentHealth ;
    private Animator animator;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animator=GetComponent<Animator>();
        currentHealth=maxHealt;
    }

    public void Take_Damage(int damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            Die();
        }
    }
    private void Die()
    {
        animator.SetTrigger("die");
        StartCoroutine(DestroyAfterAnimation()); // Wait for the animation before destroying this object


    }
    private IEnumerator DestroyAfterAnimation()
    {
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);
        Destroy(gameObject); // Destroy this GameObject after the animation is done
    }


}

using Unity.VisualScripting;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D body;
   // [SerializeField]
   // [Tooltip("speed for the player movement")]
    private float speed = 5f;
    private Vector3 local_scale;
    private Animator animator;
    private bool grounded;
    private void Start()
    {
        body = GetComponent<Rigidbody2D>();
        local_scale= transform.localScale;
        animator = GetComponent<Animator>();
        
    }
   

    // Update is called once per frame
    private void Update()
    {
        float horizontal_Input = Input.GetAxis("Horizontal");
        // Corrected "Horizontal" axis spelling
        body.linearVelocity = new Vector2(horizontal_Input * speed, body.linearVelocity.y);

        //flip player ->Moving left & Right
        if (horizontal_Input >0.01f)
        {
            transform.localScale=new Vector3(local_scale.x,local_scale.y,local_scale.z);
        }else if (horizontal_Input < -0.01f)
        {
            transform.localScale=new Vector3(-1*local_scale.x,local_scale.y, 1);
        }
        if (Input.GetKey(KeyCode.Space)&&grounded)
        {
            Jump();
           
        }

        //set Animator parameters
         animator.SetBool("run",horizontal_Input !=0);
         animator.SetBool("grounded", grounded);
    }
    //this method make the player jump 
    private void Jump()
    {
        body.AddForce(Vector2.up * 300);
        animator.SetTrigger("jump");
        grounded = false;

    }
    //this method make the player stand on the ground 
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            grounded = true;
        }
    }


}

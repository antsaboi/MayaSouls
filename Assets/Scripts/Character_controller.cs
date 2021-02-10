using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character_controller : MonoBehaviour
{
    // public desimaaliluku nimeltä movingSpeed puolipiste

    public float movingSpeed = 5;
    
  //  public int jumpSpeed = 10; //
    
    public LayerMask groundMask;

    public float raycastDistance;

    private Rigidbody2D body;
    
    private Vector2 currentVelocity;

    private bool isGrounded;

    public Animator animator;

    string state = "IDLE";

    //Buttons

/*    bool buttonRight; 
    bool buttonLeft;
    bool buttonJump;*/
    
    //Movement forces

    public float moveSpeed;
    public float jumpSpeed;

    // Start is called before the first frame update
    void Start()
    {
        body = GetComponent<Rigidbody2D>();
        animator = GetComponentInChildren<Animator>();
    }

    /*    // Update is called once per frame

        private void Update()
        {
            GetInput();
            if (state == "IDLE") StateIdle();
            if (state == "RUN") StateRun();
            if (state == "JUMP") StateJump();
        }

        private void FixedUpdate()
        {
            if (buttonRight)
            {
                body.AddForce(Vector2.right * moveSpeed);
            }
            if (buttonJump)
            {
                body.AddForce(Vector2.up * jumpSpeed, ForceMode2D.Impulse);
            }
            if (buttonLeft)
            {
                body.AddForce(Vector2.left * moveSpeed);
            }
        }

        void StateJump()
        {
            Debug.Log("State is jump");
            if (body.velocity.y == 0) // Trigger Idle -> Jump
            {
                state = "IDLE";
                animator.SetBool("Jump", false);
                animator.SetBool("Idle", true);
            }
        }

        void StateIdle()
        {
            Debug.Log("State is idle");
            if (buttonRight) // Trigger Idle -> Run
            {
                state = "RUN";
                animator.SetBool("Run", true);
                animator.SetBool("Idle", false);
            }
            if (buttonJump) // Trigger Idle -> Jump
            {
                state = "JUMP";
                animator.SetBool("Jump", true);
                animator.SetBool("Idle", false);
            }
        }

        void StateRun()
        {
            Debug.Log("State is run");
            if (!buttonRight)
            {
                state = "IDLE";
                animator.SetBool("Run", false);
                animator.SetBool("Idle", true);
            }
        }

        void GetInput()
        {
            buttonRight = Input.GetKey("d");
            buttonJump = Input.GetKey("w");
            buttonLeft = Input.GetKey("a");
        }*/

    private void Update()
    {
        float xMovement = Input.GetAxisRaw("Horizontal");

        //animator.SetFloat("Speed", xMovement);

        currentVelocity = new Vector2(movingSpeed * xMovement, body.velocity.y);

        if (Mathf.Abs(currentVelocity.x) > 3.5f)
        {
            animator.SetBool("Run", true);
        }
        else {
            animator.SetBool("Run", false);
        }

        animator.SetFloat("YSpeed", currentVelocity.y);

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            currentVelocity.y = jumpSpeed;
        }


        if (xMovement > 0.1)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
        else if (xMovement < -0.1f) {
            transform.localScale = new Vector3(-1, 1, 1);
        }

        body.velocity = currentVelocity;
    }

    void FixedUpdate()
    {
        Debug.DrawRay(transform.position, new Vector2(0, -raycastDistance), Color.white, 01f);

        RaycastHit2D hit = Physics2D.Raycast(transform.position, -Vector2.up, raycastDistance, groundMask);

        // If it hits something...
        if (hit.collider != null)
        {
            if (hit.collider.gameObject.CompareTag("Ground"))
            {
                isGrounded = true;
                animator.SetBool("InAir", false);
            }

        }
        else
        {
            isGrounded = false;
            animator.SetBool("InAir", true);
        }
    }

    //Sillä hetkellä kun osutaan toiseen collideriin

    void OnCollisionEnter2D(Collision2D other)
    {
        if(other.gameObject.tag == "Ground")
        {
            //isGrounded = true;
        }
    }

    //Sillä hetkellä kun poistutaan kosketuksista toisen colliderin kanssa

    void OnCollisionExit2D(Collision2D other)
    {
        //Jos osuttiin toiseen peliobjektiin, jonka tagi on ''Ground''
        //isGrounded = false
        if (other.gameObject.tag == "Ground")
        {
           // isGrounded = false;
        }
    }

}

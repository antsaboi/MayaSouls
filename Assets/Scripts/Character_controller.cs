using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character_controller : MonoBehaviour
{
    // Onii-san yameroo ittai sore wa hairanai

    public PlayerStats stats;
    public float movingSpeed = 5;
    public float absMinMovingSpeed;
    [Range(1, 40)]
    public float speedDecreaseFactor = 0.1f, speedBuildFactor = 1f;
    public float jumpSpeed;
    public LayerMask groundMask;
    public float raycastDistance;

    [HideInInspector]
    public Rigidbody2D body;
    private Vector2 currentVelocity;
    private bool isGrounded;
    private Animator animator;
    [HideInInspector] public bool reduceHP = true;

/*    string state = "IDLE";*/

    //Buttons
/*
    bool buttonRight; 
    bool buttonLeft;
    bool buttonJump;
    */
    //Movement forces

/*    public float moveSpeed;*/

    // Start is called before the first frame update
    void Start()
    {
        body = GetComponent<Rigidbody2D>();
        animator = GetComponentInChildren<Animator>();
        stats.ResetStats();
        StartHPReduce();
    }

    void Update()
    {
        float xMovement = Input.GetAxisRaw("Horizontal");

        if (Mathf.Abs(xMovement) > 0)
        {
            float speedIncrease = Mathf.MoveTowards(body.velocity.x, movingSpeed * xMovement, speedBuildFactor * Time.deltaTime);
            currentVelocity = new Vector2(speedIncrease, body.velocity.y);
        }
        else {
            float speedDecrease = Mathf.MoveTowards(body.velocity.x, 0, speedDecreaseFactor * Time.deltaTime);
            currentVelocity = new Vector2(speedDecrease, body.velocity.y);
        }

        if (xMovement > 0.1f)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
        else if (xMovement < -0.1f)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            currentVelocity.y = jumpSpeed;
            /*isGrounded = false;*/
        }

        body.velocity = currentVelocity;

        animator.SetFloat("XSpeed", Mathf.Abs(currentVelocity.x));
        animator.SetFloat("YSpeed", currentVelocity.y);
    }

    private void FixedUpdate()
    {
        isGrounded = GroundCheck();
        animator.SetBool("Grounded", isGrounded);
    }

    bool GroundCheck()
    {
        Debug.DrawRay(transform.position, new Vector2(0, -raycastDistance), Color.white, 01f);

        RaycastHit2D hit = Physics2D.Raycast(transform.position, -Vector2.up, raycastDistance, groundMask);

        // If it hits something...
        if (hit.collider != null)
        {
            if (hit.collider.gameObject.CompareTag("Ground"))
            {
                return true;
            }
        }

        return false;
    }

    void StartHPReduce()
    {
        StartCoroutine(HPReduce());
    }

    IEnumerator HPReduce()
    {
        while (stats.HP >= 0)
        {
            if (!reduceHP)
            {
                yield return new WaitForEndOfFrame();
                continue;
            }

            stats.ReduceHPBySecondRate();
            yield return new WaitForSeconds(1f);
        }
    }

    // Update is called once per frame
    /*
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

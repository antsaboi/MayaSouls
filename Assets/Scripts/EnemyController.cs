using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [Header("References")]
    public Transform groundDetection;
    public Transform wallDetection;

    [Header("General")]
    public float moveSpeed;

    [Header("Attacks")]
    public float chargeForce;
    public float chargeDistance;
    public float attackDistance;

    bool movingRight = true;
    int hDirection = 1;
    Rigidbody2D rb;
    float lastCharge;
    GameObject target;
    Animator anim;
    bool charging = false;
    bool attacking;


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        lastCharge = Time.time;
        anim = GetComponentInChildren<Animator>();
        anim.SetBool("Walking", true);
    }

    // Update is called once per frame
    void Update()
    {
        CheckForGround();
        CheckForWall();

        //transform.Translate(Vector2.right * moveSpeed * Time.deltaTime);

        if (!target) target = GameObject.FindGameObjectWithTag("Player");

        if (Vector2.Distance(transform.position, target.transform.position) < chargeDistance)
        {
            if (Vector2.Distance(transform.position, target.transform.position) < attackDistance)
            {
                anim.SetBool("Charge", false);
                charging = false;
                anim.SetBool("Attacking", true);
                attacking = true;
            }
            else {
                anim.SetBool("Charge", true);
                charging = true;
                anim.SetBool("Attacking", false);
                attacking = false;
            }
        }
        else
        {
            anim.SetBool("Attacking", false);
            anim.SetBool("Charge", false);
            charging = false;
            attacking = false;
        }

        if (!charging) rb.velocity = Vector2.right * moveSpeed * hDirection;
        else rb.velocity = Vector2.right * chargeForce * hDirection;

        if (attacking) rb.velocity = Vector2.zero;
    }

    void Turn()
    {
        //AudioManager.instance.PlaySound("Jump");
        if (movingRight == true)
        {
            movingRight = false;
            transform.eulerAngles = new Vector3(0, -180, 0);
        }

        else
        {
            transform.eulerAngles = new Vector3(0, 0, 0);
            movingRight = true;
        }

        if (movingRight) hDirection = 1;
        else hDirection = -1;

        rb.velocity = Vector3.zero;
        rb.angularVelocity = 0;

    }

    void CheckForGround()
    {
        int layerMask = LayerMask.GetMask("Ground");
        RaycastHit2D groundInfo = Physics2D.Raycast(groundDetection.position, Vector2.down, 0.3f, layerMask);
        if (groundInfo.collider == false) Turn();

    }

    void CheckForWall()
    {
        int layerMask = LayerMask.GetMask("Ground");
        RaycastHit2D hit = Physics2D.Raycast(wallDetection.position, Vector2.right * hDirection, 0.01f, layerMask);

        if (hit.collider) Turn();

    }
}

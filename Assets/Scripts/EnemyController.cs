using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class EnemyController : MonoBehaviour
{
    [Header("References")]
    public Transform groundDetection;
    public Transform wallDetection;

    [Header("General")]
    public float moveSpeed;
    public int HP = 2;

    [Header("Attacks")]
    [Range(0, 100)]
    public float touchDamage, attackDamage;
    public float chargeForce;
    public float chargeDistance;
    public float attackDistance;
    public float waitTimeAfterTurn;
    public BoxCollider2D hitBox;

    [HideInInspector] public float currentDamage;
    bool movingRight = true;
    int hDirection = 1;
    Rigidbody2D rb;
    float lastCharge;
    GameObject target;
    Animator anim;
    bool charging = false;
    bool attacking;
    bool isAlive = true;
    bool damaged = false;
    float waitTimeStamp;
    bool hasTurned;
    bool grounded;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        lastCharge = Time.time;
        anim = GetComponentInChildren<Animator>();
        anim.SetBool("Walking", true);
        var hit = Physics2D.Raycast(groundDetection.position, Vector2.down, 100f, LayerMask.GetMask("Ground"));
        if (hit.collider != null) transform.position = hit.point;
        currentDamage = touchDamage;
        hasTurned = true;
        if (!target) target = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if (!isAlive) return;
        if (damaged)
        {
            return;
        }

        bool playerInReach = Vector2.Distance(transform.position, target.transform.position) < chargeDistance &&
    (target.transform.position.y - transform.position.y < 0.5f && target.transform.position.y - transform.position.y > -0.5f);


        if (!playerInReach)
        {
            if (waitTimeStamp > Time.time)
            {
                anim.SetBool("Walking", false);
                rb.velocity = Vector3.zero;
                rb.angularVelocity = 0;
                return;
            }
            else
            {
                if (!hasTurned)
                {
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
                    hasTurned = true;
                }
            }
        }
        else {
            waitTimeStamp = 0;
            if (!hasTurned)
            {
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
                hasTurned = true;
            }
        }

        CheckForGround();
        CheckForWall();
        //transform.Translate(Vector2.right * moveSpeed * Time.deltaTime);

        if (playerInReach)
        {
            if (target.transform.position.x < transform.position.x)
            {
                if (movingRight) Turn();
            }
            else
            {
                if (!movingRight) Turn();
            }

            if (Vector2.Distance(transform.position, target.transform.position) < attackDistance)
            {
                anim.SetBool("Charge", false);
                charging = false;
                anim.SetBool("Attacking", true);
                attacking = true;
            }
            else
            {
                currentDamage = touchDamage;
                anim.SetBool("Charge", true);
                charging = true;
                anim.SetBool("Attacking", false);
                attacking = false;
            }
        }
        else
        {
            currentDamage = touchDamage;
            anim.SetBool("Attacking", false);
            anim.SetBool("Charge", false);
            charging = false;
            attacking = false;
        }

        if (!charging) rb.velocity = new Vector2(1, 0) * moveSpeed * hDirection;
        else rb.velocity = Vector2.right * chargeForce * hDirection;

        if (attacking) rb.velocity = Vector2.zero;
    }

    void Turn()
    {
        waitTimeStamp = Time.time + waitTimeAfterTurn;
        hasTurned = false;
        //AudioManager.instance.PlaySound("Jump");
        rb.velocity = Vector3.zero;
        rb.angularVelocity = 0;
    }

    public void HitStart()
    {
        hitBox.enabled = true;
        currentDamage = attackDamage;
    }

    public void HitEnd()
    {
        hitBox.enabled =false;
    }

    public void DamageStart()
    {
        damaged = true;
    }

    public void DamageEnd()
    {
        damaged = false;
    }

    void CheckForGround()
    {
        int layerMask = LayerMask.GetMask("Ground");
        RaycastHit2D groundInfo = Physics2D.Raycast(groundDetection.position, Vector2.down, 0.3f, layerMask);
        if (groundInfo.collider == true)
        {
            grounded = true;
            anim.SetBool("Walking", true);
        }
        if (groundInfo.collider == false)
        {
            grounded = false;
            Turn();
        }
    }

    void CheckForWall()
    {
        int layerMask = LayerMask.GetMask("Ground");
        RaycastHit2D hit = Physics2D.Raycast(wallDetection.position, Vector2.right * hDirection, 0.01f, layerMask);

        if (hit.collider) Turn();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (damaged) return;

            var player = collision.GetComponent<ProtoPlayer2D>();
            if (player.isAttacking)
            {
                HP--;
                HitEnd();
                anim.SetTrigger("Damage");

                if (HP <= 0)
                {
                    Die();
                }
            }
        }
    }

    void Die()
    {
        rb.velocity = Vector2.zero;
        isAlive = false;
        anim.SetTrigger("Death");
        var cols = GetComponentsInChildren<Collider2D>();

        foreach (var col in cols)
        {
            col.enabled = false;
        }
    }
}

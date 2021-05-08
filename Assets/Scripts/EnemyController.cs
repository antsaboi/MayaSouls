using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class EnemyController : MonoBehaviour
{
    [Header("References")]
    public Transform groundDetection;
    public Transform wallDetection;
    public ParticleSystem damageParticles;

    [Header("General")]
    public float moveSpeed;
    public int HP = 2;
    public AudioClip chaseScream, diesScream, attackSound, attackLandSound;

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
    bool haveToWait;
    float screamTimer;

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
        if (!GameManager.instance.isAlive)
        {
            rb.velocity = Vector3.zero;
            rb.angularVelocity = 0;
            anim.SetBool("Walking", false);
            anim.SetBool("Attacking", false);
            anim.SetBool("Charge", false);
            charging = false;
            attacking = false;
            return;
        }

        if (damaged)
        {
            return;
        }

        bool playerInReach = Vector2.Distance(transform.position, target.transform.position) < chargeDistance &&
    (target.transform.position.y - transform.position.y < 5f && target.transform.position.y - transform.position.y > -0.5f);


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
                        damageParticles.transform.rotation = Quaternion.AngleAxis(-90, Vector3.forward);
                    }

                    else
                    {
                        transform.eulerAngles = new Vector3(0, 0, 0);
                        movingRight = true;
                        damageParticles.transform.rotation = Quaternion.AngleAxis(90, Vector3.forward);
                    }

                    if (movingRight) hDirection = 1;
                    else hDirection = -1;
                    hasTurned = true;
                    haveToWait = false;
                }
            }
        }
        else {
            if (!haveToWait) waitTimeStamp = 0;
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
                        damageParticles.transform.rotation = Quaternion.AngleAxis(-90, Vector3.forward);
                    }

                    else
                    {
                        transform.eulerAngles = new Vector3(0, 0, 0);
                        movingRight = true;
                        damageParticles.transform.rotation = Quaternion.AngleAxis(90, Vector3.forward);
                    }

                    if (movingRight) hDirection = 1;
                    else hDirection = -1;
                    hasTurned = true;
                }
            }
        }

        if (!CheckForGround())
        {
            haveToWait = true;
            Turn();
            return;
        }
        if (CheckForWall())
        {
            haveToWait = true;
            Turn();
            return;
        }

        if (playerInReach)
        {
            if (Time.time > screamTimer)
            {
                AudioSystem.instance.PlayOneShot(chaseScream);
                screamTimer = Time.time + 3f;
            }

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
        AudioSystem.instance.PlayOneShot(attackSound);
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

    bool CheckForGround()
    {
        int layerMask = LayerMask.GetMask("Ground");
        RaycastHit2D groundInfo = Physics2D.Raycast(groundDetection.position, Vector2.down, 0.3f, layerMask);
        if (groundInfo.collider)
        {
            grounded = true;
            anim.SetBool("Walking", true);
            return true;
        }else 
        {
            grounded = false;
            return false;
        }
    }

    bool CheckForWall()
    {
        int layerMask = LayerMask.GetMask("Ground");
        RaycastHit2D hit = Physics2D.Raycast(wallDetection.position, Vector2.right * hDirection, 0.01f, layerMask);

        if (hit.collider) return true;
        else return false;
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
                damageParticles.Play();
                GameManager.instance.SoulPickedUp?.Raise();
                AudioSystem.instance.PlayOneShot(attackLandSound);

                if (HP <= 0)
                {
                    Die();
                }
            }
        }
    }

    void Die()
    {
        AudioSystem.instance.PlayOneShot(diesScream);
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

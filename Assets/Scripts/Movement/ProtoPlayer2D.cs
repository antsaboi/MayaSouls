using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;

public class ProtoPlayer2D : MonoBehaviour
{
    public PlayerStats stats;
    public PlatformerMovementWASD[] behaviours;
    public Vector2 velocity;
    public SkeletonMecanim spine;
    public ParticleSystem deathParticles;
    public float invulnerabilityTime;
    public int attackDamage;
    public BoxCollider2D hitBox;

    [HideInInspector] public bool grounded;
    [HideInInspector] public bool reduceHP = true;
    [HideInInspector] public bool isAttacking;
    Animator animator;
    bool powerUse;
    private float invulnerabilityTimeStamp;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < behaviours.Length; i++)
        {
            behaviours[i].Initialize(gameObject);
            behaviours[i].StartBehaviour();
        }
        animator = GetComponentInChildren<Animator>();
        stats.ResetStats();
        StartHPReduce();
    }

    // Update is called once per frame
    void Update()
    {
        if (powerUse || isAttacking) return;

        for (int i = 0; i < behaviours.Length; i++)
        {
            behaviours[i].UpdateBehaviour();
        }

        if (grounded && !isAttacking && Input.GetMouseButtonDown(0))
        {
            animator.SetInteger("Attack", Random.Range(1, 4));
        }
    }

    private void FixedUpdate()
    {
        if (powerUse || isAttacking)
        {
            if (!grounded) AttackEnd();
            return;
        }

        for (int i = 0; i < behaviours.Length; i++)
        {
            behaviours[i].FixedUpdateBehaviour();

            velocity = behaviours[i].currentVelocity;
            animator.SetBool("Grounded", grounded = behaviours[i].grounded);
            animator.SetFloat("XSpeed", Mathf.Abs(velocity.x));
            animator.SetFloat("YSpeed", velocity.y);
        }
    }

    public void StartGame()
    {
        if(GameManager.lastCheckPoint != Vector3.zero) transform.position = GameManager.lastCheckPoint;
    }

    public void StartPowerUse()
    {
        behaviours[0].currentVelocity = Vector2.zero;
        animator.SetFloat("XSpeed", 0);
        animator.SetBool("Power", powerUse = true);
    }

    public void EndPowerUse()
    {
        animator.SetBool("Power", powerUse = false);
    }

    void StartHPReduce()
    {
        StartCoroutine(HPReduce());
    }

    public void HealPlayer()
    {
        stats.HP += GameManager.instance.soulPickUpHealAmount;
        stats.HP = Mathf.Clamp(stats.HP, 0, 100);
    }

    IEnumerator HPReduce()
    {
        while (stats.HP > -1)
        {
            if (!reduceHP)
            {
                yield return new WaitForEndOfFrame();
                continue;
            }

            stats.ReduceHPBySecondRate();
            yield return new WaitForSeconds(stats.hpReduceInterval);
        }
    }

    private void OnDestroy()
    {
        for (int i = 0; i < behaviours.Length; i++)
        {
            behaviours[i].OnDestroyBehaviour();
        }
    }

    //damage
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!GameManager.instance.isAlive) return;

        if (invulnerabilityTimeStamp > Time.time)
        {
            return;
        }

        if (collision.CompareTag("Enemy") && !isAttacking)
        {
            var script = collision.GetComponent<EnemyController>();
            stats.ReduceHP(script.currentDamage);
            behaviours[0].TakeDamage(new Vector2(7, 5));
            CameraController.instance.ShakeCamera(2f, 0.5f);
            animator.SetTrigger("Damage");
            invulnerabilityTimeStamp = Time.time + invulnerabilityTime;
        }

        if (collision.GetComponent<DamageDealer>() is DamageDealer d)
        {
            stats.ReduceHP(d.damage);
            behaviours[0].TakeDamage(d.knockBack);
            CameraController.instance.ShakeCamera(3, 0.2f);
            animator.SetTrigger("Damage");
            invulnerabilityTimeStamp = Time.time + invulnerabilityTime;
        }
    }

    public void Die()
    {
        animator.SetBool("IsAlive", GameManager.instance.isAlive);
        CameraController.instance.ZoomTo(2, 6f);
    }

    public void DeathShakeCam()
    {
        deathParticles.Play();
        CameraController.instance.ShakeCamera(2, 2f);
    }

    public void AttackStart()
    {
        hitBox.enabled = true;
        isAttacking = true;
        behaviours[0].currentVelocity = Vector2.zero;
    }

    public void AttackEnd()
    {
        isAttacking = false;
        hitBox.enabled = false;
        animator.SetInteger("Attack", 0);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;
using DG.Tweening;

public class ProtoPlayer2D : MonoBehaviour
{
    public PlayerStats stats;
    public PlatformerMovementWASD[] behaviours;
    public Vector2 velocity;
    public SkeletonMecanim spine;
    public ParticleSystem deathParticles, winParticles;
    public float invulnerabilityTime;
    public int attackDamage;
    public BoxCollider2D hitBox;
    public ParticleSystem runningParticles, damageParticles;

    [Header("Audio")]
    [SerializeField] AudioClip attackSound1;
    [SerializeField] AudioClip attackSound2, deathSound, jumpSound, landSound, stepSound1, stepSound2, attackLandSound;

    [HideInInspector] public bool grounded;
    [HideInInspector] public bool reduceHP = true;
    [HideInInspector] public bool isAttacking;
    Animator animator;
    bool powerUse;
    bool hasSecondAttack = false;
    private float invulnerabilityTimeStamp;
    bool winGame;

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
        if (GameManager.instance.isAlive && isAttacking && (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.RightArrow)))
        {
            animator.SetBool("CancelAttack", true);
            AttackEnd();
            Invoke("AttackEnd", 0.2f);
        }

        if (GameManager.instance.isAlive && isAttacking && !hasSecondAttack && Input.GetMouseButtonDown(0))
        {
            animator.SetBool("CancelAttack", false);
            hasSecondAttack = true;
            animator.SetBool("SecondAttack", true);
            isAttacking = true;
        }

        if (powerUse || isAttacking) return;

        for (int i = 0; i < behaviours.Length; i++)
        {
            behaviours[i].UpdateBehaviour();
        }

        if (GameManager.instance.isAlive && grounded && Input.GetMouseButtonDown(0))
        {
            animator.SetBool("CancelAttack", false);
            animator.SetInteger("Attack", 1);
            isAttacking = true;
        }
    }

    private void FixedUpdate()
    {
        if (!GameManager.instance.isAlive) return;

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

    public void WinGame()
    {
/*        animator.gameObject.transform.DOScale(0, 0.3f);
        winParticles.Play();*/
    }

    public void JumpSound()
    {
        AudioSystem.instance.PlayOneShot(jumpSound, 0.3f);
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
            damageParticles.Play();
        }

        if (!isAttacking && collision.GetComponent<DamageDealer>() is DamageDealer d)
        {
            TakeDamage(d.damage, d.knockBack);
        }
    }

    public void TakeDamage(int damage, Vector2 knockBack)
    {
        damageParticles.Play();
        AudioSystem.instance.PlayOneShot(attackLandSound);
        stats.ReduceHP(damage);
        behaviours[0].TakeDamage(knockBack);
        CameraController.instance.ShakeCamera(3, 0.2f);
        animator.SetTrigger("Damage");
        invulnerabilityTimeStamp = Time.time + invulnerabilityTime;
    }

    public void WalkSound()
    {
        AudioSystem.instance.PlayOneShot(Random.Range(0,1f) > 0.5f ? stepSound1 : stepSound2, 0.2f);
    }

    public void Die()
    {
        animator.SetBool("IsAlive", GameManager.instance.isAlive);
    }

    public void DeathShakeCam()
    {
        deathParticles.Play();
        AudioSystem.instance.PlayOneShot(deathSound);
        CameraController.instance.ShakeCamera(2, 2f);
    }

    public void AttackStart()
    {
        isAttacking = true;
        AudioSystem.instance.PlayOneShot(attackSound1, 0.5f);
        animator.SetBool("SecondAttack", false);
        hasSecondAttack = false;
        hitBox.enabled = true;
        behaviours[0].currentVelocity = Vector2.zero;
    }

    public void AttackEnd()
    {
        isAttacking = false;
        hitBox.enabled = false;
        animator.SetInteger("Attack", 0);
    }
}

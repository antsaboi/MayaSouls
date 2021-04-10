using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;

public class ProtoPlayer2D : MonoBehaviour
{
    public PlayerStats stats;
    public ProtoPlayerBehaviourBase[] behaviours;
    public Vector2 velocity;
    bool grounded;
    private Animator animator;
    [HideInInspector] public bool reduceHP = true;
    public SkeletonMecanim spine;
    public ParticleSystem deathParticles;

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
        for (int i = 0; i < behaviours.Length; i++)
        {
            behaviours[i].UpdateBehaviour();
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            animator.SetTrigger("Attack");
            //body.velocity = Vector2.zero;
        }
    }

    private void FixedUpdate()
    {
        for (int i = 0; i < behaviours.Length; i++)
        {
            behaviours[i].FixedUpdateBehaviour();

            if (behaviours[i] is PlatformerMovementWASD p)
            {
                velocity = p.currentVelocity;
                animator.SetBool("Grounded", p.grounded);
                animator.SetFloat("XSpeed", Mathf.Abs(velocity.x));
                animator.SetFloat("YSpeed", velocity.y);
            }
        }
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

    public void Die()
    {
        animator.SetBool("IsAlive", GameManager.instance.isAlive);
        deathParticles.Play();
        Invoke("DeathShakeCam", 1f);
        CameraController.instance.ZoomTo(2, 4.2f);
    }

    void DeathShakeCam()
    {
        CameraController.instance.ShakeCamera(2, 2f);
    }
}

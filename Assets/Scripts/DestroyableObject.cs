using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class DestroyableObject : MonoBehaviour
{
    public int HP = 2;
    [Range(0, 100)]
    public int damage;
    public Vector2 knockBack;

    Animator anim;
    ParticleSystem[] particles;
    private void Awake()
    {
        anim = GetComponentInChildren<Animator>();
        particles = GetComponentsInChildren<ParticleSystem>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            var player = collision.GetComponent<ProtoPlayer2D>();
            if (player.isAttacking)
            {
                HP--;
                transform.DOShakePosition(0.05f, 0.3f);

                foreach (var particle in particles)
                {
                    particle.Play();
                }

                if (HP <= 0)
                {
                    Break();
                }
            }
            else {
                player.TakeDamage(damage, knockBack);
            }
        }
    }

    void Break()
    {
        anim.SetTrigger("Break");
        var cols = GetComponentsInChildren<Collider2D>();

        foreach (var col in cols)
        {
            col.enabled = false;
        }
    }
}

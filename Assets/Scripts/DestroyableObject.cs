using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyableObject : MonoBehaviour
{
    public int HP = 2;
    public Animator anim;
    [Range(0, 100)]
    public int damage;
    public Vector2 knockBack;

    private void Awake()
    {
        anim = GetComponentInChildren<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            var player = collision.GetComponent<ProtoPlayer2D>();
            if (player.isAttacking)
            {
                HP--;

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

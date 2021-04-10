using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathCollision : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!GameManager.instance.isAlive) return;

        if (collision.CompareTag("Player"))
        {
            GameManager.instance.GameOver();
        }
    }
}

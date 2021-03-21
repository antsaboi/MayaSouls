using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoneForceAdder : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            GetComponent<Rigidbody2D>().AddForce(collision.GetComponent<Rigidbody2D>().velocity.normalized * 5, ForceMode2D.Impulse);
        }
    }
}

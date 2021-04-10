using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoneForceAdder : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            GetComponent<Rigidbody2D>().AddForce(collision.GetComponent<ProtoPlayer2D>().velocity * 0.6f, ForceMode2D.Impulse);
        }
    }
}

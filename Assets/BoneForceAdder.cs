using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(CircleCollider2D))]
public class BoneForceAdder : MonoBehaviour
{
    public enum ForceType
    {
        Force,
        AngularForce
    }
    public ForceType forceType;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            switch (forceType)
            {
                case ForceType.AngularForce:
                    GetComponent<Rigidbody2D>().AddTorque(collision.GetComponent<ProtoPlayer2D>().velocity.x);
                return;
            }

            GetComponent<Rigidbody2D>().AddForce(collision.GetComponent<ProtoPlayer2D>().velocity * 0.6f, ForceMode2D.Impulse);
        }
    }

    private void OnValidate()
    {
        GetComponent<CircleCollider2D>().isTrigger = true;
    }
}

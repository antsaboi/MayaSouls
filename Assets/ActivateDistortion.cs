using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateDistortion : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player")) CameraController.instance.ActivateDistort();
    }
}

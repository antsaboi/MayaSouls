using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class TrapTrigger : MonoBehaviour
{
    [SerializeField] Animator spikesAnimator;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            spikesAnimator.SetTrigger("Activate");
            transform.DOPunchPosition(new Vector3(0, -0.05f, 0), 1f, 2);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class TrapTrigger : MonoBehaviour
{
    [SerializeField] Animator spikesAnimator;
    [SerializeField] AudioClip trapGoesUp, trapGoesDown;
    float trapTimer;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (Time.time < trapTimer) return;

            trapTimer = Time.time + 2f;
            spikesAnimator.SetTrigger("Activate");
            transform.DOPunchPosition(new Vector3(0, -0.05f, 0), 1f, 2);
            AudioSystem.instance.PlayOneShot(trapGoesUp, 0.5f);
            Invoke(nameof(InvokeTrapGoesDown), 1.5f);
        }
    }

    void InvokeTrapGoesDown()
    {
        AudioSystem.instance.PlayOneShot(trapGoesDown, 0.5f);
    }
}

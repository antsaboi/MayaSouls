using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinCollider : MonoBehaviour
{
    [SerializeField] AudioClip clip;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            AudioSystem.instance.PlayOneShot(clip);
            AudioSystem.instance.PlayGameMusic();
            GameManager.instance.WinGame();
        }
    }
}

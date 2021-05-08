using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heart : MonoBehaviour
{
    public float parallaxAmount = 5f;
    [Range(0,1)]
    public float blurRange = 0;
    [SerializeField] SpriteRenderer[] blurredSprites;

    [SerializeField] AudioClip clip;
    bool playAudio;
    float audioPlayTime;
    Transform player;
    float maxDist;
    Vector3 startPos;

    private void Start()
    {
        startPos = transform.position;
    }

    private void Update()
    {
        if (player != null && playAudio)
        {
            if (Time.time > audioPlayTime)
            {
                AudioSystem.instance.PlayOneShot(clip, Mathf.Abs(Mathf.Clamp(1 - Vector2.Distance(transform.position, player.transform.position) / maxDist, 0.2f, 0.8f)));
                audioPlayTime = Time.time + clip.length + 0.2f;
            }

            if (Vector2.Distance(transform.position, player.transform.position) > maxDist + 2f)
            {
                playAudio = false;
            }
        }
    }

    private void OnValidate()
    {
        for (int i = 0; i < blurredSprites.Length; i++)
        {
            blurredSprites[i].color = new Color(1,1,1,blurRange);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            player = collision.transform;
            maxDist = Vector2.Distance(transform.position, player.transform.position);
            playAudio = true;
        }
    }

}

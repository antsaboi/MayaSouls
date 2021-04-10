using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    [SerializeField] SpriteRenderer distortion;
    [SerializeField] ParticleSystem EnterParticles;
    [SerializeField] GameEvent CheckPointReached;

    ProtoPlayer2D playerBody;
    bool isEntered;

    Material distortionMat;
    float distortionSpeed;
    float veloRef;
    Animator anim;
    bool activated = false;

    private void Start()
    {
        distortionMat = distortion.material;
        distortion.material.SetVector("Tiling", new Vector4(0.5f, 0.5f, 0, 0));
        distortion.material.SetFloat("Speed", 0.075f);
        distortion.material.SetVector("Position", new Vector4(0.005f, 0.005f, 0,0));
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        if (isEntered)
        {
            var speed = distortion.material.GetFloat("Speed");
            distortionSpeed = speed;

            if (playerBody.velocity.sqrMagnitude > 2)
            {
                var normalized = playerBody.velocity.normalized;
                var dirX = Mathf.Sign(normalized.x);
                var dirY = Mathf.Sign(normalized.y);
                float x = Mathf.Abs(normalized.x) > 0.2f ? normalized.x : 0.2f * dirX;
                float y = Mathf.Abs(normalized.y) > 0.2f ? normalized.y : 0.2f * dirY;

                distortion.material.SetVector("Tiling", new Vector4(-x, -y, 0, 0));

                distortionSpeed = Mathf.SmoothDamp(distortionSpeed, 0.1f, ref veloRef, 0.1f * Time.deltaTime);
                distortion.material.SetFloat("Speed", distortionSpeed);
            }
        }
    }

    public void Enter(Vector2 direction)
    {
        if (!activated)
        {
            anim.SetTrigger("Enter");
            activated = true;
            CheckPointReached.Raise();
        }
        distortion.material.SetVector("Tiling", new Vector4(-direction.x, -direction.y, 0, 0));
        distortion.material.SetVector("Position", new Vector4(0.01f, 0.01f, 0, 0));
        isEntered = true;
    }

    public void Exit(Vector2 direction)
    {
        distortion.material.SetVector("Tiling", new Vector4(-direction.x, -direction.y, 0, 0));
        isEntered = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            //Save game
            var player = collision.transform.GetComponentInChildren<ProtoPlayer2D>();
            if (playerBody is null) playerBody = player;
            player.stats.ResetStats();
            player.reduceHP = false;

            EnterParticles.transform.position = collision.transform.position;
            if (playerBody.velocity.y < 1 && playerBody.velocity.y > -1) EnterParticles.transform.position += Vector3.up;
            float angle = Mathf.Atan2(-playerBody.velocity.y, -playerBody.velocity.x) * Mathf.Rad2Deg;
            EnterParticles.transform.rotation = Quaternion.AngleAxis(angle - 90, Vector3.forward);
            EnterParticles.Play();
            
            Enter(playerBody.velocity.normalized);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            var player = collision.transform.GetComponentInChildren<ProtoPlayer2D>();
            if (playerBody is null) playerBody = player;
            player.reduceHP = true;

            EnterParticles.transform.position = collision.transform.position;
            if (playerBody.velocity.y < 1 && playerBody.velocity.y > -1) EnterParticles.transform.position += Vector3.up;
            float angle = Mathf.Atan2(playerBody.velocity.y, playerBody.velocity.x) * Mathf.Rad2Deg;
            EnterParticles.transform.rotation = Quaternion.AngleAxis(angle - 90, Vector3.forward);
            EnterParticles.Play();

            Exit(playerBody.velocity.normalized);
        }
    }
}

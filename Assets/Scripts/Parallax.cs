using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    [SerializeField] float scrollSpeed = 0.1f;

    [SerializeField] Material parallaxMat;
    [SerializeField] ProtoPlayer2D player;
    [SerializeField] Camera mainCam;
    Vector3 previousPos;

    private void Start()
    {
        if (player is null)
        {
            player = FindObjectOfType<ProtoPlayer2D>();
        }

        if (mainCam is null)
        {
            mainCam = Camera.main;
        }
    }

    private void LateUpdate()
    {
        var speed = mainCam.transform.position - previousPos;

        var current = parallaxMat.GetVector("_offset");
        parallaxMat.SetVector("_offset", new Vector4(current.x + speed.x * scrollSpeed, current.y + speed.y * scrollSpeed, 0, 0));
        transform.position = (Vector2)mainCam.transform.position;
        previousPos = transform.position;
    }
}

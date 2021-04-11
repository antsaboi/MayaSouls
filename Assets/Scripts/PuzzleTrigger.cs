using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PuzzleTrigger : MonoBehaviour
{
    public Transform button;
    public Transform door;
    public MouseDragTarget puzzleTile;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<MouseDragTarget>() is MouseDragTarget m)
        {
            m.isActive = false;
            button.DOLocalMoveY(0.219f, 1f);
            door.DOLocalMoveY(3, 2f);
            CameraController.instance.ShakeCamera(1f, 2f);
        }
    }
}

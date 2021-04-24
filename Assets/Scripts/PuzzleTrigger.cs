using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PuzzleTrigger : MonoBehaviour
{
    public Transform button;
    public Transform door;
    public MouseDragTarget puzzleTile;

    Material doorGlow;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<MouseDragTarget>() is MouseDragTarget m)
        {
            m.isActive = false;
            button.DOLocalMoveY(0.219f, 1f);
            StartCoroutine(DoorGlow());
        }
    }

    IEnumerator DoorGlow()
    {
        doorGlow = door.GetChild(0).GetComponent<SpriteRenderer>().material;
        float glowAmount = 1;

        while (true)
        {
            glowAmount = Mathf.MoveTowards(glowAmount, 0, Time.deltaTime * 0.5f);
            doorGlow.SetFloat("_Dissolve", glowAmount);
            if (glowAmount < 0.01f) break;
            yield return null;
        }

        door.DOLocalMoveY(3, 2f);
        CameraController.instance.ShakeCamera(1f, 2f);
    }
}

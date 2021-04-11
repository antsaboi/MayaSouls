using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEventsCharacter : MonoBehaviour
{
    // Start is called before the first frame update
    ProtoPlayer2D controller;
    void Start()
    {
        controller = GetComponentInParent<ProtoPlayer2D>();
    }

    public void Die()
    {
        controller.DeathShakeCam();
    }

    public void HitStart()
    {
        controller.AttackStart();
    }

    public void HitEnd()
    {
        controller.AttackEnd();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEventsEnemy : MonoBehaviour
{
    EnemyController controller;

    void Start()
    {
        controller = GetComponentInParent<EnemyController>();
    }

    public void HitStart()
    {
        controller.HitStart();
    }

    public void HitEnd()
    {
        controller.HitEnd();
    }

    public void DamageStart()
    {
        controller.DamageStart();
    }

    public void DamageEnd()
    {
        controller.DamageEnd();
    }
}

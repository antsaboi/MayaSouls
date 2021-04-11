using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(TargetJoint2D))]
public class MouseDragTarget : MonoBehaviour
{
    private Rigidbody2D body;
    private TargetJoint2D joint;
    Animator anim;
    public bool isActive = true;
    private void Start()
    {
        body = GetComponent<Rigidbody2D>();
        joint = GetComponent<TargetJoint2D>();
        joint.enabled = false;

        //This is a test area
        //anim = GetComponentInChildren<Animator>();
    }

    public void Attach(Color color, Vector2 pos, float damping, float frequency)
    {
        if (!isActive)
        {
            Detach();
            return;
        }

        if (pos != Vector2.zero) joint.anchor = transform.InverseTransformPoint(pos);
        else joint.anchor = Vector2.zero;
        joint.dampingRatio = damping;
        joint.enabled = true;
        joint.frequency = frequency;

        // spriteRenderer.color = color;
        //anim.SetBool("Glowing", true);
    }

    public void Move(Transform target)
    {
        if (!isActive)
        {
            Detach();
            return;
        }

        joint.target = target.position;
    }

    public void Detach()
    {
        joint.enabled = false;
        //anim.SetBool("Glowing", false);
    }
}

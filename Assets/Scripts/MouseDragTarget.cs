using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(TargetJoint2D))]
public class MouseDragTarget : MonoBehaviour
{
    private Rigidbody2D body;
    private SpriteRenderer spriteRenderer;
    private TargetJoint2D joint;

    private void Start()
    {
        body = GetComponent<Rigidbody2D>();
        joint = GetComponent<TargetJoint2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        joint.enabled = false;
    }

    public void Attach(Color color, Vector2 pos, float damping, float frequency)
    {
        if (pos != Vector2.zero) joint.anchor = transform.InverseTransformPoint(pos);
        else joint.anchor = Vector2.zero;
        joint.dampingRatio = damping;
        joint.frequency = frequency;
        joint.enabled = true;
        spriteRenderer.color = color;
    }

    public void Move(Transform target)
    {
        joint.target = target.position;
    }

    public void Detach()
    {
        joint.enabled = false;
        spriteRenderer.color = Color.white;
    }
}

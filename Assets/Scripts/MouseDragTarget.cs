using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(TargetJoint2D))]
public class MouseDragTarget : MonoBehaviour
{
    [SerializeField] SpriteRenderer glowEffect;
    [SerializeField] AudioClip collisionAudio;
    private Material dissolveMat;
    private Rigidbody2D body;
    private TargetJoint2D joint;
    public bool isActive = true;
    bool isGlowing;
    float dissolveAmount;

    private void Start()
    {
        body = GetComponent<Rigidbody2D>();
        joint = GetComponent<TargetJoint2D>();
        joint.enabled = false;
        dissolveMat = glowEffect.material;

        //This is a test area
        //anim = GetComponentInChildren<Animator>();
    }

    public void Attach(Color color, Vector2 pos, float damping, float frequency)
    {
        isGlowing = true;

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

    private void Update()
    {
        if (isGlowing)
        {
            if (dissolveAmount > 0)
            {
                dissolveAmount = Mathf.MoveTowards(dissolveAmount, 0, Time.deltaTime * 0.6f);
                dissolveMat.SetFloat("_Dissolve", dissolveAmount);
            }
        }
        else {
            if (dissolveAmount < 1)
            {
                dissolveAmount = Mathf.MoveTowards(dissolveAmount, 1, Time.deltaTime);
                dissolveMat.SetFloat("_Dissolve", dissolveAmount);
            }
        }
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
        isGlowing = false;
        joint.enabled = false;
        //anim.SetBool("Glowing", false);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(Time.time > 2f) AudioSystem.instance.PlayOneShot(collisionAudio, 0.5f);
    }
}

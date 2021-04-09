using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class MouseDragUser : MonoBehaviour
{
    public enum AttachType
    {
        mousePoint,
        pivot
    }
    [Header("Data")]
    [SerializeField] PlayerStats stats;

    [Header("Settings")]
    [SerializeField] AttachType attachType;
    [SerializeField] float damping = 0.5f;
    [SerializeField] float frequency = 1f;
    [SerializeField] Color draggingColor;

    private Transform dragTarget;
    private MouseDragTarget currentDragTarget;
    private Dictionary<GameObject, MouseDragTarget> cachedTargets = new Dictionary<GameObject, MouseDragTarget>();
    bool dragging = false;
    float reduceHPTimer;

    // Start is called before the first frame update
    void Start()
    {
        dragTarget = new GameObject("DragTarget").transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (!GameManager.instance.isAlive) return;
        HandleInput();
    }

    void HandleInput()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);

            if (hit.collider != null)
            {
                if (cachedTargets.ContainsKey(hit.collider.gameObject))
                {
                    //Use the cached one or do nothing if is already the selected

                    if (!ReferenceEquals(currentDragTarget, null))
                        if (cachedTargets[hit.transform.gameObject] == currentDragTarget) return;

                    currentDragTarget = cachedTargets[hit.transform.gameObject];
                }
                else
                {
                    //Perform an optimizen null ref check for mousedrag
                    if (ReferenceEquals(hit.transform.GetComponent<MouseDragTarget>(), null))
                        return;
                    else
                    {
                        //Add to dict and set as used
                        cachedTargets.Add(hit.transform.gameObject, hit.transform.GetComponent<MouseDragTarget>());
                        currentDragTarget = cachedTargets[hit.collider.gameObject];
                    }
                }
                AttachToMouse(mousePos);
            }
            else
            {
                DetachTarget();
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            if (dragging)
            {
                DetachTarget();
                return;
            }
        }

        if (!ReferenceEquals(currentDragTarget, null))
        {
            MoveTargetWithMouse(dragTarget, mousePos);
        }
    }

    void AttachToMouse(Vector2 position)
    {
        dragging = true;

        switch (attachType)
        {
            case AttachType.mousePoint:
                currentDragTarget.Attach(draggingColor, position, damping, frequency);
                break;
            case AttachType.pivot:
                currentDragTarget.Attach(draggingColor, Vector2.zero, damping, frequency);
                break;
            default:
                break;
        }
    }

    void DetachTarget()
    {
        if (ReferenceEquals(currentDragTarget, null)) return;

        dragging = false;
        currentDragTarget.Detach();
        currentDragTarget = null;
    }

    void MoveTargetWithMouse(Transform target, Vector2 mousePos)
    {
        if (Time.time > reduceHPTimer)
        {
            reduceHPTimer = Time.time + stats.hpReduceInterval;
            stats.ReduceHPByPowerUse(true);
        }
        target.transform.position = mousePos;
        currentDragTarget.Move(target);
    }
}

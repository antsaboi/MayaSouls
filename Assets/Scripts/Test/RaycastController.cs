using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class RaycastController : MonoBehaviour
{
    protected float _horizontalRaySpacing, _verticalRaySpacing;
    protected RaycastOrigins raycastOrigins;
    protected Collider2D _collider;
    public int horizontalRayCount = 4, verticalRayCount = 4;
    public LayerMask collisionMask;
    protected const float skinWidth = 0.015f;

    public virtual void Start()
    {
        _collider = GetComponent<Collider2D>();
        CalculateRaySpacing();
    }

    public struct RaycastOrigins
    {
        public Vector2 topLeft, topRight;
        public Vector2 bottomLeft, bottomRight;
    }

    protected void CalculateRaySpacing()
    {
        Bounds bounds = _collider.bounds;
        bounds.Expand(skinWidth * -2);

        horizontalRayCount = Mathf.Clamp(horizontalRayCount, 2, int.MaxValue);
        verticalRayCount = Mathf.Clamp(verticalRayCount, 2, int.MaxValue);

        _horizontalRaySpacing = bounds.size.y / (horizontalRayCount - 1);
        _verticalRaySpacing = bounds.size.y / (verticalRayCount - 1);
    }

    protected void UpdateRaycastOrigins()
    {
        Bounds bounds = _collider.bounds;
        bounds.Expand(skinWidth * -2);
        raycastOrigins.bottomLeft = new Vector2(bounds.min.x, bounds.min.y);
        raycastOrigins.bottomRight = new Vector2(bounds.max.x, bounds.min.y);
        raycastOrigins.topLeft = new Vector2(bounds.min.x, bounds.max.y);
        raycastOrigins.topRight = new Vector2(bounds.max.x, bounds.max.y);
    }
}

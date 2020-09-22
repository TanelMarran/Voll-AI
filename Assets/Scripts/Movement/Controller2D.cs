using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof (BoxCollider2D))]
public class Controller2D : MonoBehaviour
{
    private const float SkinWidth = .015f;
    public int horizontalRayCount = 4;
    public int verticalRayCount = 4;

    private RayCastOrigins _rayCastOrigins;
    
    private BoxCollider2D _collider;
    private float _horizontalRaySpacing;
    private float _verticalRaySpacing;

    private void Start()
    {
        _collider = GetComponent<BoxCollider2D>();
    }

    private void Update()
    {
        UpdateRaycastOrigins();
        CalculateRaySpacing();

        for (var i = 0; i < verticalRayCount; i++)
        {
            Debug.DrawRay(_rayCastOrigins.BottomLeft + Vector2.right * (i * _verticalRaySpacing), Vector3.down * 2, Color.green);
        }
    }

    private void UpdateRaycastOrigins()
    {
        Bounds bounds = _collider.bounds;
        bounds.Expand(SkinWidth * -2);
        
        _rayCastOrigins.BottomLeft = new Vector2(bounds.min.x, bounds.min.y);
        _rayCastOrigins.BottomRight = new Vector2(bounds.max.x, bounds.min.y);
        _rayCastOrigins.TopLeft = new Vector2(bounds.min.x, bounds.max.y);
        _rayCastOrigins.TopRight = new Vector2(bounds.max.x, bounds.max.y);
    }

    private void CalculateRaySpacing()
    {
        Bounds bounds = _collider.bounds;
        bounds.Expand(SkinWidth * -2);

        horizontalRayCount = Mathf.Clamp(horizontalRayCount, 2, int.MaxValue);
        verticalRayCount = Mathf.Clamp(verticalRayCount, 2, int.MaxValue);
        
        _horizontalRaySpacing = bounds.size.y / (horizontalRayCount - 1);
        _verticalRaySpacing = bounds.size.y / (verticalRayCount - 1);
        
    }

    private struct RayCastOrigins
    {
        public Vector2 TopLeft, TopRight;
        public Vector2 BottomLeft, BottomRight;
    }
}

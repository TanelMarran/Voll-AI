using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

[RequireComponent (typeof (BoxCollider2D))]
public class Controller2D : MonoBehaviour
{
    public LayerMask CollisionMask;
    
    public const float SkinWidth = .015f;
    public int horizontalRayCount = 4;
    public int verticalRayCount = 4;
    public CollisionInfo Collisions;

    private RayCastOrigins _rayCastOrigins;
    
    private BoxCollider2D _collider;
    private float _horizontalRaySpacing;
    private float _verticalRaySpacing;

    private void Start()
    {
        _collider = GetComponent<BoxCollider2D>();
        CalculateRaySpacing();
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
    
    void HorizontalCollisions(ref Vector3 velocity)
    {
        float directionX = Mathf.Sign(velocity.x);
        float rayLength = Mathf.Abs(velocity.x) + SkinWidth;
        
        for (int i = 0; i < horizontalRayCount; i++)
        {
            Vector2 rayOrigin = (directionX == -1 ? _rayCastOrigins.BottomLeft : _rayCastOrigins.BottomRight);
            rayOrigin += Vector2.up * (i * _horizontalRaySpacing);
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.right * directionX, rayLength, CollisionMask);

            if (hit)
            {
                velocity.x = (hit.distance - SkinWidth) * directionX;
                rayLength = hit.distance;
                Collisions.left = directionX == -1;
                Collisions.right = directionX == 1;
            }
        }
    }

    void VerticalCollisions(ref Vector3 velocity)
    {
        float directionY = Mathf.Sign(velocity.y);
        float rayLength = Mathf.Abs(velocity.y) + SkinWidth;
        
        for (int i = 0; i < verticalRayCount; i++)
        {
            Vector2 rayOrigin = (directionY == -1 ? _rayCastOrigins.BottomLeft : _rayCastOrigins.TopLeft);
            rayOrigin += Vector2.right * (i * _verticalRaySpacing + velocity.x);
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.up * directionY, rayLength, CollisionMask);

            if (hit)
            {
                velocity.y = (hit.distance - SkinWidth) * directionY;
                rayLength = hit.distance;
                Collisions.below = directionY == -1;
                Collisions.above = directionY == 1;
            }
        }
    }

    public bool hasCollisionBelow()
    {
        Vector2 velocity = Vector2.down * SkinWidth * 2;
        
        float directionY = Mathf.Sign(velocity.y);
        float rayLength = Mathf.Abs(velocity.y) + SkinWidth;
        
        for (int i = 0; i < verticalRayCount; i++)
        {
            Vector2 rayOrigin = (directionY == -1 ? _rayCastOrigins.BottomLeft : _rayCastOrigins.TopLeft);
            rayOrigin += Vector2.right * (i * _verticalRaySpacing + velocity.x);
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.up * directionY, rayLength, CollisionMask);

            if (hit)
            {
                return true;
            }
        }

        return false;
    }

    public void Move(Vector3 velocity)
    {
        Collisions.Reset();
        UpdateRaycastOrigins();

        if (velocity.x != 0)
        {
            HorizontalCollisions(ref velocity);
        }

        if (velocity.y != 0)
        {
            VerticalCollisions(ref velocity);
        }

        transform.Translate(velocity);
    }

    [Serializable] public struct CollisionInfo
    {
        public bool above, below;
        public bool left, right;

        public void Reset()
        {
            above = below = false;
            left = right = false;
        }
    }
}

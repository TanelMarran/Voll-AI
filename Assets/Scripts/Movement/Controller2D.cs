using System;
using UnityEngine;
using Vector2 = UnityEngine.Vector2;

[RequireComponent (typeof (BoxCollider2D))]
public class Controller2D : MonoBehaviour
{
    public LayerMask collisionMask;
    
    public const float SkinWidth = .015f;
    public int horizontalRayCount = 4;
    public int verticalRayCount = 4;
    public CollisionInfo collisions;

    private RayCastOrigins _rayCastOrigins;
    
    public new BoxCollider2D collider;
    private float _horizontalRaySpacing;
    private float _verticalRaySpacing;

    private void Start()
    {
        collider = GetComponent<BoxCollider2D>();
        CalculateRaySpacing();
    }

    private void UpdateRaycastOrigins()
    {
        Bounds bounds = collider.bounds;
        bounds.Expand(SkinWidth * -2);
        
        _rayCastOrigins.BottomLeft = new Vector2(bounds.min.x, bounds.min.y);
        _rayCastOrigins.BottomRight = new Vector2(bounds.max.x, bounds.min.y);
        _rayCastOrigins.TopLeft = new Vector2(bounds.min.x, bounds.max.y);
        _rayCastOrigins.TopRight = new Vector2(bounds.max.x, bounds.max.y);
    }

    private void CalculateRaySpacing()
    {
        Bounds bounds = collider.bounds;
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
    
    void HorizontalCollisions(ref Vector2 velocity)
    {
        float directionX = Mathf.Sign(velocity.x);
        float rayLength = Mathf.Abs(velocity.x) + SkinWidth;
        
        for (int i = 0; i < horizontalRayCount; i++)
        {
            Vector2 rayOrigin = (directionX == -1 ? _rayCastOrigins.BottomLeft : _rayCastOrigins.BottomRight);
            rayOrigin += Vector2.up * (i * _horizontalRaySpacing);
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.right * directionX, rayLength, collisionMask);

            if (hit)
            {
                velocity.x = (hit.distance - SkinWidth) * directionX;
                rayLength = hit.distance;
                collisions.left = directionX == -1;
                collisions.right = directionX == 1;
            }
        }
    }

    void VerticalCollisions(ref Vector2 velocity)
    {
        float directionY = Mathf.Sign(velocity.y);
        float rayLength = Mathf.Abs(velocity.y) + SkinWidth;
        
        for (int i = 0; i < verticalRayCount; i++)
        {
            Vector2 rayOrigin = (directionY == -1 ? _rayCastOrigins.BottomLeft : _rayCastOrigins.TopLeft);
            rayOrigin += Vector2.right * (i * _verticalRaySpacing + velocity.x);
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.up * directionY, rayLength, collisionMask);

            if (hit)
            {
                velocity.y = (hit.distance - SkinWidth) * directionY;
                rayLength = hit.distance;
                collisions.below = directionY == -1;
                collisions.above = directionY == 1;
            }
        }
    }

    public bool hasCollisionBelow()
    {
        Vector2 velocity = Vector2.down * (SkinWidth * 2);
        
        float directionY = Mathf.Sign(velocity.y);
        float rayLength = Mathf.Abs(velocity.y) + SkinWidth;
        
        for (int i = 0; i < verticalRayCount; i++)
        {
            Vector2 rayOrigin = (directionY == -1 ? _rayCastOrigins.BottomLeft : _rayCastOrigins.TopLeft);
            rayOrigin += Vector2.right * (i * _verticalRaySpacing + velocity.x);
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.up * directionY, rayLength, collisionMask);

            if (hit)
            {
                return true;
            }
        }

        return false;
    }

    public void Move(Vector2 velocity)
    {
        collisions.Reset();
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

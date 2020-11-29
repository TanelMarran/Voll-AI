using System;
using Movement;
using UnityEngine;

internal class BallDefault : State<Ball>
{
    public bool isServing;
    public BallDefault(Ball handler) : base(handler)
    {
    }

    public override void Start()
    {
        isServing = false;
    }

    public override void FixedUpdate()
    {
        float deltaTime = Handler.Game.GameDelta;
        float time = Handler.Game.GameTime;

        if (!isServing)
        {
            Handler.velocity.current.y += -Game.gravityAmount * deltaTime;
        }
        Handler.velocity.current.y = Mathf.Max(-Game.maxFallSpeed, Handler.velocity.current.y);

        Handler.velocity.resting.y = 0;
        if (Handler.controller2D.collisions.below || Handler.controller2D.collisions.above)
        {
            Handler.velocity.current.y *= -.7f;
        }        
        
        if (Handler.controller2D.collisions.left || Handler.controller2D.collisions.right)
        {
            Handler.velocity.current.x *= -.7f;
        }

        Collider2D collision;
        Collider2D player1 = Handler.Game.Player1._ballCollider;
        Collider2D player2 = Handler.Game.Player2._ballCollider;
        collision = Handler.controller2D.collider.IsTouching(player1) ? player1 : null;
        collision = Handler.controller2D.collider.IsTouching(player2) ? player2 : collision;
        if (collision)
        {
            {
                Player player = collision.GetComponent<Player>();

                Vector2 collisionDist = Handler.transform.position - player.transform.position;
                Vector2 collisionNormal = collisionDist.normalized;
                Vector2 playerMovement = player.velocity.current + collisionNormal;
                Vector2 ballMovement = Handler.velocity.current;
                
                float angle = (Vector2.SignedAngle(Vector2.right, collisionNormal) + 90f) * Mathf.Deg2Rad;
                Vector2 crossNormal = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
                Vector2 reflectedMovement = Vector2.Reflect(-ballMovement, crossNormal) * .5f;

                float similarity = Mathf.Max(0,Vector2.Dot(playerMovement.normalized, collisionNormal)) * .7f;

                if (!Handler.isInsidePlayer)
                {
                    Handler.velocity.current = playerMovement * similarity + reflectedMovement;
                }
                
                Handler.ContactCooldownTimestamp = time + Handler.ContactCooldown;
                Handler.isInsidePlayer = true;
                Handler.controller2D.Move(collisionNormal * (.3f * (1.2f - collisionDist.magnitude)));
                isServing = false;
                //Handler.Game.applyHitstop(0.12f * Mathf.Sqrt(playerSpeed) / 8f);
            }
        }
        else
        {
            Handler.isInsidePlayer = false;
        }

        Collider2D floor = Handler.controller2D.CollisionBelow();

        if (floor)
        {
            if (floor.CompareTag("Floor"))
            {
                Handler.Game.ScorePoint();
            }
        }
        
        Collider2D net = Handler.controller2D.CollisionBelow();

        if (net)
        {
            if (net.CompareTag("Net"))
            {
                Handler.velocity.current += new Vector2(Handler.transform.position.x * .5f, .5f);
            }
        }

        if (!isServing)
        {
            Handler.velocity.Lerp(Game.airRestitution * deltaTime);
        }
        Handler.controller2D.Move(Handler.velocity.current * deltaTime);
    }
}
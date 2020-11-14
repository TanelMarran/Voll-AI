using System;
using Movement;
using UnityEngine;

internal class BallDefault : State<Ball>
{
    public BallDefault(Ball handler) : base(handler)
    {
    }

    public override void FixedUpdate()
    {
        float deltaTime = Handler.Game.GameDelta;
        float time = Handler.Game.GameTime;
        
        Handler.velocity.current.y += -Game.gravityAmount * deltaTime;
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

        Collider2D[] collisions = new Collider2D[2];
        int hit = Handler.controller2D.collider.OverlapCollider(Handler.playerContactFilter, collisions);
        if (hit > 0)
        {
            if (!Handler.isInsidePlayer)
            {
                Handler.isInsidePlayer = true;
                Player player = collisions[0].GetComponent<Player>();
                float playerSpeed = player.velocity.current.magnitude;
                Vector2 playerVelocityDirection = player.velocity.current.normalized;
                Vector2 collisionDirection = (Handler.transform.position - player.transform.position).normalized;
            
                Handler.velocity.current = (playerVelocityDirection + collisionDirection).normalized * playerSpeed;
                Handler.ContactCooldownTimestamp = time + Handler.ContactCooldown;
                Handler.Game.applyHitstop(0.12f * Mathf.Sqrt(playerSpeed) / 8f);
            }
        }
        else
        {
            Handler.isInsidePlayer = false;
        }
        
        Handler.velocity.Lerp(Game.airRestitution * deltaTime);
        Handler.controller2D.Move(Handler.velocity.current * deltaTime);
    }
}
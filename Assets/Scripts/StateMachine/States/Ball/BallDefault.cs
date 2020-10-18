using Movement;
using UnityEngine;

internal class BallDefault : State<Ball>
{
    public BallDefault(Ball handler) : base(handler)
    {
    }

    public override void FixedUpdate()
    {
        Handler.velocity.resting.y = Handler.velocity.current.y += Handler.gravity * Time.deltaTime;

        if (Handler.controller2D.collisions.below)
        {
            Handler.velocity.resting.x = Handler.velocity.current.x = 0;
        }
        
        Handler.velocity.Lerp(6f * Time.deltaTime);
        Handler.controller2D.Move(Handler.velocity.current * Time.deltaTime);
    }
}
using Movement;
using UnityEngine;
using UnityEngine.XR;

internal class BallDefault : State<Ball>
{
    public BallDefault(Ball handler) : base(handler)
    {
    }

    public override void FixedUpdate()
    {
        Handler.velocity.y += Handler.gravity * Time.deltaTime;
        
        Handler.controller2D.Move(Handler.velocity * Time.deltaTime);
    }
}
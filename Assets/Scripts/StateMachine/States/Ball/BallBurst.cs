using Movement;
using UnityEngine;

internal class BallBurst : State<Ball>
{
    private static float BurstTime = .1f;
    private static float _burstEndTime;
    
    
    public BallBurst(Ball handler) : base(handler)
    {
    }

    public override void Start()
    {
        _burstEndTime = Time.time + BurstTime;
    }

    public override void FixedUpdate()
    {
        Handler.velocity.resting = Handler.velocity.current;
        
        Handler.controller2D.Move(Handler.velocity.current * Time.deltaTime);

        if (_burstEndTime < Time.time)
        {
            Handler.velocity.current *= 0.7f;
            Handler.State.SetState(Handler.HangState);
        }
    }
}
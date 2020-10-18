using Movement;
using UnityEngine;

internal class BallHang : State<Ball>
{
    private static float HangTime = 1f;
    private float _hangEndTime;
    
    
    public BallHang(Ball handler) : base(handler)
    {
    }

    public override void Start()
    {
        _hangEndTime = Time.time + HangTime;
    }

    public override void FixedUpdate()
    {
        float stateProgress = Mathf.Min(1, 1 - (_hangEndTime - Time.time) / HangTime);
        float gravityMotifier = Mathf.Cos(Mathf.Deg2Rad * stateProgress * 180f) * 0.5f + 0.5f;
        Vector2 scaleVector = new Vector2(1, gravityMotifier);
        
        Handler.velocity.resting = Handler.velocity.current;
        Handler.controller2D.Move(Vector2.Scale(Handler.velocity.current, scaleVector) * Time.deltaTime);

        if (_hangEndTime < Time.time)
        {
            Handler.velocity.current.Scale(scaleVector);
            Handler.State.SetState(Handler.DefaultState);
        }
    }
}
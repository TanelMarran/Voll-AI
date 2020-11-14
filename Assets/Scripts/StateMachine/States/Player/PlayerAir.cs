using Movement;
using UnityEngine;
using UnityEngine.XR;

public class PlayerAir : State<Player>
{
    private bool _dashed;
    private bool _hit;
    public PlayerAir(Player handler) : base(handler)
    {
        Handler.Actions.Player.Jump.canceled += ctx => JumpCanceled();
    }

    private void JumpCanceled()
    {
        if (Handler.State._state == this && Handler.isJumping)
        {
            Handler.isJumping = false;
            Handler.velocity.current.y *= 0.5f;
        }
    }
    
    private void ResetVariables()
    {
        _dashed = false;
        _hit = false;
    }
    
    public override void Start()
    {
        ResetVariables();
    }

    public override void Update()
    {
        _dashed = Handler.Actions.Player.Dash.triggered || _dashed;
        _hit = Handler.Actions.Player.Hit.triggered || _hit;
    }

    public override void FixedUpdate()
    {
        float deltaTime = Handler.Game.GameDelta;

        if (Handler.velocity.current.y < 0)
        {
            Handler.isJumping = false;
        }
        
        Handler.velocity.resting.x = Handler.Actions.Player.Movement.ReadValue<Vector2>().x * Handler.movementSpeed;
        Handler.velocity.resting.y = Handler.velocity.current.y += Handler.gravity * deltaTime;
        
        Handler.HitBehaviour(_hit);
        
        Handler.velocity.Lerp(40f * deltaTime);
        Handler.controller2D.Move(Handler.velocity.current * deltaTime);
        
        Handler.velocity.current.y = Handler.controller2D.collisions.above || Handler.controller2D.collisions.below ? 0 : Handler.velocity.current.y;

        if (Handler.controller2D.collisions.below)
        {
            Handler.State.SetState(Handler.PlayerGround);
        }
        
        if (_dashed)
        {
            Handler.State.SetState(Handler.PlayerDash);
        }
        
        ResetVariables();
    }
}
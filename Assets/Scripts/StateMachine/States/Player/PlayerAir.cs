using Movement;
using UnityEngine;
using UnityEngine.XR;

public class PlayerAir : State<Player>
{
    private bool _dashed;
    public PlayerAir(Player handler) : base(handler)
    {
    }

    private void ResetVariables()
    {
        _dashed = false;
    }
    
    public override void Start()
    {
        ResetVariables();
        if (Handler.isJumping && Handler.inputManager.JumpReleased())
        {
            Handler.isJumping = false;
            Handler.velocity.current.y *= 0.5f;
        }
    }

    public override void Update()
    {
        _dashed = Handler.Actions.Player.Dash.triggered || _dashed;
    }

    public override void FixedUpdate()
    {
        float deltaTime = Handler.Game.GameDelta;

        if (Handler.velocity.current.y < 0)
        {
            Handler.isJumping = false;
        }
        
        if (Handler.isJumping && Handler.inputManager.JumpReleased())
        {
            Handler.isJumping = false;
            Handler.velocity.current.y *= 0.5f;
        }
        
        Handler.velocity.resting.x = Handler.inputManager.Movement().x * Handler.movementSpeed;
        Handler.velocity.resting.y = Handler.velocity.current.y += Handler.gravity * deltaTime;

        Handler.velocity.Lerp(40f * deltaTime);
        Handler.controller2D.Move(Handler.velocity.current * deltaTime);
        
        Handler.velocity.current.y = Handler.controller2D.collisions.above || Handler.controller2D.collisions.below ? 0 : Handler.velocity.current.y;

        if (Handler.controller2D.collisions.below)
        {
            Handler.State.SetState(Handler.PlayerGround);
        }
        
        if (Handler.inputManager.DashPressed())
        {
            Handler.State.SetState(Handler.PlayerDash);
        }
        
        ResetVariables();
    }
}
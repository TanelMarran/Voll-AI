using Movement;
using UnityEngine;
using UnityEngine.XR;

public class PlayerAir : State<Player>
{
    public PlayerAir(Player handler, int index) : base(handler, index)
    {
    }

    public override void Start()
    {
        if (Handler.isJumping && Handler.inputs.JumpReleased())
        {
            Handler.isJumping = false;
            Handler.velocity.current.y *= 0.5f;
        }
    }

    public override void Update()
    {
    }

    public override void FixedUpdate()
    {
        float deltaTime = Handler.game.GameDelta;

        if (Handler.velocity.current.y < 0)
        {
            Handler.isJumping = false;
        }
        
        if (Handler.isJumping && Handler.inputs.JumpReleased())
        {
            Handler.isJumping = false;
            Handler.velocity.current.y *= 0.5f;
        }
        
        Handler.velocity.resting.x = Handler.inputs.Movement().x * Handler.movementSpeed;
        Handler.velocity.resting.y = Handler.velocity.current.y = Mathf.Max(-Game.maxFallSpeed, Handler.velocity.current.y - Game.gravityAmount * deltaTime);

        if (Handler.velocity.resting.x != 0)
        {
            Handler.velocity.Lerp(20f * deltaTime);
        }
        Handler.velocity.applyRestitution(Game.airRestitution * deltaTime);
        Handler.controller2D.Move(Handler.velocity.current * deltaTime);
        
        Handler.velocity.current.y = Handler.controller2D.collisions.above || Handler.controller2D.collisions.below ? 0 : Handler.velocity.current.y;

        if (Handler.controller2D.collisions.below)
        {
            Handler.State.SetState(Handler.PlayerGround);
        }
        
        if (Handler.inputs.JumpPressed())
        {
            Handler.State.SetState(Handler.PlayerDash);
        }
        
        Handler.HitBehaviour();
    }
}
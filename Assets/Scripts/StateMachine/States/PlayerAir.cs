using Movement;
using UnityEngine;

public class PlayerAir : State<Player>
{
    private bool _dashed = false;
    public PlayerAir(Player handler) : base(handler)
    {
    }
    
    public override void Start()
    {
        _dashed = false;
    }

    public override void Update()
    {
        _dashed = Handler.actions.Player.Dash.triggered || _dashed;
    }

    public override void FixedUpdate()
    {
        Handler.velocity.x = Handler.actions.Player.Movement.ReadValue<Vector2>().x * Handler.movementSpeed;
        Handler.velocity.y += Handler.gravity * Time.deltaTime;
        
        Handler.controller2D.Move(Handler.velocity * Time.deltaTime);
        
        Handler.velocity.y = Handler.controller2D.Collisions.above || Handler.controller2D.Collisions.below ? 0 : Handler.velocity.y;

        if (Handler.controller2D.Collisions.below)
        {
            Handler.State.SetState(Handler.PlayerGround);
        }
        
        if (_dashed)
        {
            Handler.State.SetState(Handler.PlayerDash);
        }
    }
}
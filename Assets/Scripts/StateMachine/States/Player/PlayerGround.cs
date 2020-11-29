using UnityEngine;

namespace Movement
{
    public class PlayerGround : State<Player>
    {
        private bool _jumped;
        private bool _dashed;
        private bool _hit;
        
        public PlayerGround(Player handler) : base(handler)
        {
        }

        public override void Start()
        {
            ResetVariables();
            Handler.velocity.resting.y = 0;
        }

        private void ResetVariables()
        {
            _jumped = false; 
            _dashed = false;
            _hit = false;
        }

        public override void Update()
        {
            _jumped = Handler.Actions.Player.Jump.triggered || _jumped;
            _dashed = Handler.Actions.Player.Dash.triggered || _dashed;
        }

        public override void FixedUpdate()
        {
            float deltaTime = Handler.Game.GameDelta;

            if (Handler.inputManager.JumpPressed())
            {
                Handler.velocity.resting.y = Handler.velocity.current.y = Handler.jumpPower;
                Handler.isJumping = true;
                _jumped = false;
            }

            Handler.HitBehaviour(_hit);

            Handler.velocity.resting.x = Handler.inputManager.Movement().x * Handler.movementSpeed;

            Handler.velocity.Lerp(40f * deltaTime);
            Handler.controller2D.Move(Handler.velocity.current * deltaTime);

            if (!Handler.controller2D.hasCollisionBelow())
            {
                Handler.State.SetState(Handler.PlayerAir);
            }
            
            if (Handler.inputManager.DashPressed())
            {
                Handler.State.SetState(Handler.PlayerDash);
            }

            ResetVariables();
        }
    }
}
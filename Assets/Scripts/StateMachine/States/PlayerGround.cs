using System;
using Movement;
using UnityEngine;
using UnityEngine.XR;

namespace Movement
{
    public class PlayerGround : State<Player>
    {
        private bool _jumped = false;
        private bool _dashed = false;
        private bool _hit = false;
        
        public PlayerGround(Player handler) : base(handler)
        {
        }

        public override void Start()
        {
            ResetVariables();
        }

        private void ResetVariables()
        {
            _jumped = false; 
            _dashed = false;
            _hit = false;
        }

        public override void Update()
        {
            _jumped = Handler.actions.Player.Jump.triggered || _jumped;
            _dashed = Handler.actions.Player.Dash.triggered || _dashed;
            _hit = Handler.actions.Player.Hit.triggered || _hit;
        }

        public override void FixedUpdate()
        {
            if (_jumped)
            {
                Handler.velocity.y = Handler.jumpPower;
                _jumped = false;
            }

            if (_hit && Handler.IsBallInRange())
            {
                Ball ball = GameObject.FindWithTag("Ball").GetComponent<Ball>();
                
                ball.velocity = Vector2.up * 10;
            }

            Handler.velocity.x = Handler.actions.Player.Movement.ReadValue<Vector2>().x * Handler.movementSpeed;

            Handler.controller2D.Move(Handler.velocity * Time.deltaTime);

            if (!Handler.controller2D.hasCollisionBelow())
            {
                Handler.State.SetState(Handler.PlayerAir);
            }
            
            if (_dashed)
            {
                Handler.State.SetState(Handler.PlayerDash);
            }

            ResetVariables();
        }
    }
}
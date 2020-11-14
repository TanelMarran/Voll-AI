﻿using System;
using UnityEngine;

namespace Movement
{
    public class PlayerDash : State<Player>
    {
        private const float DashTime = 0.1f;
        private const float DashStrength = 20f;
        private Vector2 _dashDirection;
        private float _dashEndTime;
        
        public PlayerDash(Player handler) : base(handler)
        {
        }

        public override void Start()
        {
            _dashDirection = Handler.Actions.Player.Movement.ReadValue<Vector2>();
            float direction = Mathf.Round(Vector2.SignedAngle(Vector2.right, _dashDirection) / 45f) * 45f * Mathf.Deg2Rad;
            _dashDirection = new Vector2(Mathf.Cos(direction), Mathf.Sin(direction));
            _dashDirection.Normalize();
            _dashEndTime = Handler.Game.GameTime + DashTime;
            Handler.velocity.current = _dashDirection * DashStrength;
            Handler.velocity.resting = Vector2.zero;
        }

        public override void FixedUpdate()
        {
            float deltaTime = Handler.Game.GameDelta;
            float time = Handler.Game.GameTime;
            
            if (Handler.Actions.Player.Movement.ReadValue<Vector2>().magnitude != 0)
            {
                float inputDirection = Vector2.SignedAngle(Vector2.right, Handler.Actions.Player.Movement.ReadValue<Vector2>());
                float movementDirection = Vector2.SignedAngle(Vector2.right, Handler.velocity.current);
                float lerpDirection = Mathf.Deg2Rad * movementDirection;
                if (inputDirection - movementDirection != 0)
                {
                    lerpDirection = Mathf.Deg2Rad * Mathf.LerpAngle(movementDirection, inputDirection, (300f / Mathf.Abs(inputDirection - movementDirection)) * deltaTime); //120f / Mathf.DeltaAngle(movementDirection, inputDirection) * Time.deltaTime

                }
                float movementVelocity = Handler.velocity.current.magnitude;
                
                Handler.velocity.current.x = Mathf.Cos(lerpDirection) * movementVelocity;
                Handler.velocity.current.y = Mathf.Sin(lerpDirection) * movementVelocity;
            }
            
            Handler.velocity.Lerp(DashStrength * 3f * deltaTime);
            Handler.controller2D.Move(Handler.velocity.current * deltaTime);

            if (time > _dashEndTime || Handler.velocity.current.magnitude < 1f)
            {
                Handler.State.SetState(Handler.PlayerAir);
            }
        }
    }
}
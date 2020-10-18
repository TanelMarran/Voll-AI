using UnityEngine;

namespace Movement
{
    public class PlayerDash : State<Player>
    {
        private const float DashTime = .05f;
        private const float DashStrength = 53f;
        private float _dashDirection;
        private float _dashEndTime;
        
        public PlayerDash(Player handler) : base(handler)
        {
        }

        public override void Start()
        {
            _dashDirection = Mathf.Sign(Handler.Actions.Player.Movement.ReadValue<Vector2>().normalized.x);
            _dashEndTime = Time.time + DashTime;
            Handler.velocity.current.x = _dashDirection * DashStrength;
            Handler.velocity.resting.x = 0;
        }

        public override void FixedUpdate()
        {
            Handler.velocity.resting.y = Handler.velocity.current.y = 0;

            Handler.velocity.Lerp(DashStrength * 14f * Time.deltaTime);
            Handler.controller2D.Move(Handler.velocity.current * Time.deltaTime);

            if (Time.time > _dashEndTime)
            {
                Handler.State.SetState(Handler.PlayerAir);
            }
        }
    }
}
using UnityEngine;

namespace Movement
{
    public class PlayerDash : State<Player>
    {
        private const float DashTime = .2f;
        private float _dashDirection;
        private float _dashEndTime;
        
        public PlayerDash(Player handler) : base(handler)
        {
        }

        public override void Start()
        {
            _dashDirection = Mathf.Sign(Handler.actions.Player.Movement.ReadValue<Vector2>().normalized.x);
            _dashEndTime = Time.time + DashTime;
        }

        public override void FixedUpdate()
        {
            Handler.velocity.x = _dashDirection * 16f;
            Handler.velocity.y = 0;
            
            Handler.controller2D.Move(Handler.velocity * Time.deltaTime);

            if (Time.time > _dashEndTime)
            {
                Handler.State.SetState(Handler.PlayerAir);
            }
        }
    }
}
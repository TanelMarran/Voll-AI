using UnityEngine;
using UnityEngine.XR;

namespace Movement
{
    public class PlayerGround : State<Player>
    {
        private bool _hit;
        
        public PlayerGround(Player handler, int index) : base(handler, index)
        {
        }

        public override void Start()
        {
            Handler.velocity.resting.y = 0;
            Handler.currentDashes = Handler.totalDashes;
        }

        public override void Update()
        {
        }

        public override void FixedUpdate()
        {
            float deltaTime = Handler.game.GameDelta;

            if (Handler.inputs.JumpPressed())
            {
                Handler.velocity.resting.y = Handler.velocity.current.y = Handler.jumpPower;
                Handler.isJumping = true;
            }

            Handler.velocity.resting.x = Handler.inputs.Movement().x * Handler.movementSpeed;

            Handler.velocity.Lerp(40f * deltaTime);
            Handler.controller2D.Move(Handler.velocity.current * deltaTime);

            if (!Handler.controller2D.hasCollisionBelow())
            {
                Handler.State.SetState(Handler.PlayerAir);
            }
            
            Handler.HitBehaviour();
        }
    }
}
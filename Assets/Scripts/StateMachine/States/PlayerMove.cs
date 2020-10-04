using UnityEngine;
using UnityEngine.XR;

namespace Movement
{
    public class PlayerMove : State<Player>
    {
        public PlayerMove(Player handler) : base(handler)
        {
        }

        public override void Update()
        {
        }

        public override void FixedUpdate()
        {
            if (Handler.controller2D.Collisions.above || Handler.controller2D.Collisions.below)
            {
                Handler.velocity.y = 0;
            }
            
            Handler.velocity.x = Handler.inputController.movementInput.x * Handler.movementSpeed;

            if (Handler.inputController.jumpInput && Handler.controller2D.Collisions.below)
            {
                Handler.velocity.y = Handler.jumpPower;
            }

            Handler.velocity.y += Handler.gravity * Time.deltaTime;
            Handler.controller2D.Move(Handler.velocity * Time.deltaTime);
        }
    }
}
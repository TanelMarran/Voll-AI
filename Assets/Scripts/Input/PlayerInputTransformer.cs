using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Input
{
    public class PlayerInputTransformer : AbstractInputHandler
    {
        private Vector2 movement;
        private bool isJumpDown;
        private bool isHitDown;

        private bool isJumpPressedThisFrame;
        private bool isHitPressedThisFrame;
        
        private bool isJumpReleasedThisFrame;
        private bool isHitReleasedThisFrame;

        private void FixedUpdate()
        {
            isJumpPressedThisFrame = false;
            isHitPressedThisFrame = false;
            isJumpReleasedThisFrame = false;
            isHitReleasedThisFrame = false;
        }
        
        private void SetHitKey(bool val)
        {
            if (!isHitDown && val)
            {
                isHitPressedThisFrame = true;
            }
            if (isHitDown && !val)
            {
                isHitReleasedThisFrame = true;
            }
            isHitDown = val;
        }
        
        public void SetHitKey(float key)
        {
            SetHitKey(key > 0.5f);
        }
        
        public void SetHitKey(InputAction.CallbackContext ctx)
        {
            SetHitKey(ctx.ReadValue<float>() > 0.5);
        }

        private void SetJumpKey(bool val)
        {
            if (!isJumpDown && val)
            {
                isJumpPressedThisFrame = true;
            }
            if (isJumpDown && !val)
            {
                isJumpReleasedThisFrame = true;
            }
            isJumpDown = val;
        }
        
        public void SetJumpKey(float key)
        {
            SetJumpKey(key > 0.5f);
        }
        
        public void SetJumpKey(InputAction.CallbackContext ctx)
        {
            bool val = ctx.ReadValue<float>() > 0.5;
            SetJumpKey(val);
        }

        public void SetMovementKey(Vector2 key)
        {
            movement = key.magnitude < 0.1f ? Vector2.zero : key.normalized;
        }
        
        public void SetMovementKey(InputAction.CallbackContext ctx)
        {
            movement = ctx.ReadValue<Vector2>().normalized;
        }

        public override Vector2 Movement()
        {
            return movement;
        }

        public override bool JumpPressed()
        {
            return isJumpPressedThisFrame;
        }

        public override bool JumpReleased()
        {
            return isJumpReleasedThisFrame;
        }

        public override bool HitPressed()
        {
            return isHitPressedThisFrame;
        }

        public override bool HitReleased()
        {
            return isHitReleasedThisFrame;
        }
    }
}
using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Input
{
    public class PlayerInputManager : AbstractInputHandler
    {
        private Vector2 movement;
        private bool isJumpDown;
        private bool isDashDown;

        private bool isJumpPressedThisFrame;
        private bool isDashPressedThisFrame;
        
        private bool isJumpReleasedThisFrame;
        private bool isDashReleasedThisFrame;

        private void FixedUpdate()
        {
            isJumpPressedThisFrame = false;
            isDashPressedThisFrame = false;
            isJumpReleasedThisFrame = false;
            isDashReleasedThisFrame = false;
        }
        
        private void SetDashKey(bool val)
        {
            if (!isDashDown && val)
            {
                isDashPressedThisFrame = true;
            }
            if (isDashDown && !val)
            {
                isDashReleasedThisFrame = true;
            }
            isDashDown = val;
        }
        
        public void SetDashKey(float key)
        {
            SetDashKey(key > 0.5f);
        }
        
        public void SetDashKey(InputAction.CallbackContext ctx)
        {
            SetDashKey(ctx.ReadValue<float>() > 0.5);
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

        public override bool DashPressed()
        {
            return isDashPressedThisFrame;
        }

        public override bool DashReleased()
        {
            return isDashReleasedThisFrame;
        }
    }
}
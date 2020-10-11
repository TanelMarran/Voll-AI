using System;
using UnityEngine;
using UnityEngine.PlayerLoop;

namespace Input
{
    public class InputHandler : MonoBehaviour
    {
        public struct AnalogInput
        {
            public bool pressed;
            public bool down;

            public void setDown(bool isDown)
            {
                down = isDown;
            }

            public void Reset()
            {
                pressed = false;
            }
        }
        
        public Vector2 Axis;
        public AnalogInput Jump;

        private void Start()
        {
            Axis = new Vector2(0, 0);
            Jump.down = false;
        }

        private void Update()
        {
        }

        private void FixedUpdate()
        {
            Jump.Reset();
        }
    }
}
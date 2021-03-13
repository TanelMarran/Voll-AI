using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace Movement
{
    [Serializable] public class MovementVector
    {
        public Vector2 current;
        public Vector2 resting;
        
        public MovementVector(Vector2 resting)
        {
            this.resting = resting;
        }
        
        public void Lerp(float amount)
        {
            Vector2 toResting = resting - current;

            current += toResting.normalized * Mathf.Min(amount, toResting.magnitude);
        }

        public void applyRestitution(float amount)
        {
            Vector2 toResting = -current;

            current += toResting.normalized * Mathf.Min(amount, toResting.magnitude);
        }
    }
}
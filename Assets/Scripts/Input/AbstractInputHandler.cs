using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.PlayerLoop;

namespace Input
{
    public abstract class AbstractInputHandler : MonoBehaviour
    {
        public abstract Vector2 Movement();

        public abstract bool JumpPressed();
        public abstract bool JumpReleased();
        
        public abstract bool HitPressed();
        public abstract bool HitReleased();
    }
}
using System;
using System.Collections;
using System.Collections.Generic;
using Movement;
using UnityEngine;
using UnityEngine.Events;

public class PlayerEvent : UnityEvent<Ball.PlayerHit> {}

[RequireComponent(typeof(Controller2D))]
public class Ball : MonoBehaviour
{
    public Game game;
    public MovementVector velocity;
    [Range(0, 1)] public float AirRestitutionFactor = 0.5f;
    
    [HideInInspector] public Controller2D controller2D;

    [HideInInspector] public CircleCollider2D Collider2D;

    public PlayerEvent OnBallTouched;
    
    private bool _isActive = true;
    
    private bool movementPaused = false;

    public struct PlayerHit
    {
        public Player Player;
        public float Distance;
    }

    public bool MovementPaused
    {
        get => movementPaused;
        set => movementPaused = value;
    }

    public void Deactivate()
    {
        _isActive = false;
    }

    public void Activate()
    {
        _isActive = true;
    }

    public bool IsActive()
    {
        return _isActive;
    }

    private void Awake()
    {
        if (OnBallTouched == null)
        {
            OnBallTouched = new PlayerEvent();
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        Collider2D = GetComponent<CircleCollider2D>();
        controller2D = GetComponent<Controller2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!movementPaused)
        {
            float deltaTime = game.GameDelta;

            if (controller2D.collisions.below && IsActive())
            {
                if (transform.localPosition.y < 2) 
                {
                    if (transform.localPosition.x < 0)
                    {
                        game.AddRightPoint(1);
                    }
                    else
                    {
                        game.AddLeftPoint(1);
                    }

                    return;
                }
            
                var localPosition = transform.localPosition;
                velocity.current += new Vector2(localPosition.x * 10f, -localPosition.y);
            }

            if (controller2D.collisions.below || controller2D.collisions.above)
            {
                velocity.current.y *= -0.9f;
                controller2D.collisions.Reset();
            }

            if (controller2D.collisions.left || controller2D.collisions.right)
            {
                velocity.current.x *= -0.9f;
                controller2D.collisions.Reset();
            }

            velocity.resting.y = velocity.current.y = Mathf.Max(-Game.maxFallSpeed, velocity.current.y - Game.gravityAmount * deltaTime * AirRestitutionFactor);

            velocity.applyRestitution(Game.airRestitution * deltaTime * AirRestitutionFactor);
            controller2D.Move(velocity.current * deltaTime);
        } 
    }
}

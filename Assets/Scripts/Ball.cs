using System.Collections;
using System.Collections.Generic;
using Movement;
using UnityEngine;

[RequireComponent(typeof(Controller2D))]
public class Ball : MonoBehaviour
{
    public Game game;
    public MovementVector velocity;
    [Range(0, 1)] public float AirRestitutionFactor = 0.5f;
    
    [HideInInspector] public Controller2D controller2D;

    [HideInInspector] public CircleCollider2D Collider2D;
    
    // Start is called before the first frame update
    void Start()
    {
        Collider2D = GetComponent<CircleCollider2D>();
        controller2D = GetComponent<Controller2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float deltaTime = game.GameDelta;

        if (controller2D.collisions.below && transform.localPosition.y < 2)
        {
            if (transform.localPosition.x < 0)
            {
                game.rightPoint++;
                game.startNewRound(false);
            }
            else
            {
                game.leftPoint++;
                game.startNewRound(true);
            }
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

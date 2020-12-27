using System.Collections;
using System.Collections.Generic;
using Movement;
using UnityEngine;

[RequireComponent(typeof(Controller2D))]
public class Ball : MonoBehaviour
{
    public Game game;
    public MovementVector velocity;
    
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
        
        velocity.resting.y = velocity.current.y = Mathf.Max(-Game.maxFallSpeed, velocity.current.y - Game.gravityAmount * deltaTime);

        velocity.applyRestitution(Game.airRestitution * deltaTime);
        controller2D.Move(velocity.current * deltaTime);
    }
}

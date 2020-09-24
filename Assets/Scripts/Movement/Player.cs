using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Controller2D), typeof(InputController))]
public class Player : MonoBehaviour
{
    public float MovementSpeed = 5;
    
    private float gravity = -20;
    private Vector3 velocity;

    private Controller2D _controller2D;
    private InputController _inputController;

    private void Start()
    {
        _controller2D = GetComponent<Controller2D>();
        _inputController = GetComponent<InputController>();
    }

    private void FixedUpdate()
    {
        velocity.x = _inputController.movementInput.x * MovementSpeed;
        
        velocity.y += gravity * Time.deltaTime;
        _controller2D.Move(velocity * Time.deltaTime);
    }
}

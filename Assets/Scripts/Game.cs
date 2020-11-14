using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Random = UnityEngine.Random;

public class Game : MonoBehaviour
{
    public Player Player1;
    public Player Player2;
    public Ball Ball;

    public static float maxFallSpeed = 16f;
    public static float gravityAmount = 12f;
    public static float airRestitution = 3f;
    
    private float hitStopTimestamp = 0;
    public float GameSpeed = 1;
    private float time = 0;

    public float GameTime => time;
    public float GameDelta => Time.deltaTime * GameSpeed;

    private void Start()
    {
        Time.timeScale = 2;
    }

    private void Update()
    {
        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            ThrowBall();
        }
        GameSpeed = Time.time < hitStopTimestamp ? 0f : 1f;
        time += GameDelta;
    }

    public float getDeltaTime()
    {
        return Time.deltaTime / GameSpeed;
    }
    
    public float getTime()
    {
        return Time.time / GameSpeed;
    }

    public void ThrowBall()
    {
        float power = Random.Range(12, 15);
        float direction = Random.Range(100, 160) * Mathf.Deg2Rad;
        
        Ball.transform.position = new Vector3( 10, Random.Range(-3, 3), 0);
        Ball.velocity.current.x = Mathf.Cos(direction) * power;
        Ball.velocity.current.y = Mathf.Sin(direction) * power;
        
        Ball.controller2D.collisions.Reset();
    }

    public void applyHitstop(float _time)
    {
        hitStopTimestamp = Time.time + _time;
    }
}

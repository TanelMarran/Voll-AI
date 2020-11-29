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
    public Transform LeftCourt;
    public Transform RightCourt;

    public static float maxFallSpeed = 16f;
    public static float gravityAmount = 12f;
    public static float airRestitution = 3f;
    
    private float hitStopTimestamp = 0;
    public float GameSpeed = 1;
    private float time = 0;

    public int leftPoint = 0;
    public int rightPoint = 0;

    public float GameTime => time;
    public float GameDelta => Time.deltaTime * GameSpeed;

    public void PositionInRightCourt()
    {
        Vector3 position = RightCourt.position;
        
        float width = Mathf.Max(0, RightCourt.transform.localScale.x - 2) / 2f;
        float center = position.x;
        float xPosition = Random.Range(center - width, center + width);
        Player2.transform.position = new Vector2(10, position.y);
    }
    
    public void PositionInLeftCourt()
    {
        Vector3 position = LeftCourt.position;
        
        float width = Mathf.Max(0, LeftCourt.transform.localScale.x - 2) / 2f;
        float center = position.x;
        float xPosition = Random.Range(center - width, center + width);
        Player1.transform.position = new Vector3( -10, position.y);
    }

    public void ScorePoint()
    {
        if (Ball.transform.position.x < 0)
        {
            rightPoint++;
        }
        else
        {
            leftPoint++;
        }
        
        //Reset();
    }
    
    private void Update()
    {
        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            ThrowBall();
            PositionInLeftCourt();
            PositionInRightCourt();
        }
        GameSpeed = Time.time < hitStopTimestamp ? 0f : 1f;
        time += GameDelta;
    }

    private void FixedUpdate()
    {
        Vector3 pos = Player1.transform.position;
        if (pos.x >= 0)
        {
            Player1.transform.position = new Vector3(0, pos.y, pos.z);
        }
        
        pos = Player2.transform.position;
        if (pos.x <= 0)
        {
            Player2.transform.position = new Vector3(0, pos.y, pos.z);
        }
    }

    public float getDeltaTime()
    {
        return Time.deltaTime / GameSpeed;
    }
    
    public float getTime()
    {
        return Time.time / GameSpeed;
    }

    public void ThrowBall(float side)
    {
        Vector3 position = new Vector3( 8 * side, -4, 0);
        
        float power = 16;
        float direction = Vector2.Angle(Vector2.right, new Vector3(0, 3, 0) - position) * Mathf.Deg2Rad; //Random.Range(125, 150) * Mathf.Deg2Rad;

        Ball.transform.position = position;
        Ball.State.SetState(Ball.DefaultState);
        Ball.velocity.current.x = Mathf.Cos(direction) * power;
        Ball.velocity.current.y = Mathf.Sin(direction) * power;
        
        Ball.controller2D.collisions.Reset();
    }
    
    public void ThrowBall()
    {
        ThrowBall(Random.value > 0.5 ? 1 : -1);
    }

    public void applyHitstop(float _time)
    {
        hitStopTimestamp = Time.time + _time;
    }

    public void Reset()
    {
        PositionInLeftCourt();
        PositionInRightCourt();
        ThrowBall();
    }
    
    public void Reset(float side)
    {
        PositionInLeftCourt();
        PositionInRightCourt();
        ThrowBall(side);
    }
}

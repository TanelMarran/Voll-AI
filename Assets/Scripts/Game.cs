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

    private void Update()
    {
        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            ThrowBall();
        }

        GameSpeed = Time.time < hitStopTimestamp ? 0f : 1f;
        time += GameDelta;
    }

    private void FixedUpdate()
    {
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
    }

    public void ThrowBall()
    {
        ThrowBall(Random.value > 0.5 ? 1 : -1);
    }

    public void applyHitstop(float _time)
    {
        hitStopTimestamp = Time.time + _time;
    }
}

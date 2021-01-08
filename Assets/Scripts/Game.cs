using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using Random = UnityEngine.Random;

public class BoolEvent : UnityEvent<bool> {}

public class Game : MonoBehaviour
{
    public Player LeftPlayer;
    public Player RightPlayer;
    public Ball Ball;

    public static float maxFallSpeed = 16f;
    public static float gravityAmount = 20f;
    public static float airRestitution = 10f;

    public bool isLeftServing = true;

    private float hitStopTimestamp = 0;
    [HideInInspector] public float GameSpeed = 1;
    private float time = 0;

    public int leftPoint = 0;
    public int rightPoint = 0;

    [Tooltip("If true, points will not be awarded for missing serves")]
    public bool noServeMistakePoints = false;

    public BoolEvent OnNetCross;
    private bool hasServed;
    private float ballPreviousX;

    private Vector3 leftPlayerStart;
    private Vector3 rightPlayerStart;
    private Vector3 ballStart;

    public float GameTime => time;
    public float GameDelta => Time.deltaTime * GameSpeed;

    private TextMeshProUGUI _text;

    private static readonly Vector3 leftMirror = new Vector3(-1, 1, 1);

    private void Awake()
    {
        if (OnNetCross == null)
        {
            OnNetCross = new BoolEvent();
        }
    }

    private void Start()
    {
        _text = GetComponentInChildren<TextMeshProUGUI>();

        leftPlayerStart = LeftPlayer.transform.localPosition;
        rightPlayerStart = RightPlayer.transform.localPosition;
        ballStart = Ball.transform.localPosition;

        startNewRound(true);
    }

    private void Update()
    {
        GameSpeed = Time.time < hitStopTimestamp ? 0f : 1f;
        time += GameDelta;

        _text.text = leftPoint + "-" + rightPoint;

        CheckNetCross();
    }

    private void CheckNetCross()
    {
        float ballCurrentX = Ball.transform.localPosition.x;
        bool crossedToRight = ballPreviousX < 0 && ballCurrentX >= 0;
        bool crossedToLeft = ballPreviousX > 0 && ballCurrentX <= 0;

        if (crossedToRight || crossedToLeft)
        {
            hasServed = true;
            OnNetCross.Invoke(crossedToRight);
        }
        
        ballPreviousX = Ball.transform.localPosition.x;
    }

    public void startNewRound(bool leftScored)
    {
        UpdateServingPlayer(leftScored);
        resetPlayers();
        resetBall();
        hasServed = false;
    }

    private void resetBall()
    {
        Ball.transform.localPosition = isLeftServing ? Vector3.Scale(ballStart, leftMirror) : ballStart;
        Ball.controller2D.collisions.Reset();
        Ball.velocity.current = Vector2.zero;
        Ball.velocity.resting = Vector2.zero;
        ballPreviousX = Ball.transform.localPosition.x;
    }

    private void resetPlayers()
    {
        // Left Player
        LeftPlayer.transform.localPosition = leftPlayerStart;
        LeftPlayer.State.SetState(LeftPlayer.PlayerGround);
        LeftPlayer.controller2D.collisions.Reset();
        LeftPlayer.velocity.current = Vector2.zero;

        // Right Player
        RightPlayer.transform.localPosition = rightPlayerStart;
        RightPlayer.State.SetState(RightPlayer.PlayerGround);
        RightPlayer.controller2D.collisions.Reset();
        RightPlayer.velocity.current = Vector2.zero;
    }

    public void startNewGame()
    {
        leftPoint = 0;
        rightPoint = 0;
        isLeftServing = true;
        startNewRound(true);
    }

    private void UpdateServingPlayer(bool leftScored)
    {
        isLeftServing = (isLeftServing && !leftScored || !isLeftServing && leftScored) ? !isLeftServing : isLeftServing; // Maybe change for training
    }

    public float getDeltaTime()
    {
        return Time.deltaTime / GameSpeed;
    }

    public float getTime()
    {
        return Time.time / GameSpeed;
    }

    public void applyHitstop(float _time)
    {
        hitStopTimestamp = Time.time + _time;
    }

    public void AddRightPoint(int addedPoints)
    {
        rightPoint += hasServed ? addedPoints : 0;
        startNewRound(false);
    }
    
    public void AddLeftPoint(int addedPoints)
    {
        leftPoint += hasServed ? addedPoints : 0;
        startNewRound(true);
    }
}

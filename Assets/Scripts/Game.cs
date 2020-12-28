using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using Random = UnityEngine.Random;

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
    public float GameSpeed = 1;
    private float time = 0;

    public int leftPoint = 0;
    public int rightPoint = 0;

    private Vector3 leftPlayerStart;
    private Vector3 rightPlayerStart;
    private Vector3 ballStart;

    public float GameTime => time;
    public float GameDelta => Time.deltaTime * GameSpeed;

    private TextMeshProUGUI _text;

    private static readonly Vector3 leftMirror = new Vector3(-1, 1, 1);

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
    }

    public void startNewRound(bool leftScored)
    {
        UpdateServingPlayer(leftScored);
        resetPlayers();
        resetBall();
    }

    private void resetBall()
    {
        Ball.transform.localPosition = isLeftServing ? Vector3.Scale(ballStart, leftMirror) : ballStart;
        Ball.velocity.current = Vector2.zero;
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
}

using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
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
    public Transform RewardCanvas;
    public RewardNotice RewardNoticePrefab;
    
    public bool isLeftServing = true;

    private float hitStopTimestamp = 0;
    [HideInInspector] public float GameSpeed = 1;
    private float time = 0;

    public TextAppear LeftCelebration;
    public TextAppear RightCelebration;
    public TextAppear WinnerText;
    
    public int WinningPoints = 10;
    
    public int leftPoint = 0;
    public int rightPoint = 0;

    public bool isTraining = true;

    [Tooltip("If true, points will not be awarded for missing serves")]
    public bool noServeMistakePoints = false;
    public bool noRightPlayer = false;
    public Vector2 autoServeRange;

    public BoolEvent OnNetCross;
    private bool hasServed;
    private float ballPreviousX;

    private Vector3 leftPlayerStart;
    private Vector3 rightPlayerStart;
    private Vector3 ballStart;
    
    public int allowedHits;

    public float GameTime => time;
    public float GameDelta => Time.deltaTime * GameSpeed;

    private TextMeshProUGUI _text;
    public TextMeshProUGUI _leftTouchesText;
    public TextMeshProUGUI _rightTouchesText;
    public TextMeshProUGUI winnerText;

    private static readonly Vector3 leftMirror = new Vector3(-1, 1, 1);

    public void displayRewardNotice(bool isLeftPlayer, float value)
    {
        if (value != 0)
        {
            RewardNotice.Create(RewardNoticePrefab, (isLeftPlayer ? LeftPlayer.transform.position : RightPlayer.transform.position) + Vector3.up, RewardCanvas, value);
        }
    }

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

        startNewRound(isLeftServing);
        Ball.OnBallTouched.AddListener(OnBallTouch);
        LeftPlayer.setHits(allowedHits);
        RightPlayer.setHits(allowedHits);

        WinningPoints = PlayState.firstTo;
    }

    private void OnBallTouch(Ball.PlayerHit player)
    {
        if (player.Player == LeftPlayer)
        {
            LeftPlayer.setHits(LeftPlayer.getHits() - 1);
            RightPlayer.setHits(allowedHits);
        }
        else
        {
            RightPlayer.setHits(RightPlayer.getHits() - 1);
            LeftPlayer.setHits(allowedHits);
        }
    }

    private void Update()
    {
        GameSpeed = Time.time < hitStopTimestamp ? 0f : 1f;
        time += GameDelta;

        _text.text = leftPoint + "-" + rightPoint;
        _leftTouchesText.text = LeftPlayer.getHits().ToString();
        _rightTouchesText.text = RightPlayer.getHits().ToString();

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
        Ball.Activate();
        StartCoroutine(ballResetCorutine());
    }

    private IEnumerator ballResetCorutine()
    {
        yield return new WaitForFixedUpdate();
        Ball.transform.localPosition = isLeftServing ? Vector3.Scale(ballStart, leftMirror) : ballStart;
        Ball.velocity.current = Vector2.zero;
        Ball.velocity.resting = Vector2.zero;
        Ball.controller2D.collisions.Reset();
        if (!isLeftServing && noRightPlayer)
        {
            Ball.velocity.current = new Vector2(-3, 1.5f) * Random.Range(autoServeRange.x, autoServeRange.y);
            Ball.velocity.resting = Ball.velocity.current;
        }
        ballPreviousX = Ball.transform.localPosition.x;
    }

    private void resetPlayers()
    {
        LeftPlayer.setHits(allowedHits);
        RightPlayer.setHits(allowedHits);
        
        if (LeftPlayer.State != null)
        {
            // Left Player
            LeftPlayer.transform.localPosition = leftPlayerStart;
            LeftPlayer.State.SetState(LeftPlayer.PlayerGround);
            LeftPlayer.isHitting = false;
            LeftPlayer.HitStateTimestamp = Time.time;
            LeftPlayer.controller2D.collisions.Reset();
            LeftPlayer.velocity.current = Vector2.zero;
        }

        if (RightPlayer.State != null)
        {
            // Right Player
            RightPlayer.transform.localPosition = rightPlayerStart;
            RightPlayer.State.SetState(RightPlayer.PlayerGround);
            RightPlayer.isHitting = false;
            RightPlayer.HitStateTimestamp = Time.time;
            RightPlayer.controller2D.collisions.Reset();
            RightPlayer.velocity.current = Vector2.zero;
        }
    }

    public void startNewGame()
    {
        startNewGame(Random.value > .5f);
    }
    
    public void startNewGame(bool leftScored)
    {
        leftPoint = 0;
        rightPoint = 0;
        isLeftServing = true;
        startNewRound(leftScored);
    }

    private void UpdateServingPlayer(bool leftScored)
    {
        if (noRightPlayer)
        {
            isLeftServing = !isLeftServing;
        }
        else
        {
            isLeftServing = (isLeftServing && !leftScored || !isLeftServing && leftScored) ? !isLeftServing : isLeftServing; // Maybe change for training
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

    public void applyHitstop(float _time)
    {
        hitStopTimestamp = Time.time + _time;
    }

    public void AddRightPoint(int addedPoints)
    {
        rightPoint += hasServed || !noServeMistakePoints ? addedPoints : 0;
        performRoundEnd(false);
    }
    
    public void AddLeftPoint(int addedPoints)
    {
        leftPoint += hasServed || !noServeMistakePoints ? addedPoints : 0;
        performRoundEnd(true);
    }

    private void performRoundEnd(bool leftScored)
    {
        if (isTraining)
        {
            startNewRound(leftScored);
        }
        else
        {
            if (leftPoint >= WinningPoints || rightPoint >= WinningPoints)
            {
                winnerText.text = leftScored ? "Left wins!" : "Right wins!";
                WinnerText.In(true);
                StartCoroutine(CelebrateVictory());
            }
            else
            {
                if (leftScored)
                {
                    LeftCelebration.In(true);
                }
                else
                {
                    RightCelebration.In(true);
                }
                StartCoroutine(CelebratePoint(leftScored));
            }
        }
    }

    IEnumerator CelebratePoint(bool leftScored)
    {
        Ball.Deactivate();
        yield return new WaitForSeconds(2);
        LeftCelebration.Out(true);
        RightCelebration.Out(true);
        yield return new WaitForSeconds(1);
        LeftPlayer.MovementPaused = true;
        RightPlayer.MovementPaused = true;
        Ball.MovementPaused = true;
        Transition.Play(() => startNewRound(leftScored));
        yield return new WaitForSeconds(1f);
        LeftPlayer.MovementPaused = false;
        RightPlayer.MovementPaused = false;
        Ball.MovementPaused = false;
    }
    
    IEnumerator CelebrateVictory()
    {
        Ball.Deactivate();
        yield return new WaitForSeconds(3f);
        LeftCelebration.Out(true);
        RightCelebration.Out(true);
        Transition.Play(() => SceneManager.LoadScene(0));
    }
}

using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;

public class PlayerAgent : Agent
{
    public bool isLeftPlayer;

    public Player _self;
    public Player _opponent;
    public Ball _ball;

    public Vector2 _xSide;

    private int _selfAccountedPoints = 0;
    private int _opponentAccountedPoints = 0;

    private bool _selfTouchedBall = false;
    private bool _selfNetCross = false;
    private bool _selfMissedBall = false;

    private const float RewardNetCross = 0f;
    private const float RewardTouch = 0f;
    private const float RewardWin = 1f;
    private const float RewardPoint = 0f;
    
    private const float PenaltyBallMiss = 0f;
    private const float PenaltyPointLossed = 0;
    private const float PenaltyGameLossed = 1f;

    public static Vector2 NormalizeVector = new Vector2(1f / 8.5f, 1f / 10f);

    private const int WinningScore = 1;
    
    private void Start()
    {
        _self = GetComponent<Player>();
        _opponent = isLeftPlayer ? _self.game.RightPlayer : _self.game.LeftPlayer;
        _ball = _self.game.Ball;
        _xSide = isLeftPlayer ? new Vector2(-1, 1) : new Vector2(1, 1);
        
        _ball.OnBallTouched.AddListener(OnBallTouch);
        _ball.game.OnNetCross.AddListener(OnNetCross);
        _self.OnBallMissed.AddListener(OnBallMiss);
    }

    private void OnBallTouch(Player player)
    {
        if (player == _self)
        {
            _selfTouchedBall = true;
        }
    }
    
    private void OnBallMiss()
    {
        _selfMissedBall = true;
    }
    
    private void OnNetCross(bool crossedToRight)
    {
        if (crossedToRight && isLeftPlayer || !crossedToRight && !isLeftPlayer)
        {
            _selfNetCross = true;
        }
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        Vector2 scaler = Vector2.Scale(NormalizeVector, _xSide);
        
        // Self
        sensor.AddObservation(Vector2.Scale(_self.transform.localPosition, scaler));
        sensor.AddObservation(Vector2.Scale(_self.velocity.current, scaler));
        sensor.AddObservation(_self.currentDashes);

        // Opponent
        sensor.AddObservation(Vector2.Scale(_opponent.transform.localPosition, scaler));
        sensor.AddObservation(Vector2.Scale(_opponent.velocity.current, scaler));
        sensor.AddObservation(_opponent.currentDashes);
        
        // Ball
        sensor.AddObservation(Vector2.Scale(_ball.transform.localPosition, scaler));
        sensor.AddObservation(Vector2.Scale(_ball.velocity.current, scaler));
    }

    public override void OnActionReceived(float[] vectorAction)
    {
        Vector2 _movement = Vector2.Scale(new Vector2(vectorAction[0] == 0 ? 0 : vectorAction[0] == 1 ? -1 : 1 , vectorAction[1] == 0 ? 0 : vectorAction[1] == 1 ? -1 : 1), _xSide);
        _self.inputs.SetMovementKey(_movement);
        _self.inputs.SetJumpKey(vectorAction[2] == 0 ? 1 : 0);
        _self.inputs.SetHitKey(vectorAction[3] == 0 ? 1 : 0);

        if (_selfTouchedBall)
        {
            AddReward(RewardTouch);
            _selfTouchedBall = false;
        }
        
        if (_selfNetCross)
        {
            AddReward(RewardNetCross);
            _selfNetCross = false;
        }
        
        if (_selfMissedBall)
        {
            AddReward(-PenaltyBallMiss);
            _selfMissedBall = false;
        }

        if (isLeftPlayer)
        {
            if (_self.game.leftPoint >= WinningScore)
            {
                AddReward(RewardWin);
                EndEpisode();
            }
        
            if (_opponent.game.rightPoint >= WinningScore)
            {
                AddReward(-PenaltyGameLossed);
                EndEpisode();
            }
            
            if (_self.game.leftPoint > _selfAccountedPoints)
            {
                AddReward(RewardPoint);
                _selfAccountedPoints++;
            }
            
            if (_self.game.rightPoint > _opponentAccountedPoints)
            {
                AddReward(-PenaltyPointLossed);
                _opponentAccountedPoints++;
            }
        }
        else
        {
            if (_self.game.rightPoint >= WinningScore)
            {
                AddReward(RewardWin);
                EndEpisode();
            }
        
            if (_opponent.game.leftPoint >= WinningScore)
            {
                AddReward(-PenaltyGameLossed);
                EndEpisode();
            }
            
            if (_self.game.rightPoint > _selfAccountedPoints)
            {
                AddReward(RewardPoint);
                _selfAccountedPoints++;
            }
            
            if (_self.game.leftPoint > _opponentAccountedPoints)
            {
                AddReward(-PenaltyPointLossed);
                _opponentAccountedPoints++;
            }
        }
    }

    public override void OnEpisodeBegin()
    {
        _self.game.startNewGame();
        _selfAccountedPoints = 0;
        _opponentAccountedPoints = 0;
    }
}

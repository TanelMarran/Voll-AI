using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
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

    private const float RewardNetCross = 0.2f;
    private const float RewardTouch = 0.033f;
    private const float RewardWin = 0f;
    private const float RewardPoint = 1f;
    
    private const float PenaltyBallMiss = 0.001f;
    private const float PenaltyPointLossed = 1f;
    private const float PenaltyGameLossed = 0f;

    public static Vector2 NormalizeVector = new Vector2(1f / 8.5f, 1f / 10f);

    private const int WinningScore = 10;
    
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

        /*/ Opponent
        sensor.AddObservation(Vector2.Scale(_opponent.transform.localPosition, scaler));
        sensor.AddObservation(Vector2.Scale(_opponent.velocity.current, scaler));
        sensor.AddObservation(_opponent.currentDashes);
        //*/
        
        // Ball
        sensor.AddObservation(Vector2.Scale(_ball.transform.localPosition, scaler));
        sensor.AddObservation(Vector2.Scale(_ball.velocity.current, scaler));
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        var inputs = actions.DiscreteActions;
        
        Vector2 movement = Vector2.Scale(new Vector2(inputs[0] == 0 ? 0 : inputs[0] == 1 ? -1 : 1 , inputs[1] == 0 ? 0 : inputs[1] == 1 ? -1 : 1), _xSide);
        _self.inputs.SetMovementKey(movement);
        _self.inputs.SetJumpKey(inputs[2] == 0 ? 1 : 0);
        _self.inputs.SetHitKey(inputs[3] == 0 ? 1 : 0);

        AssignRewards();
    }

    public void AssignRewards()
    {
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
                SetReward(RewardWin);
                EndEpisode();
            }
        
            if (_opponent.game.rightPoint >= WinningScore)
            {
                SetReward(-PenaltyGameLossed);
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
                SetReward(RewardWin);
                EndEpisode();
            }
        
            if (_opponent.game.leftPoint >= WinningScore)
            {
                SetReward(-PenaltyGameLossed);
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

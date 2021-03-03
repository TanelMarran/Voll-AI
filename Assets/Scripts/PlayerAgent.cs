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
    private Game _game;

    public Vector2 _xSide;

    private int _selfAccountedPoints = 0;
    private int _opponentAccountedPoints = 0;

    private bool _selfTouchedBall = false;
    private bool _selfNetCross = false;
    private bool _selfMissedBall = false;

    private PlayerAgent _opponentAgent;

    private const float RewardNetCross = 0f;
    private const float RewardTouch = 0f;
    private const float RewardWin = 1f;
    private const float RewardPoint = 0f;
    
    private const float PenaltyBallMiss = 0f;
    private const float PenaltyPointLossed = 0f;
    private const float PenaltyGameLossed = 1f;

    private int _rounds;

    public static Vector2 NormalizeVector = new Vector2(1f / 8.5f, 1f / 10f);

    private const int WinningScore = 10;
    
    public void Start()
    {
        _rounds = 0;
        _self = GetComponent<Player>();
        _opponent = isLeftPlayer ? _self.game.RightPlayer : _self.game.LeftPlayer;
        _ball = _self.game.Ball;
        _xSide = isLeftPlayer ? new Vector2(-1, 1) : new Vector2(1, 1);
        _game = _self.game;

        _opponentAgent = _opponent.GetComponent<PlayerAgent>();
        
        _ball.OnBallTouched.AddListener(OnBallTouch);
        _self.game.OnNetCross.AddListener(OnNetCross);
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
        sensor.AddObservation(_self.getHits());

        // Opponent
        sensor.AddObservation(Vector2.Scale(_opponent.transform.localPosition, scaler));
        sensor.AddObservation(Vector2.Scale(_opponent.velocity.current, scaler));
        sensor.AddObservation(_opponent.currentDashes);

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
            _game.displayRewardNotice(isLeftPlayer, RewardTouch);
            _selfTouchedBall = false;
        }
        
        if (_selfNetCross)
        {
            AddReward(RewardNetCross);
            _game.displayRewardNotice(isLeftPlayer, RewardNetCross);
            _selfNetCross = false;
        }
        
        if (_selfMissedBall)
        {
            AddReward(-PenaltyBallMiss);
            _game.displayRewardNotice(isLeftPlayer, -PenaltyBallMiss);
            _selfMissedBall = false;
        }

        if (isLeftPlayer)
        {
            if (_self.game.leftPoint >= WinningScore)
            {
                WinReward(true);
            }
        
            if (_opponent.game.rightPoint >= WinningScore)
            {
                WinReward(false);
            }
            
            if (_self.game.leftPoint > _selfAccountedPoints)
            {
                AddReward(RewardPoint);
                _game.displayRewardNotice(isLeftPlayer, RewardPoint);
                _selfAccountedPoints++;
            }
            
            if (_self.game.rightPoint > _opponentAccountedPoints)
            {
                AddReward(-PenaltyPointLossed);
                _game.displayRewardNotice(isLeftPlayer, -PenaltyPointLossed);
                _opponentAccountedPoints++;
            }
        }
        else
        {
            if (_self.game.rightPoint >= WinningScore)
            {
                WinReward(true);
            }
        
            if (_opponent.game.leftPoint >= WinningScore)
            {
                WinReward(false);
            }
            
            if (_self.game.rightPoint > _selfAccountedPoints)
            {
                AddReward(RewardPoint);
                _game.displayRewardNotice(isLeftPlayer, RewardPoint);
                _selfAccountedPoints++;
            }
            
            if (_self.game.leftPoint > _opponentAccountedPoints)
            {
                AddReward(-PenaltyPointLossed);
                _game.displayRewardNotice(isLeftPlayer, -PenaltyPointLossed);
                _opponentAccountedPoints++;
            }
        }
    }

    public void WinReward(bool isWinner)
    {
        float selfReward = isWinner ? RewardWin : -PenaltyGameLossed;
        float opponentReward = !isWinner ? RewardWin : -PenaltyGameLossed;
        
        AddReward(selfReward);
        _opponentAgent.AddReward(opponentReward);
        _game.displayRewardNotice(isLeftPlayer, selfReward);
        _game.displayRewardNotice(!isLeftPlayer, opponentReward);
        _opponentAgent.EndEpisode();
        EndEpisode();
    }

    public override void Heuristic(in ActionBuffers actionsOut) {}

    public override void OnEpisodeBegin()
    {
        _rounds++;
        _self.game.startNewGame(_rounds % 2 == 0);
        _selfAccountedPoints = 0;
        _opponentAccountedPoints = 0;
    }
}

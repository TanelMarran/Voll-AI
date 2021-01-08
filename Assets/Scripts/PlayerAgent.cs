using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;

public class PlayerAgent : Agent
{
    public bool isLeftPlayer;

    private Player _self;
    private Player _opponent;
    private Ball _ball;

    private Vector2 _xSide;

    private int _selfAccountedPoints = 0;
    private int _opponentAccountedPoints = 0;

    private bool _selfTouchedBall = false;
    private bool _selfNetCross = false;

    private const float RewardNetCross = 0;//1f;
    private const float RewardTouch = 0;//.1f;
    private const float RewardWin = 1f;
    private const float RewardPoint = 0;//.3f;
    
    private static Vector2 NormalizeVector = new Vector2(1f / 8.5f, 1f / 10f);

    private const int WinningScore = 1;
    
    private void Start()
    {
        _self = GetComponent<Player>();
        _opponent = isLeftPlayer ? _self.game.RightPlayer : _self.game.LeftPlayer;
        _ball = _self.game.Ball;
        _xSide = isLeftPlayer ? new Vector2(-1, 1) : new Vector2(1, 1);
        
        _ball.OnBallTouched.AddListener(OnBallTouch);
        _ball.game.OnNetCross.AddListener(OnNetCross);
    }

    private void OnBallTouch(Player player)
    {
        if (player == _self)
        {
            _selfTouchedBall = true;
        }
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
        sensor.AddObservation(Vector2.Scale(_self.transform.position, scaler));
        sensor.AddObservation(Vector2.Scale(_self.velocity.current, scaler));
        sensor.AddObservation(_self.State.getCurrentIndex());

        // Opponent
        sensor.AddObservation(Vector2.Scale(_opponent.transform.position, scaler));
        sensor.AddObservation(Vector2.Scale(_opponent.velocity.current, scaler));
        sensor.AddObservation(_opponent.State.getCurrentIndex());
        
        // Ball
        sensor.AddObservation(Vector2.Scale(_ball.transform.position, scaler));
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

        if (isLeftPlayer)
        {
            if (_self.game.leftPoint >= WinningScore)
            {
                AddReward(RewardWin);
                EndEpisode();
            }
        
            if (_opponent.game.rightPoint >= WinningScore)
            {
                AddReward(-RewardWin);
                EndEpisode();
            }
            
            if (_self.game.leftPoint > _selfAccountedPoints)
            {
                AddReward(RewardPoint);
                _selfAccountedPoints++;
            }
            
            if (_self.game.rightPoint > _opponentAccountedPoints)
            {
                AddReward(-RewardPoint);
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
                AddReward(-RewardWin);
                EndEpisode();
            }
            
            if (_self.game.rightPoint > _selfAccountedPoints)
            {
                AddReward(RewardPoint);
                _selfAccountedPoints++;
            }
            
            if (_self.game.leftPoint > _opponentAccountedPoints)
            {
                AddReward(-RewardPoint);
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

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
    
    private void Start()
    {
        _self = GetComponent<Player>();
        _opponent = isLeftPlayer ? _self.game.RightPlayer : _self.game.LeftPlayer;
        _ball = _self.game.Ball;
        _xSide = isLeftPlayer ? new Vector2(-1, 1) : new Vector2(1, 1);
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        // Self
        sensor.AddObservation(Vector2.Scale(_self.transform.position, _xSide));
        sensor.AddObservation(Vector2.Scale(_self.velocity.current, _xSide));
        sensor.AddObservation(_self.State.getCurrentIndex());

        // Opponent
        sensor.AddObservation(Vector2.Scale(_opponent.transform.position, _xSide));
        sensor.AddObservation(Vector2.Scale(_opponent.velocity.current, _xSide));
        sensor.AddObservation(_opponent.State.getCurrentIndex());
        
        // Ball
        sensor.AddObservation(Vector2.Scale(_ball.transform.position, _xSide));
        sensor.AddObservation(Vector2.Scale(_ball.velocity.current, _xSide));
    }

    public override void OnActionReceived(float[] vectorAction)
    {
        Vector2 _movement = Vector2.Scale(new Vector2(vectorAction[0] == 0 ? 0 : vectorAction[0] == 1 ? -1 : 1 , vectorAction[1] == 0 ? 0 : vectorAction[1] == 1 ? -1 : 1), _xSide);
        _self.inputs.SetMovementKey(_movement);
        _self.inputs.SetJumpKey(vectorAction[2] == 0 ? 1 : 0);
        _self.inputs.SetHitKey(vectorAction[3] == 0 ? 1 : 0);

        if (isLeftPlayer)
        {
            if (_self.game.leftPoint > 5)
            {
                AddReward(1f);
                EndEpisode();
            }
        
            if (_opponent.game.rightPoint > 5)
            {
                AddReward(-1f);
                EndEpisode();
            }
            
            if (_self.game.leftPoint > _selfAccountedPoints)
            {
                AddReward(1f);
                _selfAccountedPoints++;
            }
            
            if (_self.game.rightPoint > _opponentAccountedPoints)
            {
                AddReward(-1f);
                _opponentAccountedPoints++;
            }
        }
        else
        {
            if (_self.game.rightPoint > 5)
            {
                AddReward(1f);
                EndEpisode();
            }
        
            if (_opponent.game.leftPoint > 5)
            {
                AddReward(-1f);
                EndEpisode();
            }
            
            if (_self.game.rightPoint > _selfAccountedPoints)
            {
                AddReward(1f);
                _selfAccountedPoints++;
            }
            
            if (_self.game.leftPoint > _opponentAccountedPoints)
            {
                AddReward(-1f);
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


using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using UnityEngine.InputSystem;

public class PlayerAgent : Agent
{
    public Ball ball;
    public Player otherPlayer;
    public Player agentPlayer;
    public bool XFlipped;
    
    private float xFlipMul;
    private float accountedPoints;

    public override void CollectObservations(VectorSensor sensor)
    {
        // Agent
        var pos = agentPlayer.transform.localPosition / 11.5f;
        sensor.AddObservation(pos.x * xFlipMul);
        sensor.AddObservation(pos.y);
        
        var vel = agentPlayer.velocity.current;
        sensor.AddObservation(vel.x * xFlipMul);
        sensor.AddObservation(vel.y);

        // Opponent
        pos = otherPlayer.transform.localPosition / 11.5f;
        sensor.AddObservation(pos.x * xFlipMul);
        sensor.AddObservation(pos.y);

        vel = otherPlayer.velocity.current;
        sensor.AddObservation(vel.x * xFlipMul);
        sensor.AddObservation(vel.y);

        // Ball
        pos = ball.transform.localPosition / 11.5f;
        sensor.AddObservation(pos.x * xFlipMul);
        sensor.AddObservation(pos.y);
        
        vel = ball.velocity.current;
        sensor.AddObservation(vel.x * xFlipMul);
        sensor.AddObservation(vel.y);
    }

    public override void OnActionReceived(float[] actions)
    {
        Vector2 movement = new Vector2();
        movement.x = (int)actions[0] == 0 ? xFlipMul : (int)actions[0] == 1 ? -xFlipMul : 0;
        movement.y = (int)actions[1] == 0 ? xFlipMul : (int)actions[1] == 1 ? -xFlipMul : 0;
        agentPlayer.inputManager.SetMovementKey(movement);
        agentPlayer.inputManager.SetJumpKey((int)actions[2]);
        agentPlayer.inputManager.SetDashKey((int)actions[3]);
        float checkPoints = ball.Game.leftPoint;
        float otherPoints = ball.Game.rightPoint;

        if (agentPlayer.Game.Player2.Equals(agentPlayer))
        {
            checkPoints = ball.Game.rightPoint;
            otherPoints = ball.Game.leftPoint;
        }
        
        // Reached target
        if (checkPoints > accountedPoints)
        {
            AddReward(1);
            accountedPoints = checkPoints;
        }

        if (ball.Game.rightPoint > 3 || ball.Game.leftPoint > 3)
        {
            if (checkPoints > otherPoints)
            {
                AddReward(1.0f);
            }
            else
            {
                AddReward(-1.0f);
            }
            EndEpisode();
        }
    }

    public override void Heuristic(float[] actionsOut)
    {
        actionsOut[0] = (Keyboard.current.rightArrowKey.isPressed ? 1 : 0) - (Keyboard.current.leftArrowKey.isPressed ? 1 : 0);
        actionsOut[1] = (Keyboard.current.upArrowKey.isPressed ? 1 : 0) - (Keyboard.current.downArrowKey.isPressed ? 1 : 0);
        actionsOut[2] = Keyboard.current.zKey.isPressed ? 1 : 0;
        actionsOut[3] = Keyboard.current.xKey.isPressed ? 1 : 0;
    }

    public override void OnEpisodeBegin()
    {
        xFlipMul = XFlipped ? -1f : 1f;

        ball.Game.Reset(xFlipMul);
        ball.Game.leftPoint = 0;
        ball.Game.rightPoint = 0;
        accountedPoints = 0;
    }
}

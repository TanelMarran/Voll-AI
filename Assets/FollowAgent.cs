using System.Collections;
using System.Collections.Generic;
using Input;
using UnityEngine;

//Follows the ball and hits it when it is in range
public class FollowAgent : MonoBehaviour
{
    public float targetHeight = 6f;
    private PlayerInputTransformer _input;
    private Player _player;
    private Ball _ball;
    
    // Start is called before the first frame update
    void Start()
    {
        _input = GetComponent<PlayerInputTransformer>();
        _player = GetComponent<Player>();
        _ball = _player.game.Ball;
    }

    // Update is called once per frame
    void Update()
    {
        var ballPosition = _ball.transform.position;
        var playerPosition = transform.position;
        var targetX = ballPosition.x * 1.1f;

        if (_ball.transform.localPosition.x < 0)
        {
            targetX = 5f;
        }
        
        _input.SetMovementKey(new Vector2(targetX < playerPosition.x ? -1f : 1f, 0));

        if (Vector2.Distance(ballPosition, playerPosition) < 1.75f)
        {
            _input.SetHitKey(1);
        }
        else
        {
            _input.SetHitKey(0);
        }
    }
}

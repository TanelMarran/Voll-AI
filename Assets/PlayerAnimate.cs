using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimate : MonoBehaviour
{
    private Player _player;
    private Animator _animator;

    private float _dashingStamp;
    private bool _wasDashing;

    private bool _isFlipped;
    
    // Start is called before the first frame update
    void Start()
    {
        _player = GetComponentInParent<Player>();
        _animator = GetComponent<Animator>();
        _dashingStamp = Time.time;
        _isFlipped = _player.transform.localScale.x == -1;
    }

    // Update is called once per frame
    void Update()
    {
        bool isDashing = _player.State._state == _player.PlayerDash;

        _animator.SetBool("Is Grounded", _player.State._state == _player.PlayerGround);
        
        if (isDashing && !_wasDashing)
        {
            _animator.SetBool("Is Dashing", true);
            _dashingStamp = Time.time + .2f;
        }

        if (Time.time >= _dashingStamp)
        {
            _animator.SetBool("Is Dashing", false);
        }
        _animator.SetFloat("Speed", Mathf.Abs(_player.velocity.current.x));
        
        float angle = Vector3.SignedAngle(_player.velocity.current, Vector3.up, _isFlipped ? Vector3.forward : Vector3.back) + 45f;
        angle = angle >= 0 ? angle : 360 + angle;
        angle = angle / 360f;
        
        _animator.SetFloat("Dash Direction", angle);

        _wasDashing = isDashing;
    }
}

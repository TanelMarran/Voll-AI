using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Vector2 = System.Numerics.Vector2;
using Vector3 = UnityEngine.Vector3;

public class BallStretch : MonoBehaviour
{
    private Ball _ball;


    // Start is called before the first frame update
    void Start()
    {
        _ball = GetComponentInParent<Ball>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float angle = Vector3.SignedAngle(_ball.velocity.current, Vector3.up, Vector3.back);
        float speed = _ball.velocity.current.magnitude / 10f * .2f;
        
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        transform.localScale = new Vector3(1 - speed, 1 + speed, 0);
    }
}

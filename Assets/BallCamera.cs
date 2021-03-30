using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallCamera : MonoBehaviour
{
    public Ball ball;
    public Vector3 pivotOrigin;
    public Vector2 pivotMagnitude;

    private Camera _camera;

    private void Start()
    {
        pivotOrigin = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 factor = Vector2.Scale(ball.transform.localPosition, PlayerAgent.NormalizeVector);

        transform.position = pivotOrigin + (Vector3)(factor * pivotMagnitude);
    }
}

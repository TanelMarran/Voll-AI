using System;
using Movement;
using UnityEngine;

public class StateMachine<T>
{
    private State<T> _state;

    public StateMachine()
    {
    }

    public void SetState(State<T> state)
    {
        _state = state;
    }

    public void Update()
    {
        _state.Update();
    }

    public void FixedUpdate()
    {
        _state.FixedUpdate();
    }

    public override string ToString()
    {
        return _state.ToString();
    }
}
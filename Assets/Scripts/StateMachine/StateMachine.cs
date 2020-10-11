using System;
using Movement;
using UnityEngine;

[Serializable]
public class StateMachine<T>
{
    [SerializeField] public State<T> _state;

    public StateMachine()
    {
    }

    public void SetState(State<T> state)
    {
        _state = state;
        _state.Start();
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
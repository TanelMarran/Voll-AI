using Movement;
using UnityEngine;

[RequireComponent(typeof(Controller2D))]
public class Ball : MonoBehaviour
{
    public Game Game;
    
    public MovementVector velocity;
    public float gravity = 8;
    
    [HideInInspector] public Controller2D controller2D;
    public StateMachine<Ball> State;
    public State<Ball> DefaultState;
    public State<Ball> BurstState;
    public State<Ball> HangState;

    // Start is called before the first frame update
    void Start()
    {
        controller2D = GetComponent<Controller2D>();
        InitializeStates();
    }

    void InitializeStates()
    {
        State = new StateMachine<Ball>();
        DefaultState = new BallDefault(this);
        BurstState = new BallBurst(this);
        HangState = new BallHang(this);
        State.SetState(DefaultState);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        State.FixedUpdate();
    }
}
using Input;
using Movement;
using UnityEngine;

[RequireComponent(typeof(Controller2D))]
public class Player : MonoBehaviour
{
    public Game game;
    
    public float movementSpeed = 5;
    public float jumpPower = 20;

    public StateMachine<Player> State;
    public PlayerGround PlayerGround;
    public PlayerAir PlayerAir;
    public PlayerDash PlayerDash;
    public MainInput Actions;
    public ContactFilter2D ballFilter;
    
    public float gravity = -40;
    public MovementVector velocity;

    [HideInInspector] public Controller2D controller2D;
    [HideInInspector] public InputHandler input;
    private CircleCollider2D _ballCollider;

    private void Start()
    {
        State = new StateMachine<Player>();
        InitializeStates();
        controller2D = GetComponent<Controller2D>();
        input = GetComponent<InputHandler>();
        _ballCollider = GetComponent<CircleCollider2D>();
    }

    private void Awake()
    {
        Actions = new MainInput();
    }

    private void OnEnable()
    {
        Actions.Enable();
    }

    private void OnDisable()
    {
        
        Actions.Disable();
    }

    private void InitializeStates()
    {
        PlayerGround = new PlayerGround(this);
        PlayerAir = new PlayerAir(this);
        PlayerDash = new PlayerDash(this);
        State.SetState(PlayerGround);
    }

    private void Update()
    {
        State.Update();
    }

    private void FixedUpdate()
    {
        State.FixedUpdate();
    }
    
    public bool IsBallInRange()
    {
        return _ballCollider.IsTouching(game.Ball.controller2D.collider);
    }

    public void HitBehaviour(bool hit)
    {
        if (hit && IsBallInRange())
        {
            Ball ball = game.Ball;

            Vector2 hitVelocity = Actions.Player.Movement.ReadValue<Vector2>() * 10f;
            ball.velocity.current = hitVelocity;
            ball.State.SetState(ball.BurstState);
        }
    }
}
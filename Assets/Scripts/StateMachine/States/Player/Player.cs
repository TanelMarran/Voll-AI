using System;
using Input;
using Movement;
using UnityEngine;
using UnityEngine.Serialization;

[RequireComponent(typeof(Controller2D))]
[RequireComponent(typeof(PlayerInputTransformer))]
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
    public bool isJumping = false;
    [HideInInspector] public PlayerInputTransformer inputTransformer;

    [HideInInspector] public Controller2D controller2D;
    [NonSerialized] public CircleCollider2D _ballCollider;
    
    public LineRenderer Line;

    private void Start()
    {
        State = new StateMachine<Player>();
        InitializeStates();
        controller2D = GetComponent<Controller2D>();
        _ballCollider = GetComponent<CircleCollider2D>();
        inputTransformer = GetComponent<PlayerInputTransformer>();
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
}
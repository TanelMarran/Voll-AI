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
    
    public float movementSpeed = 5f;
    public float jumpPower = 20f;
    public float DashStrength = 10f;
    public float HitStrength = 15f;

    public StateMachine<Player> State;
    public PlayerGround PlayerGround;
    public PlayerAir PlayerAir;
    public PlayerDash PlayerDash;
    public MainInput Actions;
    public ContactFilter2D ballFilter;
    
    public float gravity = -40;
    public MovementVector velocity;
    public bool isJumping = false;

    [HideInInspector] public bool isHitting = false;
    public float HitActiveTime = 10f;
    public float HitCooldownTime = 2f;
    [HideInInspector] public float HitStateTimestamp = 0f;
    
    [HideInInspector] public PlayerInputTransformer inputs;
    [HideInInspector] public Controller2D controller2D;
    [HideInInspector] public CircleCollider2D ballCollider;

    private void Start()
    {
        State = new StateMachine<Player>();
        InitializeStates();
        controller2D = GetComponent<Controller2D>();
        ballCollider = GetComponent<CircleCollider2D>();
        inputs = GetComponent<PlayerInputTransformer>();
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
        PlayerGround = new PlayerGround(this, 0);
        PlayerAir = new PlayerAir(this, 1);
        PlayerDash = new PlayerDash(this, 2);
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

    public void HitBehaviour()
    {
        if (isHitting)
        {
            var collision = ballCollider.Distance(game.Ball.Collider2D);
            bool ballHit = collision.distance <= 0f;
            
            if (ballHit)
            {
                var factor = Mathf.Clamp(Mathf.Abs(collision.distance) / ballCollider.radius + 0.5f, 0, 1);
                game.Ball.velocity.current = collision.normal * (HitStrength * factor);
            }
            
            if (Time.time > HitStateTimestamp || ballHit)
            {
                isHitting = false;
                HitStateTimestamp = Time.time + HitCooldownTime;
            }
        }
        else
        {
            if (inputs.HitPressed() && Time.time > HitStateTimestamp)
            {
                isHitting = true;
                HitStateTimestamp = Time.time + HitActiveTime;
            }
        }
    }
}
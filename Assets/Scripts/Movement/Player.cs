using System;
using System.Collections.Generic;
using Input;
using Movement;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

[RequireComponent(typeof(Controller2D))]
public class Player : MonoBehaviour
{
    public float movementSpeed = 5;
    public float jumpPower = 20;
    
    [SerializeField] public StateMachine<Player> State;
    public PlayerGround PlayerGround;
    public PlayerAir PlayerAir;
    public PlayerDash PlayerDash;
    public MainInput actions;
    public ContactFilter2D ballFilter;
    
    public float gravity = -40;
    public Vector3 velocity;

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
        actions = new MainInput();
    }

    private void OnEnable()
    {
        actions.Enable();
    }

    private void OnDisable()
    {
        
        actions.Disable();
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
        List<Collider2D> results = new List<Collider2D>();
        
        ContactFilter2D filter = new ContactFilter2D();
        filter.SetLayerMask(LayerMask.GetMask($"Ball"));

        int hit = _ballCollider.OverlapCollider(filter, results);

        return hit > 0;
    }
}
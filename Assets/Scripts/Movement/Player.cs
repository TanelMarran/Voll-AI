using Movement;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.Serialization;

[RequireComponent(typeof(Controller2D), typeof(InputController))]
public class Player : MonoBehaviour
{
    [FormerlySerializedAs("MovementSpeed")] public float movementSpeed = 5;
    [FormerlySerializedAs("JumpPower")] public float jumpPower = 20;
    
    public StateMachine<Player> State;
    public PlayerMove PlayerMove;
    
    public float gravity = -40;
    public Vector3 velocity;

    [FormerlySerializedAs("_controller2D"), HideInInspector] public Controller2D controller2D;
    [FormerlySerializedAs("_inputController"), HideInInspector] public InputController inputController;

    private void Start()
    {
        controller2D = GetComponent<Controller2D>();
        inputController = GetComponent<InputController>();
        State = new StateMachine<Player>();
        InitializeStates();
    }

    private void InitializeStates()
    {
        PlayerMove = new PlayerMove(this);
        State.SetState(PlayerMove);
    }

    private void FixedUpdate()
    {
        State.FixedUpdate();
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControlManager : MonoBehaviour
{
    public PlayerInput Human;
    public PlayerAgent Machine;
    public bool isLeftPlayer;
    
    // Start is called before the first frame update
    void Start()
    {
        if (isLeftPlayer)
        {
            Human.enabled = !PlayState.isLeftMachine;
            Machine.enabled = PlayState.isLeftMachine;
        }
        else
        {
            Human.enabled = !PlayState.isRightMachine;
            Machine.enabled = PlayState.isRightMachine;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using Unity.Barracuda;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControlManager : MonoBehaviour
{
    public PlayerInput Human;
    public PlayerAgent Machine;
    public PlayerInput Pause;

    public NNModel EasyModel;
    public NNModel HardModel;
    
    public bool isLeftPlayer;

    // Start is called before the first frame update
    void Awake()
    {
        
        if (isLeftPlayer)
        {
            Human.enabled = !PlayState.isLeftMachine;
            Machine.enabled = PlayState.isLeftMachine;
            if (!PlayState.isLeftMachine && !PlayState.isRightMachine)
            {
                Human.neverAutoSwitchControlSchemes = true;
                Human.defaultControlScheme = "Keyboard";
                Human.SwitchCurrentControlScheme("Keyboard", Keyboard.current);
            }

            if (PlayState.isLeftMachine)
            {
                Machine.SetModel("Volleyball", PlayState.isLeftHard ? HardModel : EasyModel);
            }

            if (PlayState.isLeftMachine && PlayState.isRightMachine)
            {
                Pause.enabled = true;
            }
        }
        else
        {
            Human.enabled = !PlayState.isRightMachine;
            Machine.enabled = PlayState.isRightMachine;
            
            if (PlayState.isRightMachine)
            {
                Machine.SetModel("Volleyball", PlayState.isRightHard ? HardModel : EasyModel);
            }
            
            if (!PlayState.isLeftMachine && !PlayState.isRightMachine)
            {
                Human.neverAutoSwitchControlSchemes = true;
                Human.defaultControlScheme = "Gamepad";
                Human.SwitchCurrentControlScheme("Gamepad", Gamepad.current);
            }
        }
    }
}

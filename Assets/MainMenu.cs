using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public Scene GameScene;
    
    private void Start()
    {
        PlayState.isLeftMachine = true;
        PlayState.isRightMachine = true;
        PlayState.bestTo = 10;
    }

    public void StartGame()
    {
        SceneManager.LoadScene(1);
    }

    public void CloseGame()
    {
        Application.Quit();
    }
    
    public void setLeftPlayer(int value)
    {
        PlayState.isLeftMachine = value == 1;
    }

    public void setRightPlayer(int value)
    {
        PlayState.isRightMachine = value == 1;
    }
}

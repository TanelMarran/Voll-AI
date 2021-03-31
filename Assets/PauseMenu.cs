using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    private bool _isPaused;
    public GameObject Menu;

    public bool IsPaused
    {
        get => _isPaused;
        set
        {
            _isPaused = value;
            Time.timeScale = value ? 0 : 1;
        }
    }

    private void Start()
    {
        ResumeGame();
    }

    public void PauseGame()
    {
        if (!Transition.isPlaying())
        {
            Menu.SetActive(true);
            IsPaused = true;
        }
    }

    public void ResumeGame()
    {
        Menu.SetActive(false);
        IsPaused = false;
    }

    public void ExitGame()
    {
        Time.timeScale = 1;
        Transition.Play(() => SceneManager.LoadScene(0));
    }
}

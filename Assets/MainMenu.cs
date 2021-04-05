using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public Scene GameScene;
    
    private void Start()
    {
        PlayState.isLeftMachine = false;
        PlayState.isRightMachine = true;
        PlayState.isLeftHard = false;
        PlayState.isRightHard = false;
        PlayState.firstTo = 5;
    }

    public void StartGame()
    {
        Transition.Play(() => SceneManager.LoadScene(1));
    }

    public void OpenForm()
    {
        Application.OpenURL("https://forms.gle/aQFxHEEYFftbrocy7");
    }

    public void CloseGame()
    {
        Application.Quit();
    }
    
    public void setLeftPlayer(int value)
    {
        PlayState.isLeftMachine = value != 0;
        PlayState.isLeftHard = value == 2;
    }
    
    public void setWinningScore(int value)
    {
        PlayState.firstTo = value == 0 ? 3 : (value == 1 ? 5 : 10);
    }

    public void setRightPlayer(int value)
    {
        PlayState.isRightMachine = value != 0;
        PlayState.isRightHard = value == 2;
    }

    public void openResultsDirectory()
    {
        string itemPath = Application.persistentDataPath + Path.DirectorySeparatorChar;
        OpenInFileBrowser.Open(itemPath);
        //itemPath = itemPath.Replace(@"/", @"\");   // explorer doesn't like front slashes
        //System.Diagnostics.Process.Start("explorer.exe", "/open," + itemPath);
    }
}

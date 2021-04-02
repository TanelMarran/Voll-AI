using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class PlayState
{
    public static bool isLeftMachine = false;
    public static bool isRightMachine = true;
    public static int firstTo = 5;

    public static string logFile = Application.persistentDataPath + Path.DirectorySeparatorChar + "game_logs.csv";

    public Game Game;

}

using System;
using UnityEngine;

public class GameDebugger : MonoBehaviour
{
    public static int DebugLevel = 1; // Basic Info
    // DebugLevel 2 - Warnings
    // DebugLevel 3 - Errors
    // DebugLevel 4 - Input System
    // DebugLevel 5 - Physics System
    // DebugLevel 6 - Collision System
    // DebugLevel 7 - Particle System
    // DebugLevel 8 - Objects Spawned and Deleted
    // DebugLevel 9 - UI

    void Start()
    {
        ParseCommandLineArguments();
    }

    void ParseCommandLineArguments()
    {
        string[] args = System.Environment.GetCommandLineArgs();
        for (int i = 0; i < args.Length; i++)
        {
            if (args[i].ToLower() == "-gd:" && i + 1 < args.Length)
            {
                int level;
                if (int.TryParse(args[i + 1], out level))
                {
                    DebugLevel = Mathf.Clamp(level, 1, 9);
                    break;
                }
            }
        }
        DebugLog(1, "Debugger initialized.");
        Debug.Log("Debugger Level: " + DebugLevel);
    }

    public static void DebugLog(int level, string message)
    {
        if (level == DebugLevel)
        {
            DebuggerConsole.instance.Log("Debug Level " + level + " [" + DateTime.Now.ToString("HH:mm:ss") + "]: " + message, Color.white);

            if (level == 2 && level == DebugLevel)
            {
                DebuggerConsole.instance.Log("Debug Level " + level + " [" + DateTime.Now.ToString("HH:mm:ss") + "]: " + message, Color.yellow);
            }

            if (level == 3 && level == DebugLevel)
            {
                DebuggerConsole.instance.Log("Debug Level " + level + " [" + DateTime.Now.ToString("HH:mm:ss") + "]: " + message, Color.red);
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameUI : MonoBehaviour
{
    private static GameUI instance;
    [SerializeField] private List<GameObject> UIScreens = new List<GameObject>();

    public static GameUI Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<GameUI>();

                if (instance == null)
                {
                    GameObject singletonObject = new GameObject("GameUI");
                    instance = singletonObject.AddComponent<GameUI>();
                    GameDebugger.DebugLog(2, "No instance of GameUI found in the scene.");
                }
            }
            return instance;
        }
    }

    void Start()
    {
        // Set up game manager.
        if (instance == null)
        {
            // Makes gamemanager persistent.
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            // Prevents duplicate instances of the game manager.
            Destroy(gameObject);
        }
    }

    // Returns a boolean whether the screen is active
    public bool IsScreenActive(string screenName)
    {
        foreach (GameObject screen in UIScreens)
        {
            if (screen.name == screenName)
            {
                GameDebugger.DebugLog(9, "Active Screen: " + screenName);
                return screen.activeSelf;
            }
        }

        GameDebugger.DebugLog(1, "GameUI.cs: Screen " + screenName + " not found!");
        return false;
    }

    // Set a screen to active or inactive
    public void SetIsScreenActive(string screenName, bool isActive)
    {
        foreach (GameObject screen in UIScreens)
        {
            if (screen.name == screenName)
            {
                screen.SetActive(isActive);
                GameDebugger.DebugLog(9, "Set Active Screen: " + screenName);
                return;
            }
        }

        GameDebugger.DebugLog(1, "GameUI.cs: Screen " + screenName + " not found!");
    }

    // Set all screens to active or inactive
    public void SetAllScreensActive(bool isActive)
    {
        foreach (GameObject screen in UIScreens)
        {
            screen.SetActive(isActive);
        }
        GameDebugger.DebugLog(9, "All Screens are Active");
    }

    // Pause / Unpause game. -- For resume button
    public void Pause()
    {
        GameManager.Instance.Pause();
    }

    // Exits the game. -- For quit button
    public void Quit()
    {
        GameManager.Instance.Quit();
    }
}

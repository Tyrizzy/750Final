using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    [HideInInspector] public bool isPaused = false, isPlayerDead = false, isPlayerWon = false;
    [HideInInspector] public float PipeSpeed, CameraSpeed, SpawnTime;
    [HideInInspector] public Vector3 EndPoint;

    [Header("First Selected Menu Items")]
    [SerializeField] GameObject StartScreen;
    [SerializeField] GameObject LevelScreen; 
    [SerializeField] GameObject ControlsScreen; 
    [SerializeField] GameObject PauseScreen;
    [SerializeField] GameObject DeathScreen;
    [SerializeField] GameObject WinScreen;
    [SerializeField] GameObject EndConfirmationScreen; 

    private enum Modes { Easy, Medium, Hard, UnitTest };
    Modes modes;

    public static GameManager Instance
    {
        get 
        {
            if (instance == null)
            {
                instance = FindObjectOfType<GameManager>();

                if (instance == null)
                {
                    GameObject singletonObject = new GameObject("GameManager");
                    instance = singletonObject.AddComponent<GameManager>();
                    GameDebugger.DebugLog(2, "No instance of GameManager found in the scene.");
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
        CurrentScreen();
    }

    void Update()
    {
        switch (modes)
        {
            case Modes.Easy:
                PipeSpeed = 3;
                CameraSpeed = 3;
                SpawnTime = 3;
                EndPoint = new Vector3(50,0,0);
                break;

            case Modes.Medium:
                PipeSpeed = 10;
                CameraSpeed = 3;
                SpawnTime = 1f;
                EndPoint = new Vector3(75,0,0);
                break;

            case Modes.Hard:
                PipeSpeed = 20;
                CameraSpeed = 5;
                SpawnTime = 1;
                EndPoint = new Vector3(100,0,0);
                break;
            default:
                break;
        }

        // Pause / Unpause game when escape is pressed.
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Pause();
        }

        if (isPlayerDead)
        {
            Death();
        }

        if (isPlayerWon)
        {
            Win();
        }
    }

    public void Pause()
    {
        // Don't do in start menu and Death screen
        if (GameUI.Instance.IsScreenActive("Start Screen") || GameUI.Instance.IsScreenActive("Death Screen")) return;

        isPaused = !isPaused;

        // Pause logic
        if (isPaused)
        {
            // Set Pause Screen Active
            GameUI.Instance.SetIsScreenActive("Pause Screen", true);
            CurrentScreen();
            Time.timeScale = 0f;
        }
        // Unpause logic
        else if (!isPaused)
        {
            // Set all Screen Inactive (Settings, Options, Pause, etc.)
            GameUI.Instance.SetAllScreensActive(false);
            GameUI.Instance.SetIsScreenActive("Player UI", true);
            Time.timeScale = 1f;
        }
    }

    public void Death()
    {
        // Don't do in start menu
        if (GameUI.Instance.IsScreenActive("Start Screen")) return;

        if (isPlayerDead)
        {
            // Set Death Screen Active
            GameUI.Instance.SetIsScreenActive("Death Screen", true);
            CurrentScreen();
            isPlayerDead = false;
            Time.timeScale = 0f;
        }
    }

    public void Win()
    {
        // Don't do in start menu
        if (GameUI.Instance.IsScreenActive("Start Screen")) return;

        if (isPlayerWon)
        {
            // Set Win Screen Active
            GameUI.Instance.SetIsScreenActive("Win Screen", true);
            CurrentScreen();
            isPlayerWon = false;
            Time.timeScale = 0f;
        }
    }

    public void MainMenu()
    {
        // Don't do in start menu
        if (GameUI.Instance.IsScreenActive("Start Screen")) return;

        Time.timeScale = 1f;
        GameUI.Instance.SetAllScreensActive(false);
        GameUI.Instance.SetIsScreenActive("Start Screen", true);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
        CurrentScreen();
        GameDebugger.DebugLog(1, "Loading Main Menu");
    }

    public void Restart()
    {
        // Don't do in start menu
        if (GameUI.Instance.IsScreenActive("Start Screen")) return;

        Time.timeScale = 1f;
        GameUI.Instance.SetAllScreensActive(false);
        GameUI.Instance.SetIsScreenActive("Player UI", true);
        GameDebugger.DebugLog(1, "Reloading Scene");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void Quit()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
            GameDebugger.DebugLog(1, "Quiting Game");
        #else
            GameDebugger.DebugLog(1, "Quiting Game");
            Application.Quit();
        #endif
    }

    public void EasyMode()
    {
        modes = Modes.Easy;
        GameUI.Instance.SetIsScreenActive("Level Screen", false);
        GameUI.Instance.SetIsScreenActive("Player UI", true);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        GameDebugger.DebugLog(1, "Loading EasyMode");
    }

    public void MediumMode()
    {
        modes = Modes.Medium;
        GameUI.Instance.SetIsScreenActive("Level Screen", false);
        GameUI.Instance.SetIsScreenActive("Player UI", true);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        GameDebugger.DebugLog(1, "Loading MediumMode");
    }

    public void HardMode()
    {
        modes = Modes.Hard;
        GameUI.Instance.SetIsScreenActive("Level Screen", false);
        GameUI.Instance.SetIsScreenActive("Player UI", true);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        GameDebugger.DebugLog(1, "Loading HardMode");
    }

    public void CurrentScreen()
    {
        if (GameUI.Instance.IsScreenActive("Start Screen"))
        {
            EventSystem.current.SetSelectedGameObject(StartScreen);
        }

        else if (GameUI.Instance.IsScreenActive("Level Screen"))
        {
            EventSystem.current.SetSelectedGameObject(LevelScreen);
        }

        else if (GameUI.Instance.IsScreenActive("Controls Screen"))
        {
            EventSystem.current.SetSelectedGameObject(ControlsScreen);
        }

        else if (GameUI.Instance.IsScreenActive("Pause Screen"))
        {
            EventSystem.current.SetSelectedGameObject(PauseScreen);
        }

        else if (GameUI.Instance.IsScreenActive("Death Screen"))
        {
            EventSystem.current.SetSelectedGameObject(DeathScreen);
        }

        else if (GameUI.Instance.IsScreenActive("Win Screen"))
        {
            EventSystem.current.SetSelectedGameObject(WinScreen);
        }

        else if (GameUI.Instance.IsScreenActive("End Confirmation Screen"))
        {
            EventSystem.current.SetSelectedGameObject(EndConfirmationScreen);
        }
    }
}

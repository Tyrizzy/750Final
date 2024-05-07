using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public class SpawnEndPoint : MonoBehaviour
{
    GameManager gameManager;
    GameObject Player;
    TMP_Text DistUI;

    private void Awake()
    {
        gameManager = FindAnyObjectByType<GameManager>();
        DistUI = GameObject.Find("DistanceText").GetComponent<TMP_Text>();
        Player = GameObject.Find("Player");
    }

    private void Start()
    {
        this.transform.position = gameManager.EndPoint;
        GameDebugger.DebugLog(8, "End Goal Spawned");
    }

    private void Update()
    {
        if (!gameManager.isPlayerDead)
        {
            DistUI.text = "Distance To Finish: " + Vector2.Distance(this.transform.position, Player.transform.position).ToString("F0");

            if (Player.transform.position.x >= this.transform.position.x)
            {
                gameManager.isPlayerWon = true;
                DestroyImmediate(Player);
            }
        }
    }
}

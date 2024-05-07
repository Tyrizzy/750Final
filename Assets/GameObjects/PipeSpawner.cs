using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PipeSpawner : MonoBehaviour
{
    public GameObject objectToSpawn;
    private Camera mainCamera;
    GameManager gameManager;
    public float spawnInterval;

    private void Awake()
    {
        gameManager = FindAnyObjectByType<GameManager>();
    }

    void Start()
    {
        mainCamera = Camera.main; // Cache the main camera
        InvokeRepeating("SpawnOnBounds", .5f, gameManager.SpawnTime);
        GameDebugger.DebugLog(8, "Start Pipe Spawner");
    }

    void SpawnOnBounds()
    {
        Vector3 spawnPos = Vector3.zero;
        float randomY = Random.Range(.2f, .8f); // Random x-coordinate in viewport space

        // Randomly choose top or bottom edge
        if (Random.value > 0.3f)
        {
            // Top edge
            spawnPos = mainCamera.ViewportToWorldPoint(new Vector3(1, randomY, mainCamera.nearClipPlane));
        }
        else
        {
            // Bottom edge
            spawnPos = mainCamera.ViewportToWorldPoint(new Vector3(1, randomY, mainCamera.nearClipPlane));
        }

        // Ensure the object spawns facing forward relative to the camera
        Quaternion spawnRotation = Quaternion.LookRotation(mainCamera.transform.forward);

        // Instantiate the object at the calculated position
        Instantiate(objectToSpawn, spawnPos, spawnRotation);
        GameDebugger.DebugLog(8, "Pipes Being Spawned Every " + gameManager.SpawnTime + " Seconds at Postiton" + spawnPos);
    }
}

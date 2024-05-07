using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    GameManager gameManager;

    private void Start()
    {
        gameManager = FindAnyObjectByType<GameManager>();
    }

    private void FixedUpdate()
    {
        transform.position = new Vector3(transform.position.x + gameManager.CameraSpeed * Time.deltaTime, transform.position.y, transform.position.z);
        GameDebugger.DebugLog(5, "Camera is Moving: " + transform.position);
    }
}

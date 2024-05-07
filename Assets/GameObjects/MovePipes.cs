using UnityEngine;

public class MovePipes : MonoBehaviour
{
    private Camera mainCamera;
    Vector3 EndPosX;
    GameManager gameManager;
     
    private void Awake()
    {
        mainCamera = Camera.main;
        gameManager = FindAnyObjectByType<GameManager>();
    }

    void FixedUpdate()
    {
        transform.position = new Vector2(transform.position.x - gameManager.PipeSpeed * Time.deltaTime, transform.position.y);
        EndPosX = mainCamera.ViewportToWorldPoint(new Vector3(0, 1, mainCamera.nearClipPlane));

        if (transform.position.x < EndPosX.x)
        {
            Destroy(gameObject);
        }
    }
}

using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    private float Horizontal;
    [SerializeField] private float Speed = 5f;
    [SerializeField] private float JumpPower = 5f;
    private bool isFacingRight = true;

    private Camera mainCamera;
    PhysicsController physicsController;
    ParticleSys particleSys;
    GameManager gameManager;

    private void Start()
    {
        physicsController = GetComponent<PhysicsController>();
        particleSys = FindAnyObjectByType<ParticleSys>();
        gameManager = FindAnyObjectByType<GameManager>();
        mainCamera = Camera.main;
    }

    private void FixedUpdate()
    {
        // Movement Calcs
        physicsController.velocity = new Vector2 (Horizontal * Speed, physicsController.velocity.y);
        GameDebugger.DebugLog(5, "Current Movement: " + physicsController.velocity);

        // Detects the Camerea Bounds in a janky way but it works
        Vector3 CamBoundsPos = mainCamera.ViewportToWorldPoint(new Vector3(1, 0, mainCamera.nearClipPlane));
        Vector3 CamBoundsNeg = mainCamera.ViewportToWorldPoint(new Vector3(0, 1, mainCamera.nearClipPlane));

        if (transform.position.x < CamBoundsNeg.x || transform.position.x > CamBoundsPos.x)
        {
            Destroy(gameObject);
            gameManager.isPlayerDead = true;
            GameDebugger.DebugLog(6, "Player has Collided with the Cameras Bounds");
        }
        else if (transform.position.y < -CamBoundsNeg.y || transform.position.y > CamBoundsNeg.y)
        {
            Destroy(gameObject);
            gameManager.isPlayerDead = true;
            GameDebugger.DebugLog(6, "Player has Collided with the Cameras Bounds");
        }
        particleSys.SpawnParticle();
    }

    void Update()
    {
        if (isFacingRight && Horizontal > 0f)
        {
            Flip();
        }
        else if (isFacingRight && Horizontal < 0f)
        {
            Flip();
        }
    }
    public void Jump(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            physicsController.velocity = new Vector2(physicsController.velocity.x, JumpPower * 2);
            GameDebugger.DebugLog(4, "Player Jumped: " + physicsController.velocity);
            //particleSys.SpawnParticle();
        }
    }

    private void Flip()
    {
        isFacingRight = !isFacingRight;
        Vector3 localScale = transform.localScale;
        localScale.x *= -1f;
        transform.localScale = localScale;
    }

    public void Move(InputAction.CallbackContext context)
    {
        Horizontal = context.ReadValue<Vector2>().x;
        GameDebugger.DebugLog(4, "Player Moved: " + Horizontal);
    }

    public void Pause(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            gameManager.Pause();
            GameDebugger.DebugLog(9, "Player Paused: " + Horizontal);
        }
    }
}

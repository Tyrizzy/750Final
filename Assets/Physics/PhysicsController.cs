using UnityEngine;
using UnityEngine.SceneManagement;

public class PhysicsController : MonoBehaviour
{
    [Range(0, 5)] public float mass = 1f;
    [Range(0, 10)] public float linearDrag = 0f;
    public bool useGravity = false;
    public bool isStatic = true;

    [HideInInspector] public Vector3 velocity;

    private void FixedUpdate()
    {
        if (!isStatic) // Check if the object is not static
        {
            ApplyGravity();

            // Apply linear drag
            velocity.x -= velocity.x * linearDrag * Time.deltaTime;
            GameDebugger.DebugLog(5, "Applying Linear Drag of: " + linearDrag);
            GameDebugger.DebugLog(5, "Current Velocity After Applying Linear Drag: " + velocity.x);

            // Update position based on velocity
            transform.position += velocity * Time.deltaTime;
            GameDebugger.DebugLog(5, "Updated Position: " + transform.position);
        }
    }

    private void ApplyGravity()
    {
        if (useGravity)
        {
            velocity += Physics.gravity * mass * Time.deltaTime;
            GameDebugger.DebugLog(5, "Activating Gravity: " + useGravity);
        }
        else
        {
            velocity = Vector3.zero;
            GameDebugger.DebugLog(5, "Deactivating Gravity: " + useGravity);
        }
    }
}

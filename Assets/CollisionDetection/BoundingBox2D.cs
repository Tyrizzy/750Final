using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(PhysicsController))]
public class BoundingBox2D : MonoBehaviour
{
    public Color boundingColor = Color.red;
    public bool showBoundingBox = true;
    private SpriteRenderer spriteRenderer;
    [HideInInspector] public PhysicsController physicsController;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        physicsController = GetComponent<PhysicsController>();
    }

    private void OnDrawGizmosSelected()
    {
        if (showBoundingBox && spriteRenderer != null && spriteRenderer.sprite != null)
        {
            Vector3[] corners = GetWorldCorners();
            Gizmos.color = boundingColor;
            for (int i = 0; i < corners.Length; i++)
            {
                Gizmos.DrawLine(corners[i], corners[(i + 1) % corners.Length]);
            }
        }
    }

    public Vector3[] GetWorldCorners()
    {
        Vector2[] vertices = spriteRenderer.sprite.vertices;
        Vector2 min = new Vector2(float.MaxValue, float.MaxValue);
        Vector2 max = new Vector2(float.MinValue, float.MinValue);

        foreach (Vector2 vertex in vertices)
        {
            min = Vector2.Min(min, vertex);
            max = Vector2.Max(max, vertex);
        }

        Vector3 center = (min + max) / 2;
        Vector3 size = max - min;
        Matrix4x4 matrix = transform.localToWorldMatrix;

        Vector3[] corners = new Vector3[4];
        corners[0] = new Vector3(min.x, min.y, 0);
        corners[1] = new Vector3(max.x, min.y, 0);
        corners[2] = new Vector3(max.x, max.y, 0);
        corners[3] = new Vector3(min.x, max.y, 0);

        for (int i = 0; i < corners.Length; i++)
        {
            corners[i] = matrix.MultiplyPoint3x4(corners[i]);
        }

        return corners;
    }

    public Vector3 GetSize()
    {
        Vector3[] corners = GetWorldCorners();
        Vector3 min = corners[0];
        Vector3 max = corners[2];
        Vector3 size = new Vector3(Mathf.Abs(max.x - min.x), Mathf.Abs(max.y - min.y), Mathf.Abs(max.z - min.z));
        return size;
    }
}

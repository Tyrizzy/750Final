using System.Collections.Generic;
using UnityEngine;

public class CollisionDetectionQuadTree2D : MonoBehaviour
{
    public Rect quadTreeBounds;
    private QuadTree quadTree;
    GameManager gameManager;

    void Start()
    {
        quadTree = new QuadTree(0, quadTreeBounds);
        gameManager = FindAnyObjectByType<GameManager>();
    }

    void Update()
    {
        quadTree.Clear();

        var allBoundingBoxes = FindObjectsOfType<BoundingBox2D>();
        foreach (var box in allBoundingBoxes)
        {
            quadTree.Insert(box);
        }

        foreach (var box in allBoundingBoxes)
        {
            List<BoundingBox2D> returnObjects = new List<BoundingBox2D>();
            quadTree.Retrieve(returnObjects, box);

            foreach (var otherBox in returnObjects)
            {
                if (otherBox != box && SATCollisionCheck(box, otherBox, out Vector3 mtv))
                {
                    if (box.gameObject.CompareTag("Player"))
                    {
                        Destroy(box.gameObject);
                        gameManager.isPlayerDead = true;
                        GameDebugger.DebugLog(6, $"Collision detected between {box.gameObject.name} and {otherBox.gameObject.name}");
                    }
                    else if (otherBox.gameObject.CompareTag("Player"))
                    {
                        Destroy(otherBox.gameObject);
                        gameManager.isPlayerDead = true;
                        GameDebugger.DebugLog(6, $"Collision detected between {box.gameObject.name} and {otherBox.gameObject.name}");
                    }
                    else
                    {
                        // Resolve the collision using MTV
                        ResolveCollision(box, otherBox, mtv);
                    }
                }
            }
        }
    }

    private void ResolveCollision(BoundingBox2D box1, BoundingBox2D box2, Vector3 mtv)
    {
        if (box1.physicsController.isStatic && box2.physicsController.isStatic)
            return;  // Neither object should move if both are static.

        // Apply a more aggressive position correction
        Vector3 correction = mtv * 1.1f;  // Over-correct to ensure separation

        // Separate this into more manageable parts if both are dynamic
        if (!box1.physicsController.isStatic && !box2.physicsController.isStatic)
        {
            box1.transform.position += correction * 0.5f;
            box2.transform.position -= correction * 0.5f;
        }
        else if (box1.physicsController.isStatic && !box2.physicsController.isStatic)
        {
            box1.physicsController.velocity = Vector3.zero;

            box2.transform.position -= correction * .5f;
        }
        else if (!box1.physicsController.isStatic && box2.physicsController.isStatic)
        {
            box2.physicsController.velocity = Vector3.zero;

            box1.transform.position += correction * .5f;
        }

        // Calculate relative velocity
        Vector3 relativeVelocity = box1.physicsController.velocity - box2.physicsController.velocity;

        // Calculate relative normal velocity (dot product of relative velocity and MTV)
        float relativeNormalVelocity = Vector3.Dot(relativeVelocity, mtv.normalized);

        // Calculate impulse magnitude using coefficient of restitution and mass
        float e = .8f; // coefficient of restitution (adjust as needed)
        float impulseMagnitude = -(1 + e) * relativeNormalVelocity / (1 / box1.physicsController.mass + 1 / box2.physicsController.mass);

        // Apply impulse to change velocity
        box1.physicsController.velocity += impulseMagnitude * mtv.normalized / box1.physicsController.mass;
        box2.physicsController.velocity -= impulseMagnitude * mtv.normalized / box2.physicsController.mass;
    }

    private bool SATCollisionCheck(BoundingBox2D box1, BoundingBox2D box2, out Vector3 mtv)
    {
        Vector3[] corners1 = box1.GetWorldCorners();
        Vector3[] corners2 = box2.GetWorldCorners();

        // We need to test the axes of both polygons (rectangles here)
        Vector3[] axesToTest = {
        corners1[1] - corners1[0],
        corners1[3] - corners1[0],
        corners2[1] - corners2[0],
        corners2[3] - corners2[0]
    };

        float minOverlap = float.MaxValue;
        mtv = Vector3.zero;

        foreach (Vector3 axis in axesToTest)
        {
            if (!OverlapOnAxis(corners1, corners2, axis))
            {
                return false; // No collision if there's a gap on any axis
            }

            // Calculate overlap along the axis
            float overlap = CalculateOverlap(corners1, corners2, axis);
            if (overlap < minOverlap)
            {
                minOverlap = overlap;
                mtv = axis.normalized * overlap; // MTV points in the direction of separation
            }
        }

        return true; // Collision if no gaps found on all axes
    }

    private bool OverlapOnAxis(Vector3[] corners1, Vector3[] corners2, Vector3 axis)
    {
        // Project all corners onto the axis and get the minimum and maximum values
        float min1, max1, min2, max2;
        ProjectPolygonOnAxis(corners1, axis, out min1, out max1);
        ProjectPolygonOnAxis(corners2, axis, out min2, out max2);

        // Check if there is a gap
        if (min1 > max2 || min2 > max1)
        {
            return false; // No overlap means a separating axis exists
        }

        return true; // Overlap found
    }

    private float CalculateOverlap(Vector3[] corners1, Vector3[] corners2, Vector3 axis)
    {
        float min1, max1, min2, max2;
        ProjectPolygonOnAxis(corners1, axis, out min1, out max1);
        ProjectPolygonOnAxis(corners2, axis, out min2, out max2);

        if (min1 > max2 || min2 > max1)
        {
            return 0.0f; // No overlap
        }

        // Calculate overlap amount
        float overlap1 = max2 - min1;
        float overlap2 = max1 - min2;
        return Mathf.Min(overlap1, overlap2);
    }

    private void ProjectPolygonOnAxis(Vector3[] corners, Vector3 axis, out float min, out float max)
    {
        min = Vector3.Dot(axis, corners[0]);
        max = min;

        for (int i = 1; i < corners.Length; i++)
        {
            float projection = Vector3.Dot(axis, corners[i]);
            if (projection < min)
            {
                min = projection;
            }
            if (projection > max)
            {
                max = projection;
            }
        }
    }

    private void PositionalCorrection(BoundingBox2D box1, BoundingBox2D box2, float penetrationDepth, Vector3 correctionAxis)
    {
        float positionalCorrectionFactor = 0.5f; // Adjust this value as necessary
        float penetrationAllowance = 0.01f; // Allow a small amount of penetration to avoid jitter
        float correctionMagnitude = (penetrationDepth - penetrationAllowance) * positionalCorrectionFactor;
        Vector3 correction = correctionAxis.normalized * correctionMagnitude;

        if (!box1.physicsController.isStatic)
            box1.transform.position += correction * (box2.physicsController.mass / (box1.physicsController.mass + box2.physicsController.mass));
        if (!box2.physicsController.isStatic)
            box2.transform.position -= correction * (box1.physicsController.mass / (box1.physicsController.mass + box2.physicsController.mass));
    }

    private void OnDrawGizmosSelected()
    {
        Debug.Log("OnDrawGizmos called");
        if (quadTree != null)
        {
            Gizmos.color = Color.green; // Set Gizmos color
            quadTree.DrawBounds(quadTreeBounds);
        }
    }
}

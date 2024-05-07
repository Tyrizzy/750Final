using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuadTree
{
    private int maxObjects = 10;
    private int maxLevels = 5;

    private int level;
    private List<BoundingBox2D> objects;
    private Rect bounds;
    private QuadTree[] nodes;

    public QuadTree(int pLevel, Rect pBounds)
    {
        level = pLevel;
        objects = new List<BoundingBox2D>();
        bounds = pBounds;
        nodes = new QuadTree[4];
    }

    public void Clear()
    {
        objects.Clear();

        for (int i = 0; i < nodes.Length; i++)
        {
            if (nodes[i] != null)
            {
                nodes[i].Clear();
                nodes[i] = null;
            }
        }
    }

    private void Split()
    {
        float subWidth = bounds.width / 2;
        float subHeight = bounds.height / 2;
        float x = bounds.x;
        float y = bounds.y;

        nodes[0] = new QuadTree(level + 1, new Rect(x + subWidth, y, subWidth, subHeight));
        nodes[1] = new QuadTree(level + 1, new Rect(x, y, subWidth, subHeight));
        nodes[2] = new QuadTree(level + 1, new Rect(x, y + subHeight, subWidth, subHeight));
        nodes[3] = new QuadTree(level + 1, new Rect(x + subWidth, y + subHeight, subWidth, subHeight));
    }

    private int GetIndex(BoundingBox2D box)
    {
        int index = -1;
        float verticalMidpoint = bounds.x + (bounds.width / 2);
        float horizontalMidpoint = bounds.y + (bounds.height / 2);

        // Object can completely fit within the top quadrants
        bool topQuadrant = (box.GetWorldCorners()[0].y < horizontalMidpoint && box.GetWorldCorners()[3].y < horizontalMidpoint);
        // Object can completely fit within the bottom quadrants
        bool bottomQuadrant = (box.GetWorldCorners()[0].y > horizontalMidpoint);

        // Object can completely fit within the left quadrants
        if (box.GetWorldCorners()[0].x < verticalMidpoint && box.GetWorldCorners()[1].x < verticalMidpoint)
        {
            if (topQuadrant)
            {
                index = 1;
            }
            else if (bottomQuadrant)
            {
                index = 2;
            }
        }
        // Object can completely fit within the right quadrants
        else if (box.GetWorldCorners()[0].x > verticalMidpoint)
        {
            if (topQuadrant)
            {
                index = 0;
            }
            else if (bottomQuadrant)
            {
                index = 3;
            }
        }

        return index;
    }

    public void Insert(BoundingBox2D box)
    {
        if (nodes[0] != null)
        {
            int index = GetIndex(box);

            if (index != -1)
            {
                nodes[index].Insert(box);
                return;
            }
        }

        objects.Add(box);

        if (objects.Count > maxObjects && level < maxLevels)
        {
            if (nodes[0] == null)
            {
                Split();
            }

            int i = 0;
            while (i < objects.Count)
            {
                int index = GetIndex(objects[i]);
                if (index != -1)
                {
                    nodes[index].Insert(objects[i]);
                    objects.RemoveAt(i);
                }
                else
                {
                    i++;
                }
            }
        }
    }

    public List<BoundingBox2D> Retrieve(List<BoundingBox2D> returnObjects, BoundingBox2D box)
    {
        int index = GetIndex(box);
        if (index != -1 && nodes[0] != null)
        {
            nodes[index].Retrieve(returnObjects, box);
        }

        returnObjects.AddRange(objects);

        return returnObjects;
    }

    public void DrawBounds(Rect bounds)
    {
        Gizmos.color = Color.white;
        Gizmos.DrawLine(new Vector3(bounds.x, bounds.y), new Vector3(bounds.x + bounds.width, bounds.y));
        Gizmos.DrawLine(new Vector3(bounds.x + bounds.width, bounds.y), new Vector3(bounds.x + bounds.width, bounds.y + bounds.height));
        Gizmos.DrawLine(new Vector3(bounds.x + bounds.width, bounds.y + bounds.height), new Vector3(bounds.x, bounds.y + bounds.height));
        Gizmos.DrawLine(new Vector3(bounds.x, bounds.y + bounds.height), new Vector3(bounds.x, bounds.y));

        if (nodes[0] != null)
        {
            for (int i = 0; i < nodes.Length; i++)
            {
                nodes[i].DrawBounds(bounds);
            }
        }
    }
}


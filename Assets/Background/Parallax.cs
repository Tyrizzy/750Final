using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    private float Length, StartPos;
    [SerializeField] GameObject cam;
    [SerializeField] float ParrallaxEffect;

    private void Start()
    {
        StartPos = transform.position.x;
        Length = GetComponent<SpriteRenderer>().bounds.size.x;
    }

    private void FixedUpdate()
    {
        float temp = cam.transform.position.x * (1 - ParrallaxEffect);
        float dist = cam.transform.position.x * ParrallaxEffect;

        transform.position = new Vector3(StartPos + dist, transform.position.y, transform.position.z);

        if (temp > StartPos + Length)
        {
            StartPos += Length;
        }
        else if (temp < StartPos - Length) 
        {
            StartPos -= Length;
        }
    }
}

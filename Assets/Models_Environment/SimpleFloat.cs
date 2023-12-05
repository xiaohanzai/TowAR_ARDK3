using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuickFloat : MonoBehaviour
{
    [SerializeField]
    private float speed = 5f;

    [SerializeField]
    private float height = 0.5f;

    private float initialY;

    void Start()
    {
        // Store the initial Y position
        initialY = transform.position.y;
    }

    void Update()
    {
        // Calculate the new Y position based on a sine wave
        float updateY = Mathf.Sin(Time.time * speed);

        // Update the Y position around the initial Y position
        transform.position = new Vector3(transform.position.x, initialY + updateY * height, transform.position.z);
    }
}
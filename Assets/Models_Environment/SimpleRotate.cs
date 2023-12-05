using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleRotate : MonoBehaviour
{
    [SerializeField]

    private Vector3 roatateValues;
    private float rotateSpeed = 5;


    // Update is called once per frame
    void Update()
    {
        transform.Rotate(roatateValues * Time.deltaTime * rotateSpeed);

    }
}

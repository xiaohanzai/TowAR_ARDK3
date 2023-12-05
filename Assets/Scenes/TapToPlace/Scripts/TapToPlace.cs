using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class TapToPlace : MonoBehaviour
{
    public GameObject objectToPlace;
    private GameObject placedObject;

    // Update is called once per frame
    void Update()
    {
        if (Input.touchCount > 0 && placedObject == null)
        {
            // Raycast from the touch position
            Ray ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                // Print the position on the plane that the ray hits
                placedObject = Instantiate(objectToPlace, hit.transform.position, Quaternion.identity);
                Debug.Log("Hit Position: " + hit.point);
            }
        }
    }
}

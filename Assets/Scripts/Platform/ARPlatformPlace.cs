using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;


public class ARPlatformPlace : MonoBehaviour
{
    public GameObject objectToPlace;
    private ARRaycastManager arRaycastManager;
    private Vector2 touchPosition;
    public GameObject semanticController;
    public GameObject factorySimPrefab;
    public GameObject materialDome;


    void Start()
    {
        arRaycastManager = FindObjectOfType<ARRaycastManager>();
    }

    void Update()
    {

        if (Input.GetMouseButtonDown(0) || Input.GetMouseButton(0) || Input.GetMouseButtonUp(0))
        {
            // If using mouse, set the mouse position
            touchPosition = Input.mousePosition;

            // Perform AR raycast to check for planes
            Ray ray = Camera.main.ScreenPointToRay(touchPosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, Mathf.Infinity))
            {
                // Instantiate the object at the hit position
                Instantiate(objectToPlace, hit.point, Quaternion.identity);

                // Enable semanticController and factorySimPrefab, and disable ARPlatformPlace
                semanticController.SetActive(true);
                factorySimPrefab.SetActive(true);
                materialDome.SetActive(true);
                gameObject.SetActive(false); // Disable ARPlatformPlace
            }
        }
    }
}

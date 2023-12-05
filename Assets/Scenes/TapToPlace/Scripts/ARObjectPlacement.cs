using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class ARObjectPlacement : MonoBehaviour
{
    public GameObject objectToPlace;
    private ARRaycastManager arRaycastManager;
    private Vector2 touchPosition;

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
            }
        }
    }
}

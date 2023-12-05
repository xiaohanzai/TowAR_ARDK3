using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InstantiateDomeAndGrid : MonoBehaviour
{
    public GameObject materialDome;
    public GameObject factorySimElements;

    private bool isPlaced;
    private Vector3 pos;

    public GameObject triggerObject;
    public List<GameObject> thingsToSetActive;
    public List<GameObject> thingsToSetInactive;
    public AudioSource audioSource;

    private void Start()
    {
        isPlaced = false;
    }

    //private void Update()
    //{
    //    if (!isPlaced && triggerObject != null && triggerObject.activeInHierarchy)
    //    {
    //        if (Input.GetMouseButtonDown(0))
    //        {
    //            // Raycast from the touch position
    //            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
    //            RaycastHit hit;

    //            if (Physics.Raycast(ray, out hit))
    //            {
    //                pos = hit.transform.position;
    //                InstantiateThings();
    //                isPlaced = true;

    //                // start game
    //                materialDome.GetComponent<MaterialDome>().DomeOn();
    //                audioSource.Play();
    //                Debug.Log("Hello world!");
    //                foreach (var gameObject in thingsToSetActive)
    //                {
    //                    gameObject.SetActive(true);
    //                    Debug.Log("hello ");
    //                }
    //                foreach (var gameObject in thingsToSetInactive)
    //                {
    //                    gameObject.SetActive(false);
    //                    Debug.Log("world ");
    //                }
    //            }
    //        }
    //    }
    //}

    public void InstantiateThings()
    {
        Debug.Log(Camera.main.transform.position);
        pos = Camera.main.transform.position + Vector3.down * 1.5f;
        materialDome.GetComponent<MaterialDome>().SetPosition(pos + Vector3.down * 0.1f);
        factorySimElements.transform.position = pos;
        GridBuildingSystem.Instance.SetGridOriginPosition(pos);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SystemCompleteButton : MonoBehaviour
{
    public GameObject systemComplete; 
    public void ONButtonClick()
    {
        //check if prefab is assigned.
        if(systemComplete != null)
        {
            Instantiate(systemComplete); 
        }
        else
        {
            Debug.LogError("Prefab not assigned!");
        }
    }
}

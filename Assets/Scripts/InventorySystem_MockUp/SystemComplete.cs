using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SystemComplete : MonoBehaviour
{
    CO2Meter co2Meter;
    void Start()
    {
        //gets a ref to the co2Meter
        co2Meter = FindObjectOfType<CO2Meter>();
        
        //calls the RemoveCO2OverTime function every second.
        InvokeRepeating("RemoveCO2", 1f, 1f);
    }

    private void RemoveCO2()
    {
        co2Meter.RemoveCO2OverTime();
        Debug.Log("Remove Co2 Run");
    }
}

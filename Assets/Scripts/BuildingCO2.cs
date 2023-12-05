using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingCO2 : MonoBehaviour
{
    [SerializeField] float co2RemovalAmount = 5f;
    [SerializeField] float co2Cost = 1f;
    [SerializeField] int nonRenewCost = 1;
    [SerializeField] int bioRenewCost = 5;

    //sets the CO2Meter as a local variable
    CO2Meter co2Meter;

    void Start()
    {
        //gets the reference to CO2Meter
        co2Meter = FindObjectOfType<CO2Meter>();
       // AddPollution();
    }

    public void AddPollution()
    {
        //to guard against calling function when no CO2meter is present
        if (co2Meter == null) { return; }

        //tells the CO2Meter to run the AddPollution method and add to the amount of CO2
        co2Meter.AddCO2(co2Cost);
    }

    //****NOT currently needed
    public void RemovePollution()
    {
        if (co2Meter == null) { return; }

        //tells the CO2 Meter to remove the amount of CO2 specified 
        co2Meter.RemoveCO2(co2RemovalAmount);
    }


}

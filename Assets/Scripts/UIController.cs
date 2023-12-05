using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class UIController : MonoBehaviour
{
    public void DemolishBtnPressed()
    {
        if (GridBuildingSystem.Instance != null)
        {
            GridBuildingSystem.Instance.SetDemolishActive();
        }
    }

    public void RotateBtnPressed()
    {
        if (GridBuildingSystem.Instance != null)
        {
            GridBuildingSystem.Instance.HandleDirRotation(true);
        }
    }

    public void BuildBtnPressed()
    {
        if (GridBuildingSystem.Instance != null)
        {
            GridBuildingSystem.Instance.SetBuildActive();
        }
    }
}
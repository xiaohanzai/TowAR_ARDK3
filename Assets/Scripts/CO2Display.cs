using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CO2Display : MonoBehaviour
{
    float score;
    TMP_Text amountText;

    CO2Meter co2Meter;

    private void Start()
    {
        amountText = GetComponent<TMP_Text>();

        amountText.text = "start";

        co2Meter = FindObjectOfType<CO2Meter>();
    }

    private void Update()
    {
        score = co2Meter.co2Amount;
        amountText.text = score.ToString();
    }

}

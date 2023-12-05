using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class CO2Meter : MonoBehaviour
{
    //get a ref to the CO2 display image
    public Image co2DisplayImage;
    //set the starting co2Amount in the inspector
    public float co2Amount = 33f;
    //set the CO2 level to win in inspector
    public float winAmount = 10f;
    //set the CO2 level to lose in inspector
    public float lossAmount = 99f;

    //sets the CO2 cost to place with nonrenewable ***temp solution
    public float co2Cost = 2f;
    public float co2RemovalAmount = 1f;

    //set speed of adding or removing CO2
    public float addCO2OverTimeAount = 2f;
    public float removeCO2OverTimeAmount = 1.5f;

    //win and loss buttons
    public GameObject winCanvas;
    public GameObject lossCanvas;

    //win and loss animations
    public GameObject winScenario;
    public GameObject lossScenario; 

    //get a reference to the text field 
    [SerializeField] TextMeshProUGUI displayAmount;

    // Declare ParticleSystem variable
    public ParticleSystem particleSystemEditor;
    public ParticleSystem particleSystemAR;
    public float particleSystemUpperRange = 1000f;

    //get reference to the MaterialDome Script
    private MaterialDome materialDome;

    //adds CO2 to the count over time
    private void Start()
    {
        //calls the AddCO2OverTime function every second.
        InvokeRepeating("AddCO2OverTime", 1f, 1f);

        //get local reference to Material Dome
        materialDome = FindObjectOfType<MaterialDome>();
    }

    private void Update()
    {
        updateDisplay();
        
        //check for lose condition
        if (co2Amount >= lossAmount)
        {
            //lose game;
            lossCanvas.SetActive(true);
            lossScenario.SetActive(true);

            // Call SpawnLoseObjectOnPanel for corresponding panels
         //   foreach (MyPanel panel in materialDome.wallPanels)
         //   {
                // Check if the panel is active
         //       if (panel.gameObject.activeSelf)
          //      {
                    // Call SpawnWinObjectOnPanel after setting the position and parenting
         //           materialDome.SpawnLoseObjectOnPanel(panel.transform, true);
         //       }
         //   }

         //   foreach (MyPanel panel in materialDome.floorPanels)
         //   {
                // Check if the panel is active
         //       if (panel.gameObject.activeSelf)
         //       {
                    // Call SpawnWinObjectOnPanel after setting the position and parenting
        //            materialDome.SpawnLoseObjectOnPanel(panel.transform, false);
        //        }
        //    }
        }

        //check for win condition
        if(co2Amount <= winAmount)
        {
            //win game
            winCanvas.SetActive(true);
            winScenario.SetActive(true);

            // Call SpawnWinObjectOnPanel for corresponding panels
           // foreach (MyPanel panel in materialDome.wallPanels)
           // {
                // Check if the panel is active
          //      if (panel.gameObject.activeSelf)
          //      {
                    // Call SpawnWinObjectOnPanel after setting the position and parenting
          //          materialDome.SpawnWinObjectOnPanel(panel.transform, true);
          //      }
         //   }

           // foreach (MyPanel panel in materialDome.floorPanels)
           // {
                // Check if the panel is active
           //     if (panel.gameObject.activeSelf)
           //     {
                    // Call SpawnWinObjectOnPanel after setting the position and parenting
                   // materialDome.SpawnWinObjectOnPanel(panel.transform, false);
           //     }
          //  }
        }

        // Update ParticleSystem Rate Over Time based on co2Amount
        UpdateParticleSystemRate();
    }
    public void AddCO2(float co2Cost)
    {
        co2Amount += co2Cost;
        co2Amount = Mathf.Clamp(co2Amount, 0, 100);

        co2DisplayImage.fillAmount = co2Amount / 100f;
        Debug.Log("addco2 ran");
    }

    public void RemoveCO2(float co2RemovalAmount)
    {
        co2Amount -= co2RemovalAmount;
        co2DisplayImage.fillAmount = co2Amount / 100f;
        Debug.Log("removeco2 ran");

    }

    //this method updates our display balance whenever it changes, so this will be embedded in each appropriate 
    //method.
    void updateDisplay()
    {
        displayAmount.text = "CO2: " + co2Amount;
    }

    //function to add CO2 over time
    private void AddCO2OverTime()
    {
        co2Amount += addCO2OverTimeAount;
        co2Amount = Mathf.Clamp(co2Amount, 0, 100);

        co2DisplayImage.fillAmount = co2Amount / 100f;
    }

    //function to lower co2 over time
    public void RemoveCO2OverTime()
    {
        co2Amount -= removeCO2OverTimeAmount;
        co2Amount = Mathf.Clamp(co2Amount, 0, 100);

        co2DisplayImage.fillAmount = co2Amount / 100f;
    }

    //Used by Play Again button
    public void ReloadScene()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.buildIndex);
    }

    // Update ParticleSystem Rate Over Time
    private void UpdateParticleSystemRate()
    {
        // Check if the ParticleSystemEditor reference is not null
        if (particleSystemEditor != null)
        {
            // Map co2Amount to a range between 0 and particleSystemUpperRange float
            float mappedRate = Mathf.Lerp(0f, particleSystemUpperRange, co2Amount / 100f);

            // Update Rate Over Time based on the mappedRate
            var emissionModule = particleSystemEditor.emission;
            emissionModule.rateOverTime = mappedRate;
            Debug.Log(mappedRate);
        }

        // Check if the ParticleSystemAR reference is not null
        if (particleSystemAR != null)
        {
            // Map co2Amount to a range between 0 and particleSystemUpperRange float
            float mappedRateAR = Mathf.Lerp(0f, particleSystemUpperRange, co2Amount / 100f);

            // Update Rate Over Time based on the mappedRate
            var emissionModuleAR = particleSystemAR.emission;
            emissionModuleAR.rateOverTime = mappedRateAR;
        }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Text.RegularExpressions;

public class InventoryController_GridBuilding : MonoBehaviour
{
    //gets a reference to the text field for the UI displays
    [SerializeField]
    private TextMeshProUGUI[] spriteCounts;

    //gets a reference to the GridBuildingSystem script
    private GridBuildingSystem gridBuildingSystem;

    //gets a reference to the CO2Meter script
    private CO2Meter co2Meter;

    //set the sprites for each semantic channel.
    public GameObject[] sprites;

    //a dictionary to hold a reference to each sprite
    private Dictionary<string, int> spriteDict;

    //string reference to each sprite name
    public string[] spriteNames;

    //string reference to the current semantic channel
    private string currSemantic;

    //array to hold the sprites and the amount for each sprite
    private int[] inventory;

    //sets a limit for the amount of resources that can be collected
    private int collectLimit;

    //a regular expression to use with string references
    private Regex regex;

    // Boolean flags to determine which costs are active
    private bool bioCostActive;
    private bool nonCostActive;

    //holds the 

    // Start is called before the first frame update
    void Start()
    {
        //sets a regular expression of case insensitve combination of letters and numbers
        //to extract info from strings with the same pattern
        regex = new Regex("([a-z]+)([0-9]+)", RegexOptions.Compiled | RegexOptions.IgnoreCase);

        //gets access to the correct sprites for each channel
        spriteDict = new Dictionary<string, int>();
        int idx;
        collectLimit = 999;

        //is an array that keeps track of the number of each sprite.
        inventory = new int[sprites.Length];

        //looks up the dictionary to set the inventory for each sprite to 0
        for (idx = 0; idx < sprites.Length; idx++)
        {
            spriteDict.Add(spriteNames[idx], idx);
            //initializes the count of the current sprite at the current index to zero.
            inventory[idx] = 0;
        }

        //sets the text of the diplay for each semantic counter to that of the inventory
        for (idx = 0; idx < spriteCounts.Length; idx++)
        {
            spriteCounts[idx].text = inventory[idx].ToString();
            //the below is only used if adding an additional UI display to inventory pop up
           // if (spriteMenuCounts[idx] != null)
            //{
            //    spriteMenuCounts[idx].text = inventory[idx].ToString();
           // }
        }

        // Initialize BioCostActive and NonCostActive to false
        bioCostActive = false;
        nonCostActive = false;

        //reference to the CO2Meter
        co2Meter = FindAnyObjectByType<CO2Meter>();
        //reference to the GridBuildingSystem
        gridBuildingSystem = FindObjectOfType<GridBuildingSystem>();

    }

    //Adds 1 per call to the appropriate semantic channel counter
    public void ResourceCollected(GameObject callingObject)
    {

        // Set the current semantic channel based on the tag of the game object
        currSemantic = callingObject.tag;

       if (spriteDict.ContainsKey(currSemantic))
       {
            int spriteIdx = spriteDict[currSemantic];
            //updates the sprite count
            inventory[spriteIdx] = Mathf.Clamp(inventory[spriteIdx] + 1, 0, collectLimit);
            spriteCounts[spriteIdx].text = inventory[spriteIdx].ToString();

           // Debug.Log("Semantic Counter ran");
       }
        else
        {
            // The key doesn't exist, handle this situation (e.g., log an error)
            Debug.LogError($"Key not found in spriteDict: {currSemantic}");
        }
        
     }

    public void BioCostActive()
    {
        bioCostActive = true;
        nonCostActive = false;
    }

    public void NonCostActive()
    {
        nonCostActive = true;
        bioCostActive = false;
    }

    public bool SubtractInventory()
    {
        // Check if there are elements in the spriteCounts array
        if (spriteCounts.Length > 0)
        {
            // Determine which sprite count to subtract from based on BioCostActive and NonCostActive
            TextMeshProUGUI targetSpriteCount = bioCostActive ? spriteCounts[0] : (nonCostActive ? spriteCounts[1] : null);

            // Check if the target sprite count is not null
            if (targetSpriteCount != null)
            {
                // Convert the text value to an integer (assuming it represents a number)
                if (int.TryParse(targetSpriteCount.text, out int currentValue))
                {
                    // Access the PlacedObjectTypeSO scriptable object (replace 'yourBuildingSO' with the actual reference)
                    PlacedObjectTypeSO buildingSO = gridBuildingSystem.placedObjectTypeSO;

                    // Determine the cost to subtract based on BioCostActive and NonCostActive
                    int costToSubtract = bioCostActive ? buildingSO.bioCost : (nonCostActive ? buildingSO.nonCost : 0);

                    // Check if the cost to be subtracted is less than the current value
                    if (costToSubtract <= currentValue)
                    {
                        // Subtract the cost value from the current value
                        int newValue = currentValue - costToSubtract;

                        // Update the text of the target sprite count with the new value
                        targetSpriteCount.text = newValue.ToString();
                        if (nonCostActive)
                        {
                            co2Meter.AddCO2(co2Meter.co2Cost);
                        }
                       else if(bioCostActive)
                        {
                            co2Meter.RemoveCO2(co2Meter.co2RemovalAmount); 
                        }
                        return true;
                    }
                    else
                    {
                        Debug.LogWarning("Attempted to subtract more than the current amount in spriteCount. Check your logic.");
                        return false;
                    }
                }
                else
                {
                    Debug.LogError("Failed to parse the text as an integer.");
                    return false;
                }
            }
            else
            {
                Debug.LogError("The target sprite count is null. Check your BioCostActive and NonCostActive logic.");
                return false;
            }
        }
        else
        {
            Debug.LogError("The spriteCounts array is empty.");
            return false; 
        }
    }
    //***DEPRICATED. the below can probably be erased. 
    public int GetCost(int idx)
    {
        // Access the buildingCosts from GameAssets
        if (GameAssets.i.buildingCosts.Length > idx)
        {
            return GameAssets.i.buildingCosts[idx];
        }
        return 0;
    }
    //checks the inventory to see if there are enough resources of specific type to spawn a building
    //if so, it decrements the cost of the building from the inventory count
    public bool QueryInventory(string name, bool decrement)
    {
        MatchCollection matches = regex.Matches(name);
        GroupCollection groups = matches[0].Groups;

        string semName = groups[1].Value;
        Debug.Log("NUM IDX: " + Int32.Parse(groups[2].Value));
        int numNeeded = GetCost(Int32.Parse(groups[2].Value));
        int semIdx = spriteDict[semName];

        //checks if enough resources are available
        Debug.Log("NUM NEEDED: " + numNeeded + ", AMOUNT LEFT: " + inventory[semIdx]);
        if (numNeeded <= inventory[semIdx])
        {
            Debug.Log("NUM ABOUT TO DECREMENT");
            //if there are enough resources, it decrements the correct amount from the inventory for that semantic channel and
            //updates the UI
            if (decrement)
            {
                inventory[semIdx] = inventory[semIdx] - numNeeded;
                spriteCounts[semIdx].text = inventory[semIdx].ToString();
                //The below only needed for additional menu count display, not currently implemented
                //if (spriteMenuCounts[semIdx] != null)
               // {
                 //   spriteMenuCounts[semIdx].text = inventory[semIdx].ToString();
               // }
            }
            //if enough resources
            return true;
        }
        //if not enough resources
        return false;
    }

}

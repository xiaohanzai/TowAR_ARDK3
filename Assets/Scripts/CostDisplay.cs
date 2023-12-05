using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CostDisplay : MonoBehaviour
{
    public TextMeshProUGUI textFieldNon;
    public TextMeshProUGUI textFieldBio;
    //public Toggle toggleDisplay;
    public BuildingTypeSO buildingObjectTypeSO;

    private void Start()
    {
        // Update the text field with nonCost
        textFieldNon.text = $"{buildingObjectTypeSO.nonCost}";

        // Update the text field with bioCost
        textFieldBio.text = $"{buildingObjectTypeSO.bioCost}";
    
    // Attach the toggle event listener
    //toggleDisplay.onValueChanged.AddListener(UpdateCostDisplay);

    // Initial setup
    //UpdateCostDisplay(toggleDisplay.isOn);
}

   // private void UpdateCostDisplay(bool displayNonCost)
   // {
     //   if (displayNonCost)
     //   {
      //      DisplayNonCost();
      //  }
      //  else
      //  {
      //      DisplayBioCost();
      //  }
   // }

  //  private void DisplayNonCost()
  //  {
   //     // Update the text field with nonCost
  //      textFieldNon.text = $"Non Cost: {buildingObjectTypeSO.nonCost}";
  //  }

   // private void DisplayBioCost()
   // {
        // Update the text field with bioCost
   //     textFieldBio.text = $"Bio Cost: {buildingObjectTypeSO.bioCost}";
   // }
}

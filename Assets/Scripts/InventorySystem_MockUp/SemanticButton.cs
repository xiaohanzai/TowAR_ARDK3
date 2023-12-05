using UnityEngine;
using UnityEngine.UI;

public class SemanticButton : MonoBehaviour
{
    //***Currently, this script is redundant as the InventoryController script gets a reference to the button object.

    // Reference to the game object with the InventoryController_GridBuilding script
    public GameObject inventoryControllerObject;

    private void Start()
    {
        // Ensure the inventoryControllerObject is set in the Inspector
        if (inventoryControllerObject == null)
        {
            Debug.LogError("InventoryControllerObject is not assigned!");
            return;
        }

        // Get the Button component and add a listener to the onClick event
        Button button = GetComponent<Button>();
        if (button != null)
        {
            button.onClick.AddListener(OnButtonClick);
        }
        else
        {
            Debug.LogError("Button component not found!");
        }
    }

    private void OnButtonClick()
    {
        // Call the NonAdd function on the game object with InventoryController_GridBuilding
        inventoryControllerObject.GetComponent<InventoryController_GridBuilding>().ResourceCollected(gameObject);
    }
}

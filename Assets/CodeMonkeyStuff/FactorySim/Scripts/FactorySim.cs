using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;
/* Xiaohan */
using TMPro;
/* Xiaohan */

public class FactorySim : MonoBehaviour {

    [SerializeField] private TilemapVisual tilemapVisual;

    /* Xiaohan */
    //public TextMeshProUGUI textMeshPro;
    /* Xiaohan */

    /*JDM*/
    //checks if the system is complete **Now handled in Storage script
   // private bool buildSuccessFlag = false; 

    //to instantiate system complete function for each success
    public GameObject systemComplete;
    /*JDM*/

    private void Start() {
        TimeTickSystem.Create();
        BeltSystem.Create();

        /* Xiaohan */
        Tilemap tilemap = new Tilemap(GridBuildingSystem.Instance.gridWidth, GridBuildingSystem.Instance.gridHeight, 1f, Vector3.zero);
        /* Xiaohan */
        tilemap.SetTilemapVisual(tilemapVisual);


        for (int i = 0; i < 10; i++) {
            //GridBuildingSystem.Instance.TryPlaceObject(new Vector2Int(5 + i, 5), GameAssets.i.placedObjectTypeSO_Refs.conveyorBelt, PlacedObjectTypeSO.Dir.Right);
            //GridBuildingSystem.Instance.TryPlaceObject(new Vector2Int(5 + i, 7), GameAssets.i.placedObjectTypeSO_Refs.conveyorBelt, PlacedObjectTypeSO.Dir.Left);
        }
        /*
        GridBuildingSystem.Instance.TryPlaceObject(new Vector2Int(11, 12), GameAssets.i.placedObjectTypeSO_Refs.miningMachine, PlacedObjectTypeSO.Dir.Down);
        GridBuildingSystem.Instance.TryPlaceObject(new Vector2Int(13, 12), GameAssets.i.placedObjectTypeSO_Refs.grabber, PlacedObjectTypeSO.Dir.Right);

        GridBuildingSystem.Instance.TryPlaceObject(new Vector2Int(14, 12), GameAssets.i.placedObjectTypeSO_Refs.conveyorBelt, PlacedObjectTypeSO.Dir.Right);
        GridBuildingSystem.Instance.TryPlaceObject(new Vector2Int(15, 12), GameAssets.i.placedObjectTypeSO_Refs.conveyorBelt, PlacedObjectTypeSO.Dir.Right);
        GridBuildingSystem.Instance.TryPlaceObject(new Vector2Int(16, 12), GameAssets.i.placedObjectTypeSO_Refs.conveyorBelt, PlacedObjectTypeSO.Dir.Right);
        */
        //GridBuildingSystem.Instance.TryPlaceObject(new Vector2Int(16, 13), GameAssets.i.placedObjectTypeSO_Refs.grabber, PlacedObjectTypeSO.Dir.Up);
        //GridBuildingSystem.Instance.TryPlaceObject(new Vector2Int(16, 14), GameAssets.i.placedObjectTypeSO_Refs.smelter, PlacedObjectTypeSO.Dir.Down);

        /*
        GridBuildingSystem.Instance.TryPlaceObject(new Vector2Int(5, 5), GameAssets.i.placedObjectTypeSO_Refs.conveyorBelt, PlacedObjectTypeSO.Dir.Down, out PlacedObject beltPlacedObject);
        GridBuildingSystem.Instance.TryPlaceObject(new Vector2Int(5, 4), GameAssets.i.placedObjectTypeSO_Refs.conveyorBelt, PlacedObjectTypeSO.Dir.Down);
        GridBuildingSystem.Instance.TryPlaceObject(new Vector2Int(5, 3), GameAssets.i.placedObjectTypeSO_Refs.conveyorBelt, PlacedObjectTypeSO.Dir.Down);
        GridBuildingSystem.Instance.TryPlaceObject(new Vector2Int(5, 6), GameAssets.i.placedObjectTypeSO_Refs.conveyorBelt, PlacedObjectTypeSO.Dir.Down);
        
        GridBuildingSystem.Instance.TryPlaceObject(new Vector2Int(5, 2), GameAssets.i.placedObjectTypeSO_Refs.conveyorBelt, PlacedObjectTypeSO.Dir.Right);
        GridBuildingSystem.Instance.TryPlaceObject(new Vector2Int(6, 2), GameAssets.i.placedObjectTypeSO_Refs.conveyorBelt, PlacedObjectTypeSO.Dir.Right);
        GridBuildingSystem.Instance.TryPlaceObject(new Vector2Int(7, 2), GameAssets.i.placedObjectTypeSO_Refs.conveyorBelt, PlacedObjectTypeSO.Dir.Up);
        GridBuildingSystem.Instance.TryPlaceObject(new Vector2Int(7, 3), GameAssets.i.placedObjectTypeSO_Refs.conveyorBelt, PlacedObjectTypeSO.Dir.Right);
        GridBuildingSystem.Instance.TryPlaceObject(new Vector2Int(8, 3), GameAssets.i.placedObjectTypeSO_Refs.conveyorBelt, PlacedObjectTypeSO.Dir.Right);
        GridBuildingSystem.Instance.TryPlaceObject(new Vector2Int(9, 3), GameAssets.i.placedObjectTypeSO_Refs.conveyorBelt, PlacedObjectTypeSO.Dir.Up);
        GridBuildingSystem.Instance.TryPlaceObject(new Vector2Int(9, 4), GameAssets.i.placedObjectTypeSO_Refs.conveyorBelt, PlacedObjectTypeSO.Dir.Up);
        GridBuildingSystem.Instance.TryPlaceObject(new Vector2Int(9, 5), GameAssets.i.placedObjectTypeSO_Refs.conveyorBelt, PlacedObjectTypeSO.Dir.Up);
        GridBuildingSystem.Instance.TryPlaceObject(new Vector2Int(9, 6), GameAssets.i.placedObjectTypeSO_Refs.conveyorBelt, PlacedObjectTypeSO.Dir.Left);
        GridBuildingSystem.Instance.TryPlaceObject(new Vector2Int(8, 6), GameAssets.i.placedObjectTypeSO_Refs.conveyorBelt, PlacedObjectTypeSO.Dir.Left);
        GridBuildingSystem.Instance.TryPlaceObject(new Vector2Int(7, 6), GameAssets.i.placedObjectTypeSO_Refs.conveyorBelt, PlacedObjectTypeSO.Dir.Left);
        GridBuildingSystem.Instance.TryPlaceObject(new Vector2Int(6, 6), GameAssets.i.placedObjectTypeSO_Refs.conveyorBelt, PlacedObjectTypeSO.Dir.Left);
        
        WorldItem worldItem = WorldItem.Create(beltPlacedObject.GetGridPosition());

        (beltPlacedObject as ConveyorBelt).TrySetWorldItem(worldItem);
        worldItem.SetGridPosition(beltPlacedObject.GetGridPosition());
        //*/
    }

    private void Update() {
        /* Xiaohan */
        //HandleBuildingSelection();
        if (CheckBuildSuccess())
        {
            //textMeshPro.text = "success!";
            /*JDM*/
            // Reset the flag after success. Now handled in Storage script.
            //buildSuccessFlag = false;
            /*JDM*/
        }
        else
        {
            //textMeshPro.text = "system not complete";
        }
        /* Xiaohan */
    }

    private void HandleBuildingSelection() {
//#if UNITY_EDITOR
        if (Input.GetMouseButtonDown(0) && !UtilsClass.IsPointerOverUI() && GridBuildingSystem.Instance.GetPlacedObjectTypeSO() == null)
//#else
//        if (Input.touchCount > 0 && !UtilsClass.IsPointerOverUI() && GridBuildingSystem.Instance.GetPlacedObjectTypeSO() == null)
//#endif
        {
            // Not building anything
            if (GridBuildingSystem.Instance.GetGridObject(Mouse3D.GetMouseWorldPosition()) != null) {
                PlacedObject placedObject = GridBuildingSystem.Instance.GetGridObject(Mouse3D.GetMouseWorldPosition()).GetPlacedObject();
                if (placedObject != null) {
                    // Clicked on something
                    if (placedObject is Smelter) {
                        SmelterUI.Instance.Show(placedObject as Smelter);
                    }
                    if (placedObject is MiningMachine) {
                        MiningMachineUI.Instance.Show(placedObject as MiningMachine);
                    }
                    if (placedObject is Assembler) {
                        AssemblerUI.Instance.Show(placedObject as Assembler);
                    }
                    if (placedObject is Storage) {
                        StorageUI.Instance.Show(placedObject as Storage);
                    }
                    if (placedObject is Grabber) {
                        GrabberUI.Instance.Show(placedObject as Grabber);
                    }
                }
            }
        }
    }

    private void HandleDebugSpawnItem() {
        if (Input.GetKeyDown(KeyCode.I)) {
            PlacedObject placedObject = GridBuildingSystem.Instance.GetGridObject(GridBuildingSystem.Instance.GetMouseWorldSnappedPosition()).GetPlacedObject();
            if (placedObject != null && placedObject is IWorldItemSlot) {
                IWorldItemSlot worldItemSlot = placedObject as IWorldItemSlot;

                if (worldItemSlot.IsEmpty()) {
                    WorldItem worldItem = WorldItem.Create(worldItemSlot.GetGridPosition(), GameAssets.i.itemSO_Refs.ironOre);

                    worldItemSlot.TrySetWorldItem(worldItem);
                    worldItem.SetGridPosition(worldItemSlot.GetGridPosition());
                }
            }
        }
    }

    /* Xiaohan */
    private bool CheckBuildSuccess()
    {
        /*JDM*/
        // Return false if the success condition has already been met. Now handled in Storage script.
       // if (buildSuccessFlag)
       // {
        //    return false;
       // }
        /*JDM*/

        Storage[] storages = FindObjectsOfType<Storage>();
        if (storages.Length > 0)
        {
            foreach (Storage storage in storages)
            { 
                /*JDM*/
                // Check if the success flag has already been triggered for this storage unit
                if (!storage.HasTriggeredSuccessFlag())
                /*JDM*/
                {
                ItemStackList itemStackList = storage.GetItemStackList();
                ItemStack itemStack = itemStackList.GetItemStackWithItemType(GameAssets.i.itemSO_Refs.goldIngot);
                    if (itemStack != null && itemStack.amount >= 1)
                    {
                        /*JDM*/
                        //adds system complete prefab each time player completes circuit.
                        //check if prefab is assigned.
                        if (systemComplete != null)
                        {
                            Instantiate(systemComplete);
                        }
                        else
                        {
                            Debug.LogError("Prefab not assigned!");
                        }
                        // Set the flag to true upon success. Now triggered in Storage script.
                        storage.ResetSuccessFlag();
                        /*JDM*/
                        return true;
                    }
                }
            }
        }
        return false;
    }
    /* Xiaohan */
}

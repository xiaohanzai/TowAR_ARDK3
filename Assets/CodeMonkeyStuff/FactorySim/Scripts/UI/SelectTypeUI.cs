using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;

public class SelectTypeUI : MonoBehaviour {


    private Transform container;
    private Transform containerRight;

    private void Awake() {
        container = transform.Find("Container");

        container.Find("NoneBtn").GetComponent<Button_UI>().ClickFunc = () => {
            GridBuildingSystem.Instance.DeselectObjectType();
        };
        /*
        transform.Find("goldBtn").GetComponent<Button_UI>().ClickFunc = () => {
            GridBuildingSystem.Instance.SetSelectedPlacedObject(0);
        };

        transform.Find("ironBtn").GetComponent<Button_UI>().ClickFunc = () => {
            GridBuildingSystem.Instance.SetSelectedPlacedObject(1);
        };
        */
        container.Find("MiningMachineBtn").GetComponent<Button_UI>().ClickFunc = () => {
            GridBuildingSystem.Instance.SetSelectedPlacedObject(GameAssets.i.placedObjectTypeSO_Refs.miningMachine);
        };

        container.Find("SmelterBtn").GetComponent<Button_UI>().ClickFunc = () => {
            GridBuildingSystem.Instance.SetSelectedPlacedObject(GameAssets.i.placedObjectTypeSO_Refs.smelter);
        };

        container.Find("StorageBtn").GetComponent<Button_UI>().ClickFunc = () => {
            GridBuildingSystem.Instance.SetSelectedPlacedObject(GameAssets.i.placedObjectTypeSO_Refs.storage);
        };

        /* Xiaohan */
        //container.Find("AssemblerBtn").GetComponent<Button_UI>().ClickFunc = () => {
        //    GridBuildingSystem.Instance.SetSelectedPlacedObject(GameAssets.i.placedObjectTypeSO_Refs.assembler);
        //};

        //container.Find("BeltBtn").GetComponent<Button_UI>().ClickFunc = () => {
        //    GridBuildingSystem.Instance.SetSelectedPlacedObject(GameAssets.i.placedObjectTypeSO_Refs.conveyorBelt);
        //};

        //container.Find("GrabberBtn").GetComponent<Button_UI>().ClickFunc = () => {
        //    GridBuildingSystem.Instance.SetSelectedPlacedObject(GameAssets.i.placedObjectTypeSO_Refs.grabber);
        //};

        container.Find("ELineBtn").GetComponent<Button_UI>().ClickFunc = () => {
            GridBuildingSystem.Instance.SetSelectedPlacedObject(GameAssets.i.placedObjectTypeSO_Refs.electricalLine);
        };
        container.Find("PipeBtn").GetComponent<Button_UI>().ClickFunc = () => {
            GridBuildingSystem.Instance.SetSelectedPlacedObject(GameAssets.i.placedObjectTypeSO_Refs.pipe);
        };

        containerRight = transform.Find("ContainerRight");
        containerRight.Find("DemolishBtn").GetComponent<Button_UI>().ClickFunc = () => {
            GridBuildingSystem.Instance.SetDemolishActive();
        };
        containerRight.Find("RotateBtn").GetComponent<Button_UI>().ClickFunc = () => {
            GridBuildingSystem.Instance.HandleDirRotation(true);
        };
        /* Xiaohan */
    }

    private void Start() {
        GridBuildingSystem.Instance.OnSelectedChanged += Instance_OnSelectedChanged;

        UpdateSelectedPlacedObject();

        AddTooltipToButton(container.Find("NoneBtn").GetComponent<Button_UI>(), "Mouse");
        AddTooltipToButton(container.Find("MiningMachineBtn").GetComponent<Button_UI>(), "Mining Machine");
        AddTooltipToButton(container.Find("SmelterBtn").GetComponent<Button_UI>(), "Smelter");
        AddTooltipToButton(container.Find("StorageBtn").GetComponent<Button_UI>(), "Storage");
        /* Xiaohan */
        //AddTooltipToButton(container.Find("AssemblerBtn").GetComponent<Button_UI>(), "Assembler");
        //AddTooltipToButton(container.Find("BeltBtn").GetComponent<Button_UI>(), "Belt");
        //AddTooltipToButton(container.Find("GrabberBtn").GetComponent<Button_UI>(), "Grabber");
        AddTooltipToButton(containerRight.Find("DemolishBtn").GetComponent<Button_UI>(), "Demolish");
        /* Xiaohan */
    }

    private void Instance_OnSelectedChanged(object sender, System.EventArgs e) {
        UpdateSelectedPlacedObject();
    }

    private void UpdateSelectedPlacedObject() {
        PlacedObjectTypeSO placedObjectTypeSO = GridBuildingSystem.Instance.GetPlacedObjectTypeSO();

        container.Find("NoneBtn").Find("Selected").gameObject.SetActive(placedObjectTypeSO == null && !GridBuildingSystem.Instance.IsDemolishActive());
        container.Find("MiningMachineBtn").Find("Selected").gameObject.SetActive(placedObjectTypeSO == GameAssets.i.placedObjectTypeSO_Refs.miningMachine);
        container.Find("SmelterBtn").Find("Selected").gameObject.SetActive(placedObjectTypeSO == GameAssets.i.placedObjectTypeSO_Refs.smelter);
        container.Find("StorageBtn").Find("Selected").gameObject.SetActive(placedObjectTypeSO == GameAssets.i.placedObjectTypeSO_Refs.storage);
        /* Xiaohan */
        //container.Find("AssemblerBtn").Find("Selected").gameObject.SetActive(placedObjectTypeSO == GameAssets.i.placedObjectTypeSO_Refs.assembler);
        //container.Find("BeltBtn").Find("Selected").gameObject.SetActive(placedObjectTypeSO == GameAssets.i.placedObjectTypeSO_Refs.conveyorBelt);
        //container.Find("GrabberBtn").Find("Selected").gameObject.SetActive(placedObjectTypeSO == GameAssets.i.placedObjectTypeSO_Refs.grabber);
        container.Find("ELineBtn").Find("Selected").gameObject.SetActive(placedObjectTypeSO == GameAssets.i.placedObjectTypeSO_Refs.electricalLine);
        container.Find("PipeBtn").Find("Selected").gameObject.SetActive(placedObjectTypeSO == GameAssets.i.placedObjectTypeSO_Refs.pipe);

        containerRight.Find("DemolishBtn").Find("Selected").gameObject.SetActive(GridBuildingSystem.Instance.IsDemolishActive());
        /* Xiaohan */
    }


    private void AddTooltipToButton(Button_UI buttonUI, string tooltip) {
        buttonUI.MouseOverOnceTooltipFunc = () => {
            TooltipCanvas.ShowTooltip_Static(tooltip);
        };
        buttonUI.MouseOutOnceTooltipFunc = () => {
            TooltipCanvas.HideTooltip_Static();
        };
    }

}

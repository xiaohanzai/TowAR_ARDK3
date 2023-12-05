using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;
using TMPro;

public class GridBuildingSystem : MonoBehaviour {

    public static GridBuildingSystem Instance { get; private set; }

    //*JDM*gets a reference to the InventoryController_GridBuilding
    public InventoryController_GridBuilding inventoryController;
    //sets the CO2Meter as a local variable
    CO2Meter co2Meter;
    float demolishCost = 2; 
    /*JDM */

    /* Xiaohan */
    public int gridWidth = 50;
    public int gridHeight = 50;
    float cellSize = 1f;
    public float scaling;
    //public TextMeshProUGUI textMeshPro; // for debugging
    private bool isBuildActive;
    public GameObject RotateBtn;
    /* Xiaohan */

    public event EventHandler OnSelectedChanged;
    public event EventHandler OnObjectPlaced;

    private BuildingCO2 buildingC02; 
    private GridXZ<GridObject> grid;
    /*JDM */
    //making this public to access in InventoryController
    public PlacedObjectTypeSO placedObjectTypeSO;
    /*JDM */
    private PlacedObjectTypeSO.Dir dir;

    private bool isDemolishActive;

    private void Awake() {
        Instance = this;

        /* Xiaohan */
        if (transform.parent != null)
        {
            scaling = transform.parent.localScale.x;
        }
        else
        {
            scaling = 1;
        }
        grid = new GridXZ<GridObject>(gridWidth, gridHeight, cellSize * scaling, transform.parent.position, (GridXZ<GridObject> g, int x, int y) => new GridObject(g, x, y));
        //textMeshPro.text = transform.parent.position.ToString();

        isBuildActive = false;
        /* Xiaohan */

        placedObjectTypeSO = null;
    }

    /*JDM */
    private void Start()
    {
        //gets the reference to CO2Meter
        co2Meter = FindObjectOfType<CO2Meter>();
    }
    /*JDM */

    /* Xiaohan */
    public void SetGridOriginPosition(Vector3 pos)
    {
        grid.SetOriginPosition(pos);
    }
    /* Xiaohan */

    public class GridObject {

        private GridXZ<GridObject> grid;
        private int x;
        private int y;
        public PlacedObject placedObject;

        public GridObject(GridXZ<GridObject> grid, int x, int y) {
            this.grid = grid;
            this.x = x;
            this.y = y;
            placedObject = null;
        }

        public override string ToString() {
            return x + ", " + y + "\n" + placedObject;
        }

        public void TriggerGridObjectChanged() {
            grid.TriggerGridObjectChanged(x, y);
        }

        public void SetPlacedObject(PlacedObject placedObject) {
            this.placedObject = placedObject;
            TriggerGridObjectChanged();
        }

        public void ClearPlacedObject() {
            placedObject = null;
            TriggerGridObjectChanged();
        }

        public PlacedObject GetPlacedObject() {
            return placedObject;
        }

        public bool CanBuild() {
            return placedObject == null;
        }

    }

    private void Update() {
        if (placedObjectTypeSO == GameAssets.i.placedObjectTypeSO_Refs.conveyorBelt) {
            // Placing a belt, place in a line
            HandleBeltPlacement();
        } else {
            HandleNormalObjectPlacement();
        }
        HandleDirRotation();
        HandleDemolish();

#if UNITY_EDITOR
        if (Input.GetMouseButtonDown(1)) {
            DeselectObjectType();
        }
#endif
    }

    private void HandleNormalObjectPlacement() {
        /* Xiaohan */
        if (!isBuildActive)
        {
            return;
        }
        /* Xiaohan */
//#if UNITY_EDITOR
        if (placedObjectTypeSO != null) {
//#else
//        if (Input.touchCount > 0 && placedObjectTypeSO != null && !UtilsClass.IsPointerOverUI()) {
//#endif
            Vector3 mousePosition = Mouse3D.GetMouseWorldPosition();
            grid.GetXZ(mousePosition, out int x, out int z);
            /* Xiaohan */
            //textMeshPro.text += "\n" + mousePosition.ToString() + "\n";
            //textMeshPro.text += x.ToString() + ", " + z.ToString();
            /* Xiaohan */

            Vector2Int placedObjectOrigin = new Vector2Int(x, z);

            /*JDM */
            bool placed = false;
            if (inventoryController != null)
            {
                if (inventoryController.SubtractInventory())
                {
                    TryPlaceObject(placedObjectOrigin, placedObjectTypeSO, dir, out PlacedObject placedObject);
                    // Object placed
                    placed = true;
                    /*JDM */
                }
            }
            else
            {
                if (TryPlaceObject(placedObjectOrigin, placedObjectTypeSO, dir, out PlacedObject placedObject))
                {
                    placed = true;
                }
            }
            if (!placed)
            {
                // Error!
                UtilsClass.CreateWorldTextPopup("Cannot Build Here!", mousePosition);
            }
        }
        /* Xiaohan */
        isBuildActive = false;
        /* Xiaohan */
    }

    private void HandleBeltPlacement() {
        /* Xiaohan */
        if (!isBuildActive)
        {
            return;
        }
        /* Xiaohan */
        // Placing a belt, place in a line
        if (placedObjectTypeSO == GameAssets.i.placedObjectTypeSO_Refs.conveyorBelt) {
            Vector3 mousePosition = Mouse3D.GetMouseWorldPosition();
            grid.GetXZ(mousePosition, out int x, out int z);

            Vector2Int placedObjectOrigin = new Vector2Int(x, z);
            TryPlaceObject(placedObjectOrigin, placedObjectTypeSO, dir, out PlacedObject placedObject);

            // ###### TODO: PLACE BELTS IN A LINE
            /*
            if (beltPlaceStartGridPosition == null) {
                // First stage, select start point
                beltPlaceStartGridPosition = placedObjectOrigin;
            } else {
                // Second stage, place all belts
                Vector2Int beltPlaceEndGridPosition = placedObjectOrigin;
                for (int beltX = beltPlaceStartGridPosition.Value.x; beltX < beltPlaceEndGridPosition.x; beltX++) {
                    TryPlaceObject(beltX, beltPlaceStartGridPosition.Value.y, placedObjectTypeSO, PlacedObjectTypeSO.Dir.Right);
                }
                for (int beltY = beltPlaceStartGridPosition.Value.y; beltY < beltPlaceEndGridPosition.y; beltY++) {
                    TryPlaceObject(beltPlaceEndGridPosition.x, beltY, placedObjectTypeSO, PlacedObjectTypeSO.Dir.Up);

                }
            }
            */
        }
        /* Xiaohan */
        isBuildActive = false;
        /* Xiaohan */
    }

    /* Xiaohan */
    public void HandleDirRotation(bool execute = false) {
        if ((Input.GetKeyDown(KeyCode.R) || execute) && placedObjectTypeSO != null) {
            dir = PlacedObjectTypeSO.GetNextDir(dir);
        }
    }
    /* Xiaohan */

    private void HandleDemolish() {
        if (isDemolishActive && Input.GetMouseButtonDown(0) && !UtilsClass.IsPointerOverUI()) {
            Vector3 mousePosition = Mouse3D.GetMouseWorldPosition();
            PlacedObject placedObject = grid.GetGridObject(mousePosition).GetPlacedObject();
            if (placedObject != null) {
                // Demolish
                placedObject.DestroySelf();
                /*JDM */
                //adds CO2 w/ every demo
                co2Meter.AddCO2(demolishCost);
                /*JDM */
                List<Vector2Int> gridPositionList = placedObject.GetGridPositionList();
                foreach (Vector2Int gridPosition in gridPositionList) {
                    grid.GetGridObject(gridPosition.x, gridPosition.y).ClearPlacedObject();
                }
            }
        }
    }

    private void UpdateCanBuildTilemap() {
        for (int x = 0; x < grid.GetWidth(); x++) {
            for (int y = 0; y < grid.GetHeight(); y++) {
                // Tilemap
                Tilemap.Instance.SetTilemapSprite(new Vector3(x, y),
                    grid.GetGridObject(x, y).CanBuild() ? 
                    Tilemap.TilemapObject.TilemapSprite.CanBuild :
                    Tilemap.TilemapObject.TilemapSprite.CannotBuild);
            }
        }
    }

    public void DeselectObjectType() {
        placedObjectTypeSO = null;
        isDemolishActive = false;
        RefreshSelectedObjectType();
    }

    private void RefreshSelectedObjectType() {
        UpdateCanBuildTilemap();

        if (placedObjectTypeSO == null) {
            TilemapVisual.Instance.Hide();
        } else {
            TilemapVisual.Instance.Show();
        }

        OnSelectedChanged?.Invoke(this, EventArgs.Empty);
    }

    public bool TryPlaceObject(int x, int y, PlacedObjectTypeSO placedObjectTypeSO, PlacedObjectTypeSO.Dir dir) {
        return TryPlaceObject(new Vector2Int(x, y), placedObjectTypeSO, dir, out PlacedObject placedObject);
    }

    public bool TryPlaceObject(Vector2Int placedObjectOrigin, PlacedObjectTypeSO placedObjectTypeSO, PlacedObjectTypeSO.Dir dir) {
        return TryPlaceObject(placedObjectOrigin, placedObjectTypeSO, dir, out PlacedObject placedObject);
    }

    public bool TryPlaceObject(Vector2Int placedObjectOrigin, PlacedObjectTypeSO placedObjectTypeSO, PlacedObjectTypeSO.Dir dir, out PlacedObject placedObject)
    {
        // Test Can Build
        List<Vector2Int> gridPositionList = placedObjectTypeSO.GetGridPositionList(placedObjectOrigin, dir);
        bool canBuild = true;
        foreach (Vector2Int gridPosition in gridPositionList)
        {
            if (!grid.IsValidGridPositionWithPadding(gridPosition))
            {
                // Not valid
                canBuild = false;
                break;
            }
            if (!grid.GetGridObject(gridPosition.x, gridPosition.y).CanBuild())
            {
                canBuild = false;
                break;
            }
        }

        if (canBuild)
        {
            
                {
                    Vector2Int rotationOffset = placedObjectTypeSO.GetRotationOffset(dir);
                    Vector3 placedObjectWorldPosition = grid.GetWorldPosition(placedObjectOrigin.x, placedObjectOrigin.y) + new Vector3(rotationOffset.x, 0, rotationOffset.y) * grid.GetCellSize();
                    placedObject = PlacedObject.Create(placedObjectWorldPosition, placedObjectOrigin, dir, placedObjectTypeSO);
                    foreach (Vector2Int gridPosition in gridPositionList)
                    {
                        grid.GetGridObject(gridPosition.x, gridPosition.y).SetPlacedObject(placedObject);
                    }

                    placedObject.GridSetupDone();

                    OnObjectPlaced?.Invoke(placedObject, EventArgs.Empty);

                    return true;
                }
            }
            else
            {
                // Cannot build here
                placedObject = null;
                return false;
            }
        
    }

    public Vector2Int GetGridPosition(Vector3 worldPosition) {
        grid.GetXZ(worldPosition, out int x, out int z);
        return new Vector2Int(x, z);
    }

    public Vector3 GetWorldPosition(Vector2Int gridPosition) {
        return grid.GetWorldPosition(gridPosition.x, gridPosition.y);
    }

    public GridObject GetGridObject(Vector2Int gridPosition) {
        return grid.GetGridObject(gridPosition.x, gridPosition.y);
    }

    public GridObject GetGridObject(Vector3 worldPosition) {
        return grid.GetGridObject(worldPosition);
    }

    public bool IsValidGridPosition(Vector2Int gridPosition) {
        return grid.IsValidGridPosition(gridPosition);
    }

    public Vector3 GetMouseWorldSnappedPosition() {
        Vector3 mousePosition = Mouse3D.GetMouseWorldPosition();
        grid.GetXZ(mousePosition, out int x, out int z);

        if (placedObjectTypeSO != null) {
            Vector2Int rotationOffset = placedObjectTypeSO.GetRotationOffset(dir);
            Vector3 placedObjectWorldPosition = grid.GetWorldPosition(x, z) + new Vector3(rotationOffset.x, 0, rotationOffset.y) * grid.GetCellSize();
            return placedObjectWorldPosition;
        } else {
            return mousePosition;
        }
    }

    public Quaternion GetPlacedObjectRotation() {
        if (placedObjectTypeSO != null) {
            return Quaternion.Euler(0, placedObjectTypeSO.GetRotationAngle(dir), 0);
        } else {
            return Quaternion.identity;
        }
    }

    public PlacedObjectTypeSO GetPlacedObjectTypeSO() {
        return placedObjectTypeSO;
    }

    public void SetSelectedPlacedObject(PlacedObjectTypeSO placedObjectTypeSO) {
        this.placedObjectTypeSO = placedObjectTypeSO;
        isDemolishActive = false;
        RefreshSelectedObjectType();
        if (RotateBtn != null)
        {
            RotateBtn.SetActive(true);
        }
    }

    public void SetDemolishActive() {
        placedObjectTypeSO = null;
        isDemolishActive = true;
        RefreshSelectedObjectType();
    }

    public bool IsDemolishActive() {
        return isDemolishActive;
    }

    /*JDM */
    public void SetSelectedInActive()
    {
        placedObjectTypeSO = null;
        isDemolishActive = false;
        RefreshSelectedObjectType();
        if (RotateBtn != null)
        {
            RotateBtn.SetActive(false);
        }
    }
    /*JDM */

    /* Xiaohan */
    public void SetBuildActive()
    {
        isBuildActive = true;
    }
    /* Xiaohan */
}

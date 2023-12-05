using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldItem : MonoBehaviour {

    public static WorldItem Create(Vector2Int gridPosition, ItemSO itemScriptableObject) {
        Transform worldItemTransform = Instantiate(GameAssets.i.pfWorldItem, GridBuildingSystem.Instance.GetWorldPosition(gridPosition), Quaternion.identity);
        /* Xiaohan */
        for (int i = 0; i < worldItemTransform.childCount; i++)
        {
            Transform childTransform = worldItemTransform.GetChild(i);
            childTransform.localScale = Vector3.one * GridBuildingSystem.Instance.scaling;
            childTransform.position = new Vector3((childTransform.position.x - worldItemTransform.position.x) * GridBuildingSystem.Instance.scaling + worldItemTransform.position.x, childTransform.position.y, (childTransform.position.z - worldItemTransform.position.z) * GridBuildingSystem.Instance.scaling + worldItemTransform.position.z);
        }
        /* Xiaohan */
        WorldItem worldItem = worldItemTransform.GetComponent<WorldItem>();
        worldItem.SetGridPosition(gridPosition);
        worldItem.itemSO = itemScriptableObject;

        return worldItem;
    }

    private Vector2Int gridPosition;
    private bool hasAlreadyMoved;
    private ItemSO itemSO;

    private void Start() {
        transform.Find("ItemVisual").Find("itemSprite").GetComponent<SpriteRenderer>().sprite = itemSO.sprite;
    }

    private void Update() {
        transform.position = Vector3.Lerp(transform.position, GridBuildingSystem.Instance.GetWorldPosition(gridPosition), Time.deltaTime * 10f);
    }

    public void SetGridPosition(Vector2Int gridPosition) {
        this.gridPosition = gridPosition;
    }

    public bool CanMove() {
        return !hasAlreadyMoved;
    }

    public void SetHasAlreadyMoved() {
        hasAlreadyMoved = true;
    }

    public void ResetHasAlreadyMoved() {
        hasAlreadyMoved = false;
    }

    public ItemSO GetItemSO() {
        return itemSO;
    }

    public void DestroySelf() {
        Destroy(gameObject);
    }

}

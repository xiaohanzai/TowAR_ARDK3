using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Storage : PlacedObject, IItemStorage {

    public event EventHandler OnItemStorageCountChanged;

    private ItemStackList itemStackList;

    protected override void Setup() {
        //Debug.Log("Storage.Setup()");
        itemStackList = new ItemStackList();
        /* Xiaohan */
        if (GetComponent<PlaceGrabbers>() != null)
        {
            GetComponent<PlaceGrabbers>().Place();
        }
        /* Xiaohan */
    }

    /*JDM*/
    // Flag to track whether the success flag has been triggered for this storage unit
    private bool hasTriggeredSuccessFlag = false;
    /*JDM*/

    public override string ToString() {
        return itemStackList.ToString();
    }

    public ItemStackList GetItemStackList() {
        return itemStackList;
    }

    public int GetItemStoredCount(ItemSO filterItemSO) {
        return itemStackList.GetItemStoredCount(filterItemSO);
    }

    public bool TryGetStoredItem(ItemSO[] filterItemSO, out ItemSO itemSO) {
        ItemStack itemStack = itemStackList.GetFirstItemStackWithFilter(filterItemSO);
        if (itemStack != null && itemStack.amount > 0) {
            itemStack.amount--;
            itemSO = itemStack.itemSO;
            OnItemStorageCountChanged?.Invoke(this, EventArgs.Empty);
            TriggerGridObjectChanged();
            return true;
        } else {
            itemSO = null;
            return false;
        }
    }

    public ItemSO[] GetItemSOThatCanStore() {
        return new ItemSO[] { GameAssets.i.itemSO_Refs.any };
    }

    public bool TryStoreItem(ItemSO itemSO) {
        if (itemStackList.CanAddItemToItemStack(itemSO)) {
            itemStackList.AddItemToItemStack(itemSO);
            OnItemStorageCountChanged?.Invoke(this, EventArgs.Empty);
            TriggerGridObjectChanged();
            return true;
        } else {
            return false;
        }
    }

    /*JDM*/
    // a method to reset the flag if needed
    public void ResetSuccessFlag()
    {
        hasTriggeredSuccessFlag = true;
    }

    public bool HasTriggeredSuccessFlag()
    {
        return hasTriggeredSuccessFlag;
    }
    /*JDM*/
}

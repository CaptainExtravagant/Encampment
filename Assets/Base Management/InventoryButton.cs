using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryButton : MonoBehaviour {

    private bool inInventory;

    private BaseItem heldItem;

    private BaseItem.ITEM_TYPE itemType;

    private void Awake()
    {
        itemType = heldItem.itemType;
    }

    public void RemoveItem()
    {

    }

    public void MoveItem()
    {

    }
}

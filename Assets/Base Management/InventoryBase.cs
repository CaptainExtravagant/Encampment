using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryBase : MonoBehaviour, I_Inventory {

	private GameObject itemButtonReference;
	private GameObject[] itemButtonList = new GameObject[itemCap];

	public BaseItem[] itemList = new BaseItem[itemCap];

	private BaseItem[] displayItems = new BaseItem[itemCap];

	private BaseWeapon[] weaponList = new BaseWeapon[itemCap];
	private BaseArmor[] armorList = new BaseArmor[itemCap];

	public Image[] imageList = new Image[itemCap];
	private static int itemCap = 10;

	public GameObject inventoryScrollBox;
	public Dropdown inventoryDropdown;

	void Awake()
	{
		itemButtonReference = (GameObject)Resources.Load ("InventoryItem");
	}

	public void SortItems()
	{
		if (inventoryDropdown.value == 0) {
			displayItems = itemList;
		} else if (inventoryDropdown.value == 1) {
			displayItems = weaponList;
		} else if (inventoryDropdown.value == 2) {
			displayItems = armorList;
		}
	}

	public void AddItem(BaseItem itemToAdd)
	{
		I_Inventory inventoryInterface = this as I_Inventory;

		inventoryInterface.AddItem (itemToAdd);
	}

	bool I_Inventory.AddItem(BaseItem itemToAdd)
    {
        //Run through entire inventory
        for(int i = 0; i < itemList.Length; i++)
        {
            //Find the next empty slot
            if(itemList[i] == null)
            {

				itemList [i] = Instantiate(itemToAdd);

                //Add the item to the inventory, the object itself is somewhere in the world when it's picked up/created
				itemButtonList[i] = 
					Instantiate(itemButtonReference,
						inventoryScrollBox.transform, true);
				
                //Create the button for the inventory
				itemButtonList [i].GetComponent<InventoryButton> ().Init (itemList[i].GetItemType(),
					itemList[i].GetItemName(),
					itemList[i].GetItemSprite());

				switch (itemList [i].GetItemType ()) {
				case BaseItem.ITEM_TYPE.ITEM_WEAPON:
					weaponList [i] = itemList [i].GetComponent<BaseWeapon>();
					break;

				case BaseItem.ITEM_TYPE.ITEM_ARMOR:
					armorList [i] = itemList [i].GetComponent<BaseArmor>();
					break;
				}
				
                return true;
            }
        }
        return false;
    }

	bool I_Inventory.RemoveItem(BaseItem itemToRemove)
    {
        //Go through entire inventory
        for (int i = 0; i < itemList.Length; i++)
        {
            //Find the item that needs to be removed
            if(itemList[i] == itemToRemove)
            {
                //Set the item and the inventory button to false
                itemList[i] = null;
                itemButtonList[i] = null;

                return true;
            }
        }

        return false;
    }


    public void RemoveItem(BaseItem itemToRemove)
	{
		for (int i = 0; i < itemList.Length; i++) {
			if (itemList [i] == itemToRemove) {
				itemList [i] = null;
				imageList [i].sprite = null;
				imageList [i].enabled = false;

                itemButtonList[i] = null;
			}
		}
	}
}

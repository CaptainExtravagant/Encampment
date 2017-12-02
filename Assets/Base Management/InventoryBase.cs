using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryBase : MonoBehaviour, I_Inventory {

	public GameObject itemButtonReference;
	private GameObject[] itemButtonList;

	public BaseItem[] itemList = new BaseItem[itemCap];
	public Image[] imageList = new Image[itemCap];
	private static int itemCap = 10;

	public GameObject inventoryScrollBox;

	void Awake()
	{
		itemButtonReference = Resources.Load ("InventoryItem") as GameObject;
	}

	public void AddItem(BaseItem itemToAdd)
	{
		for (int i = 0; i < itemList.Length; i++) {
			if (itemList [i] == null) {
				itemList [i] = Instantiate(itemToAdd);

                itemButtonList[i] = Instantiate(itemButtonReference,
                    inventoryScrollBox.transform, true);
                
				itemButtonList [i].GetComponent<Button> ().image = imageList [i];
				itemButtonList [i].GetComponentInChildren<Text> ().text = itemToAdd.GetItemName ();

			}
		}
	}

    bool I_Inventory.AddItem(GameObject itemToAdd)
    {
        //Run through entire inventory
        for(int i = 0; i < itemList.Length; i++)
        {
            //Find the next empty slot
            if(itemList[i] == null)
            {
                //Add the item to the inventory, the object itself is somewhere in the world when it's picked up/created
                itemList[i] = itemToAdd.GetComponent<BaseItem>();

                //Create the button for the inventory
                itemButtonList[i] = Instantiate(itemButtonReference, inventoryScrollBox.transform, true);

                return true;
            }
        }
        return false;
    }

    bool I_Inventory.RemoveItem(GameObject itemToRemove)
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

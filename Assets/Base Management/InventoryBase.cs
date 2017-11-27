using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryBase : MonoBehaviour {

	private GameObject itemButtonReference;
	private GameObject[] itemButtonList;

	public BaseItem[] itemList = new BaseItem[itemCap];
	public Image[] imageList = new Image[itemCap];
	private static int itemCap = 10;

	public GameObject inventoryScrollBox;

	void Awake()
	{
		itemButtonReference = (GameObject)Resources.Load ("InventoryItem");
	}

	public void AddItem(BaseItem itemToAdd)
	{
		for (int i = 0; i < itemList.Length; i++) {
			if (itemList [i] == null) {
				itemList [i] = itemToAdd;
				imageList [i].sprite = itemToAdd.GetSprite ();
				imageList [i].enabled = true;

                itemButtonList[i] = Instantiate(itemButtonReference, inventoryScrollBox.transform, true);
                
				itemButtonList [i].GetComponent<Button> ().image = imageList [i];
				itemButtonList [i].GetComponentInChildren<Text> ().text = itemToAdd.GetItemName ();

			}
		}
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

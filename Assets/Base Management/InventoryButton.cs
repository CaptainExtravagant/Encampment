using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryButton : MonoBehaviour {

    private bool inInventory;

    private BaseItem heldItem;

    private BaseItem.ITEM_TYPE itemType;

	Text buttonText;
	Image buttonImage;

    private void Awake()
    {
		buttonText = GetComponent<Text> ();
		buttonImage = GetComponent<Image>();
    }

	public void Init(BaseItem.ITEM_TYPE newType, string newName, Sprite newSprite)
	{
		buttonText = GetComponentInChildren<Text> ();
		buttonImage = GetComponent<Image> ();

		SetItemType (newType);
		SetItemName (newName);
		SetItemImage (newSprite);
	}

	public void SetItemType(BaseItem.ITEM_TYPE newType)
	{
		itemType = newType;
	}

	public void SetItemName(string newName)
	{
		buttonText.text = newName;
	}

	public void SetItemImage(Sprite newSprite)
	{
		buttonImage.sprite = newSprite;
	}
}

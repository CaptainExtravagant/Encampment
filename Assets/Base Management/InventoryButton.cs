using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryButton : MonoBehaviour {

	PlayerController playerController;

    private bool inInventory;
	private InventoryBase inventoryRef;
    private BaseItem heldItem;

    private BaseItem.ITEM_TYPE itemType;

	Text buttonText;
	Image buttonImage;

    private void Awake()
    {
		buttonText = GetComponent<Text> ();
		buttonImage = GetComponent<Image>();
    }

	public void Init(BaseItem.ITEM_TYPE newType, string newName, Sprite newSprite, InventoryBase inventory, BaseItem item, PlayerController controller)
	{
		buttonText = GetComponentInChildren<Text> ();
		buttonImage = GetComponent<Image> ();
		playerController = controller;

		heldItem = item;

		inventoryRef = inventory;

		SetItemType (newType);
		SetItemName (newName);
		SetItemImage (newSprite);

		GetComponent<Button> ().onClick.AddListener (ButtonPressed);
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

	public BaseItem.ITEM_TYPE GetItemType()
	{
		return itemType;
	}

	private void ButtonPressed()
	{
		inventoryRef.RemoveItem (heldItem);
		playerController.InventoryButtonPressed (heldItem);
	}
}

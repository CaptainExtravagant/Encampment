using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseItem : MonoBehaviour {

    protected float baseAbility;
    protected string itemName;

    public enum ITEM_TYPE
    {
        ITEM_WEAPON = 0,
        ITEM_ARMOR,
        ITEM_UTILITY
    }

    public ITEM_TYPE itemType;

	protected Sprite itemSprite;
    
	public Sprite GetSprite()
	{
		return itemSprite;
	}

	protected void SetItemName(string nameToSet)
	{
		itemName = nameToSet;
	}

	public string GetItemName()
	{
		Debug.Log (itemName);
		return itemName;
	}
}

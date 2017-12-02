using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseItem : MonoBehaviour, I_Item {

    protected float baseAbility;
    protected string itemName;

    public enum ITEM_TYPE
    {
        ITEM_WEAPON = 0,
        ITEM_ARMOR,
        ITEM_UTILITY
    }

	protected ITEM_TYPE itemType;

	protected Sprite itemSprite;

	public Sprite GetItemSprite()
	{
		return itemSprite;
	}

	protected void SetItemSprite(Sprite spriteToSet)
	{
		itemSprite = spriteToSet;
	}

	protected void SetItemType(ITEM_TYPE typeToSet)
	{
		itemType = typeToSet;
	}

	public ITEM_TYPE GetItemType()
	{
		return itemType;
	}

	void I_Item.CalculateBaseStats(BaseVillager villagerReference)
	{

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

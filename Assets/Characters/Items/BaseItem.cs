using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseItem : MonoBehaviour {

    protected float baseAbility;
    protected string itemName;

	protected Sprite itemSprite;

	protected void Init()
	{
		//Event dispatch to inventory manager, add to inventory etc.
	}

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

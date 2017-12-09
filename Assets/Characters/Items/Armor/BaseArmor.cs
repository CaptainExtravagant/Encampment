using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseArmor : BaseItem, I_Item {
    
    private float defense;
    private bool heavyArmor;

	protected float baseArmorValue;

    private void Awake()
    {
		SetItemType (ITEM_TYPE.ITEM_ARMOR);
    }

	void I_Item.CalculateBaseStats(BaseVillager villagerReference)
	{
		float smithingBonus = baseArmorValue + (1 + (villagerReference.GetTaskSkills ().armorCrafting / 100)) * (villagerReference.GetTaskSkills().blacksmithing / 100) ;

		SetBaseScore (smithingBonus);
	}

    protected void SetBaseScore(float smithingSkill)
    {
        baseAbility = smithingSkill;
    }

    protected float GetBaseScore()
    {
        return baseAbility;
    }

    public string GetArmorName()
    {
        return itemName;
    }

    public void SetArmorName(string newName)
    {
        itemName = newName;
    }

    public float GetDefenseValue()
    {
        return defense;
    }

    protected void SetDefenseValue(float newDefense)
    {
        defense = newDefense;
    }

    protected void SetHeavyArmor(bool isHeavy)
    {
        heavyArmor = isHeavy;
    }

    public bool IsHeavyArmor()
    {
        return heavyArmor;
    }

	public void Load(ArmorData armorData)
	{
		SetBaseScore (armorData.baseAbility);
		SetItemName (armorData.name);
		SetItemType ((ITEM_TYPE)armorData.itemType);

		SetDefenseValue (armorData.defense);
		SetHeavyArmor (armorData.heavyArmor);
	}

	public ArmorData Save()
	{
		ArmorData armorData = new ArmorData ();

		armorData.baseAbility = GetBaseScore();
		armorData.name = GetItemName ();
		armorData.itemType = (int)GetItemType ();

		armorData.defense = GetDefenseValue ();
		armorData.heavyArmor = IsHeavyArmor ();

		return armorData;
	}
}

[System.Serializable]
public class ArmorData
{
	public float baseAbility;
	public string name;
	public int itemType;

	public float defense;
	public bool heavyArmor;

}

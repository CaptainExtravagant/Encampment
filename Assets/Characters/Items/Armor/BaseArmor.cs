using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseArmor : BaseItem, I_Item {
    
    private float defense;
    private bool heavyArmor;

	protected float baseArmorValue;

    private void Awake()
    {
        itemType = ITEM_TYPE.ITEM_ARMOR;
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
}

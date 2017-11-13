using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseArmor : BaseItem {
    
    private float defense;
    private bool heavyArmor;

    protected void SetBaseScore(float smithingSkill)
    {
        baseAbility = smithingSkill / 10;
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

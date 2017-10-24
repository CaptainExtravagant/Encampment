using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseArmor : MonoBehaviour {

    private float baseAbility;

    private float defense;
    private string armorName;

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
        return armorName;
    }

    public void SetArmorName(string newName)
    {
        armorName = newName;
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

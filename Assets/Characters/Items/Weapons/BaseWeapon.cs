using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseWeapon : BaseItem, I_Item {

    public enum WEAPON_TYPE
    {
        WEAPON_SWORD = 0,
        WEAPON_AXE,
        WEAPON_POLEARM,
        WEAPON_BOW,
        WEAPON_LONGSWORD,
        WEAPON_SHIELD
    }

    private WEAPON_TYPE weaponType;

	protected float baseWeaponValue;

    private float damage;
    private float defense;

    private bool twoHanded;

    private void Awake()
    {
		SetItemType (ITEM_TYPE.ITEM_WEAPON);
    }

    public virtual void CalculateStats(Character characterReference)
	{
		//Calculate Attack Value
		float attackValue;
		float characterSkill = characterReference.GetCombatSkills().brawling;

		attackValue = GetBaseScore() + characterSkill;
		SetDamageValue(attackValue);

		//Calculate Defense Value
		float defenseValue;

		defenseValue = (GetBaseScore() / 10) + characterSkill;

		SetDefenseValue(defenseValue);
	}

	void I_Item.CalculateBaseStats(BaseVillager villagerReference)
	{
		float smithingBonus = baseWeaponValue + (1 + (villagerReference.GetTaskSkills ().weaponCrafting / 100)) * (villagerReference.GetTaskSkills().blacksmithing / 100) ;

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

    protected void SetWeaponType(WEAPON_TYPE newWeaponType)
    {
        weaponType = newWeaponType;
    }

    public WEAPON_TYPE GetWeaponType()
    {
        return weaponType;
    }

    public float GetDamageValue()
    {
        return damage;
    }

    protected void SetDamageValue(float newDamage)
    {
        damage = newDamage;
    }

    public float GetDefenseValue()
    {
        return defense;
    }

    protected void SetDefenseValue(float newDefense)
    {
        defense = newDefense;
    }

    public bool IsTwoHanded()
    {
        return twoHanded;
    }

    protected void SetTwoHanded(bool isTwoHanded)
    {
        twoHanded = isTwoHanded;
    }
}

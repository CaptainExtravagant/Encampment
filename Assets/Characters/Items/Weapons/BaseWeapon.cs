using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseWeapon : BaseItem, I_Item {

    public enum WEAPON_TYPE
    {
        WEAPON_SWORD = 0,
        WEAPON_FISTS,
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

	public WeaponData Save()
	{
        WeaponData weaponData = new WeaponData
        {
            baseAbility = GetBaseScore(),
            name = GetItemName(),
            itemType = (int)GetItemType(),
            weaponType = (int)GetWeaponType(),

            damage = GetDamageValue(),
            defense = GetDefenseValue(),
            twoHanded = IsTwoHanded()
        };

        if (weaponData == null)
            Debug.Log("Data is null");

        return weaponData;
	}

	public void Load(WeaponData weaponData)
	{

		SetBaseScore(weaponData.baseAbility);
		SetItemName(weaponData.name);
		SetItemType ((ITEM_TYPE)weaponData.itemType);
		SetWeaponType ((WEAPON_TYPE)weaponData.weaponType);

		SetDamageValue (weaponData.damage);
		SetDefenseValue (weaponData.defense);
		SetTwoHanded(weaponData.twoHanded);
	}
}

[System.Serializable]
public class WeaponData
{
	public float baseAbility;
	public string name;
	public int itemType;
	public int weaponType;

	public float damage;
	public float defense;
	public bool twoHanded;
}
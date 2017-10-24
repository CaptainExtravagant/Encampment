using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseWeapon : MonoBehaviour {

    protected enum WEAPON_TYPE
    {
        WEAPON_SWORD = 0,
        WEAPON_AXE,
        WEAPON_POLEARM,
        WEAPON_BOW,
        WEAPON_LONGSWORD,
        WEAPON_SHIELD
    }

    private WEAPON_TYPE weaponType;
    private float baseAbility;

    private float damage;
    private float defense;
    private string weaponName;

    private bool twoHanded;

    protected void SetBaseScore(float smithingSkill)
    {
        baseAbility = smithingSkill / 10;
    }

    protected float GetBaseScore()
    {
        return baseAbility;
    }

    protected void SetWeaponType(WEAPON_TYPE newWeaponType)
    {
        weaponType = newWeaponType;
    }

    public string GetWeaponName()
    {
        return weaponName;
    }

    public void SetWeaponName(string newName)
    {
        weaponName = newName;
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

    protected bool IsTwoHanded()
    {
        return twoHanded;
    }

    protected void SetTwoHanded(bool isTwoHanded)
    {
        twoHanded = isTwoHanded;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon_Sword : BaseWeapon {

	void Awake()
    {
		SetWeaponType(WEAPON_TYPE.WEAPON_SWORD);
		SetItemName("Sword");
    }

	public override void CalculateStats(Character characterReference)
    {

        //Calculate Attack Value
        float attackValue;
        float characterSkill = characterReference.GetCombatSkills().sword;

        attackValue = GetBaseScore() + characterSkill;
        SetDamageValue(attackValue);

        //Calculate Defense Value
        float defenseValue;

        defenseValue = (GetBaseScore() / 10) + characterSkill;

        SetDefenseValue(defenseValue);
    }
}

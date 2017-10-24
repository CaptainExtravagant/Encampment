using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon_Sword : BaseWeapon {

	void Start()
    {
        SetWeaponType(WEAPON_TYPE.WEAPON_SWORD);
        SetWeaponName("Sword");
    }

    private void CalculateStats(Character characterReference)
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon_Bow : BaseWeapon {
    
    private void Awake()
    {
        SetWeaponType(WEAPON_TYPE.WEAPON_BOW);
        SetItemName("Bow");
        SetTwoHanded(true);
		weaponRange = 15.0f;
    }

    public override void CalculateStats(Character characterReference)
    {
        //Calculate Attack Value
        float attackValue;
        float characterSkill = characterReference.GetCombatSkills().bow;

        attackValue = GetBaseScore() + characterSkill;
        SetDamageValue(attackValue);

        //Calculate Defense Value
        float defenseValue;

        defenseValue = (GetBaseScore() / 10) + characterSkill;

        SetDefenseValue(defenseValue);
    }
}
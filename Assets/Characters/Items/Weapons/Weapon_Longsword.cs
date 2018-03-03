using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon_Longsword : BaseWeapon {

    private void Awake()
    {
        SetWeaponType(WEAPON_TYPE.WEAPON_LONGSWORD);
        SetItemName("Longsword");
        SetTwoHanded(true);
		weaponRange = 6.0f;
    }

    public override void CalculateStats(Character characterReference)
    {
        //Calculate Attack Value
        float attackValue;
        float characterSkill = characterReference.GetCombatSkills().longsword;

        attackValue = GetBaseScore() + characterSkill;
        SetDamageValue(attackValue);

        //Calculate Defense Value
        float defenseValue;

        defenseValue = (GetBaseScore() / 10) + characterSkill;

        SetDefenseValue(defenseValue);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_Shield : BaseWeapon {

	private void Awake()
    {
        SetWeaponType(WEAPON_TYPE.WEAPON_SHIELD);
        SetItemName("Shield");
    }

    public override void CalculateStats(Character characterReference)
    {
        float attackValue;
        float characterSkill = characterReference.GetCombatSkills().armor;

        attackValue = (GetBaseScore() / 10) + characterSkill;
        SetDamageValue(attackValue);

        float defenseValue;

        defenseValue = GetBaseScore() + characterSkill;

        SetDefenseValue(defenseValue);
    }

}

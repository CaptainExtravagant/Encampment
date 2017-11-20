using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon_Axe : BaseWeapon {

    void Awake()
    {
        SetWeaponType(WEAPON_TYPE.WEAPON_AXE);
        SetItemName("Axe");
    }

    public override void CalculateStats(Character characterReference)
    {
        float attackValue;
        float characterSkill = characterReference.GetCombatSkills().axe;

        attackValue = GetBaseScore() + characterSkill;
        SetDamageValue(attackValue);

        float defenseValue;

        defenseValue = (GetBaseScore() / 10) + characterSkill;

        SetDefenseValue(defenseValue);
    }
}

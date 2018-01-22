using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building_Barracks : BaseBuilding, I_Building {

	private float skillBonus;

	private BaseWeapon.WEAPON_TYPE trainedWeapon;

	bool I_Building.PlaceInWorld()
	{
		BaseBuilding buildingParent = GetComponent<BaseBuilding> ();

		if (buildingParent.IsPlaced ()) {
			SetPlacedInWorld (true);
		}

		return false;
	}

	void Awake()
	{
		SetBuildingType (BUILDING_TYPE.BUILDING_BARRACKS);

		loadPath = "Buildings/BuildingBarracks";

		maxWorkingVillagers = 6;

		workTime = 10.0f;
		activeTimer = workTime;
        GetSkillBonus();
	}

    private void GetSkillBonus()
    {
        skillBonus = Mathf.Ceil(10 * Mathf.Pow(GetBuildingLevel(), 10));
    }

	public void SetTrainingWeapon(BaseWeapon.WEAPON_TYPE newWeapon)
	{
		trainedWeapon = newWeapon;
	}

	public BaseWeapon.WEAPON_TYPE GetTrainingWeapon()
	{
		return trainedWeapon;
	}

	override public void WorkBuilding(BaseVillager villagerReference)
	{
		activeTimer -= Time.deltaTime;
		float bonusValue;

		if (
			//If the villagers focus is lower than their curiosity then end the timer 1% of the curiostity value earlier than normal and improve the skill
			villagerReference.GetCharacterInfo ().characterAttributes.focus < villagerReference.GetCharacterInfo ().characterAttributes.curiosity
		    &&
		    activeTimer <= activeTimer - villagerReference.GetCharacterInfo ().characterAttributes.curiosity / 100) {

            GetSkillBonus();
            villagerReference.AddExperience ((int)skillBonus);
			activeTimer = workTime;

		}
		else if(activeTimer <= 0.0f)
		{
            GetSkillBonus();
            bonusValue = skillBonus + (villagerReference.GetCharacterInfo ().characterAttributes.focus / 100);

			villagerReference.AddExperience ((int)bonusValue);
			activeTimer = workTime;
		}
	}
}

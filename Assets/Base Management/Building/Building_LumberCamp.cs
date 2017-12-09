using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building_LumberCamp : BaseBuilding, I_Building {

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
		SetBuildingType (BUILDING_TYPE.BUILDING_LUMBERCAMP);
		loadPath = "Buildings/BuildingLumbercamp";
		workTime = 20.0f;
		activeTimer = workTime;
	}

	override public void WorkBuilding(BaseVillager villagerReference)
	{
		activeTimer -= Time.deltaTime;

		if (activeTimer <= 0.0f) {
			float resourceValue = villagerReference.GetTaskSkills ().woodcutting;

			baseManager.AddResources ((int)resourceValue, 1);

			activeTimer = workTime;
		}
	}
}

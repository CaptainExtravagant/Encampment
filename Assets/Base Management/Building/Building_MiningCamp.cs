using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building_MiningCamp : BaseBuilding, I_Building{

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
		SetBuildingType (BUILDING_TYPE.BUILDING_MININGCAMP);
		loadPath = "Buildings/BuildingMiningcamp";
		workTime = 20.0f;
		activeTimer = workTime;
	}

	override public void WorkBuilding(BaseVillager villagerReference)
	{
		activeTimer -= Time.deltaTime;

		if (activeTimer <= 0.0f) {
			float resourceValue = villagerReference.GetTaskSkills ().mining;

			baseManager.AddResources ((int)resourceValue, 0);

			activeTimer = workTime;
		}
	}
}

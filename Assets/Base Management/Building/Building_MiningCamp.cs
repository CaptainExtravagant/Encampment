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

	void Update()
	{
		if (workingVillagers.Count > 0) {
			activeTimer -= Time.deltaTime;

			if (activeTimer <= 0)
				WorkBuilding ();
		}
	}

	override public void WorkBuilding()
	{
		float resourceValue = 0;

		for (int i = 0; i < workingVillagers.Count; i++) {
			resourceValue += workingVillagers [i].GetTaskSkills ().mining;
		}

		resourceValue = resourceValue / workingVillagers.Count;

		baseManager.AddResources ((int)resourceValue, 0);

		activeTimer = workTime;
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building_MiningCamp : BaseBuilding {

	void Awake()
	{
		SetBuildingType (BUILDING_TYPE.BUILDING_MININGCAMP);

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

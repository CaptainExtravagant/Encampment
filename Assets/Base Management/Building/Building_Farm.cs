using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building_Farm : BaseBuilding {

	void Awake()
	{
		SetBuildingType (BUILDING_TYPE.BUILDING_FARM);

		workTime = 20.0f;
		activeTimer = workTime;
	}

	override public void WorkBuilding(BaseVillager villagerReference)
	{
		activeTimer -= Time.deltaTime;

		if (activeTimer <= 0.0f) {

			float resourceValue = villagerReference.GetTaskSkills ().farming;

			baseManager.AddResources ((int)resourceValue, 2);

			activeTimer = workTime;
		}
	}
}

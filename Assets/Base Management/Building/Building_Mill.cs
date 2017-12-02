using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building_Mill : BaseBuilding {

	// Use this for initialization
	void Awake()
	{
		SetBuildingType (BUILDING_TYPE.BUILDING_MILL);

		workTime = 15.0f;
		activeTimer = workTime;
	}

	override public void WorkBuilding(BaseVillager villagerReference)
	{
		activeTimer -= Time.deltaTime;

		if (activeTimer <= 0.0f) {
			float resourceValue = villagerReference.GetTaskSkills().farming;
			resourceValue *= 1 + (villagerReference.GetTaskSkills().farming / 100);

			baseManager.AddResources((int)resourceValue, 2);

			activeTimer = workTime;
		}
	}
}

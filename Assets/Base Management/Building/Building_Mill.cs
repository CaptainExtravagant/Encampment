using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building_Mill : BaseBuilding, I_Building {
    
	// Use this for initialization
	void Awake()
	{
		SetBuildingType (BUILDING_TYPE.BUILDING_MILL);
		loadPath = "Buildings/BuildingMill";
		workTime = 15.0f;
		activeTimer = workTime;
	}

	new void Update()
	{
        base.Update();

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
			resourceValue += workingVillagers [i].GetTaskSkills ().farming;
		}

		resourceValue = resourceValue / workingVillagers.Count;

		baseManager.AddResources((int)resourceValue, 2);

		activeTimer = workTime;
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building_LumberCamp : BaseBuilding, I_Building {
    
	void Awake()
	{
		SetBuildingType (BUILDING_TYPE.BUILDING_LUMBERCAMP);
		loadPath = "Buildings/BuildingLumbercamp";
		workTime = 20.0f;
		maxWorkingVillagers = 4;
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
			resourceValue += workingVillagers [i].GetTaskSkills ().woodcutting;
		}

		resourceValue = resourceValue / workingVillagers.Count;
	
		baseManager.AddResources ((int)resourceValue, 1);

		activeTimer = workTime;
	}
}

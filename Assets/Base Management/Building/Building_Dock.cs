using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building_Dock : BaseBuilding, I_Building {
    

	private void Awake()
	{

		SetBuildingType (BUILDING_TYPE.BUILDING_DOCK);

		loadPath = "Buildings/BuildingDocks";

		workTime = 20.0f;
		activeTimer = workTime;
		maxWorkingVillagers = 4;

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
			resourceValue += workingVillagers [i].GetTaskSkills().sailing;
		}

		resourceValue = resourceValue / workingVillagers.Count;

		baseManager.AddResources ((int)resourceValue, 2);

		activeTimer = workTime;
	}
}

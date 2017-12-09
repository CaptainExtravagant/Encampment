﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building_Dock : BaseBuilding, I_Building {

	bool I_Building.PlaceInWorld()
	{
		BaseBuilding buildingParent = GetComponent<BaseBuilding> ();

		if (buildingParent.IsPlaced ()) {
			SetPlacedInWorld (true);
		}

		return false;
	}

	private void Awake()
	{

		SetBuildingType (BUILDING_TYPE.BUILDING_DOCK);

		loadPath = "Buildings/BuildingDocks";

		workTime = 20.0f;
		activeTimer = workTime;

	}

	override public void WorkBuilding(BaseVillager villagerReference)
	{
		activeTimer -= Time.deltaTime;

		if (activeTimer <= 0.0f) {

			float resourceValue = villagerReference.GetTaskSkills().sailing;

			baseManager.AddResources ((int)resourceValue, 2);

			activeTimer = workTime;
		}
	}
}

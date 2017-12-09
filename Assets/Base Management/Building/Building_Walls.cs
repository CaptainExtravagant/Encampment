using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building_Walls : BaseBuilding, I_Building {

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
		SetBuildingType (BUILDING_TYPE.BUILDING_WALL);
		loadPath = "Buildings/BuildingWall";
		baseHealthValue = 100;
	}
}

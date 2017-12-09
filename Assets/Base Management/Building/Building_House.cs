using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building_House : BaseBuilding, I_Building {

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
		SetBuildingType (BUILDING_TYPE.BUILDING_HOUSE);
		loadPath = "Buildings/BuildingHouse";
	}

}

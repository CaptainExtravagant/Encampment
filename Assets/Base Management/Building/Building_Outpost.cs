using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building_Outpost : BaseBuilding, I_Building {

	bool I_Building.PlaceInWorld()
	{
		BaseBuilding buildingParent = GetComponent<BaseBuilding> ();

		if (buildingParent.IsPlaced ()) {
			SetPlacedInWorld (true);
		}

		return false;
	}

	// Use this for initialization
	void Awake()
	{
		SetBuildingType (BUILDING_TYPE.BUILDING_OUTPOST);
		loadPath = "Buildings/BuildingOutpost";


	}
}

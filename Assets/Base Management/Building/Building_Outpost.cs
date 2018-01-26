using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building_Outpost : BaseBuilding, I_Building {
    
	// Use this for initialization
	void Awake()
	{
		SetBuildingType (BUILDING_TYPE.BUILDING_OUTPOST);
		loadPath = "Buildings/BuildingOutpost";


	}
}

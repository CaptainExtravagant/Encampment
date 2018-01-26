using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building_Walls : BaseBuilding, I_Building {
    
	void Awake()
	{
		SetBuildingType (BUILDING_TYPE.BUILDING_WALL);
		loadPath = "Buildings/BuildingWall";
		baseHealthValue = 100;
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building_House : BaseBuilding, I_Building {
    
	void Awake()
	{
		SetBuildingType (BUILDING_TYPE.BUILDING_HOUSE);
		loadPath = "Buildings/BuildingHouse";
	}

}

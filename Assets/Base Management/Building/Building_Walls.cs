using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building_Walls : BaseBuilding, I_Building {
    
	void Awake()
	{
		SetBuildingType (BUILDING_TYPE.BUILDING_WALL);
		loadPath = "Buildings/BuildingWall";
		baseHealthValue = 100;

        SetBuildingCost();

        SetBuildingName();
        SetBuildingFunction();
    }

    protected override void SetBuildingName()
    {
        buildingName = "Wall";
    }

    protected override void SetBuildingFunction()
    {
        buildingFunction = "Stops goblins advancing into the village so fast. Very high health.";
    }
}

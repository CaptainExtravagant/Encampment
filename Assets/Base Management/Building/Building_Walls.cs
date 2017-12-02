using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building_Walls : BaseBuilding {

	void Awake()
	{
		SetBuildingType (BUILDING_TYPE.BUILDING_WALL);

		baseHealthValue = 100;
	}
}

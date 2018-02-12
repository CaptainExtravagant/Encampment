using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building_House : BaseBuilding, I_Building {

    int villagerSlots;

    void Awake()
	{
		SetBuildingType (BUILDING_TYPE.BUILDING_HOUSE);
		loadPath = "Buildings/BuildingHouse";
        villagerSlots = 5;
	}

    protected override void CreateBuilding(BaseVillager characterReference)
    {

        baseManager.IncreaseVillagerCap(villagerSlots);

        base.CreateBuilding(characterReference);
    }
}

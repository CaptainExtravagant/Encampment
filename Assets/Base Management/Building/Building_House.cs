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

        SetBuildingCost();

        SetBuildingName();
        SetBuildingFunction();
    }

    protected override void CreateBuilding(BaseVillager characterReference)
    {

        baseManager.IncreaseVillagerCap(villagerSlots);

        base.CreateBuilding(characterReference);
    }

    protected override void SetBuildingName()
    {
        buildingName = "House";
    }

    protected override void SetBuildingFunction()
    {
        buildingFunction = "Give villagers a place to live. Increases villager cap.";
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingDisplay : MonoBehaviour {

    private string buildingName;
    private int buildingLevel;
    private int buildingHealth;
    private int buildingMaxHealth;

    private BaseBuilding buildingInformation;
    private BaseBuilding.BUILDING_TYPE buildingType;

	public void SetInformation(BaseBuilding baseBuilding)
    {
        buildingInformation = baseBuilding;
        buildingType = baseBuilding.GetBuildingType();

        switch(baseBuilding.GetBuildingType())
        {
            case BaseBuilding.BUILDING_TYPE.BUILDING_BARRACKS:
                buildingName = "Barracks";
                break;

            case BaseBuilding.BUILDING_TYPE.BUILDING_BLACKSMITH:
                buildingName = "Blacksmith";
                break;

            case BaseBuilding.BUILDING_TYPE.BUILDING_DOCK:
                buildingName = "Dock";
                break;

            case BaseBuilding.BUILDING_TYPE.BUILDING_FARM:
                buildingName = "Farm";
                break;

            case BaseBuilding.BUILDING_TYPE.BUILDING_HOUSE:
                buildingName = "House";
                break;

            case BaseBuilding.BUILDING_TYPE.BUILDING_LUMBERCAMP:
                buildingName = "Lumber Camp";
                break;

            case BaseBuilding.BUILDING_TYPE.BUILDING_MILL:
                buildingName = "Mill";
                break;

            case BaseBuilding.BUILDING_TYPE.BUILDING_MININGCAMP:
                buildingName = "Mining Camp";
                break;

            case BaseBuilding.BUILDING_TYPE.BUILDING_OUTPOST:
                buildingName = "Outpost";
                break;

            case BaseBuilding.BUILDING_TYPE.BUILDING_TOWNHALL:
                buildingName = "Town Hall";
                break;

            case BaseBuilding.BUILDING_TYPE.BUILDING_WALL:
                buildingName = "Wall";
                break;

            default:
                
                break;
        }

        buildingLevel = baseBuilding.GetBuildingLevel();
        buildingHealth = (int)baseBuilding.GetCurrentHealth();
        buildingMaxHealth = (int)baseBuilding.GetMaxHealth();

        buildingType = baseBuilding.GetBuildingType();
    }
}

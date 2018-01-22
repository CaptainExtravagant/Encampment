using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildingDisplay : MonoBehaviour {

    private string buildingName;
    private int buildingLevel;
    private int buildingHealth;
    private int buildingMaxHealth;

    private BaseBuilding buildingInformation;
    private BaseBuilding.BUILDING_TYPE buildingType;

    private GameObject uniquePanel;

	public void SetInformation(BaseBuilding baseBuilding, GameObject infoPanel)
    {
        buildingInformation = baseBuilding;
        buildingType = baseBuilding.GetBuildingType();

        uniquePanel = baseBuilding.GetInfoPanel();

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

        GetComponentsInChildren<Text>()[0].text = buildingName;
        GetComponentsInChildren<Text>()[2].text = buildingLevel.ToString();

        GetComponentInChildren<Slider>().value = buildingHealth / buildingMaxHealth;

        Instantiate(uniquePanel, gameObject.transform);
    }
}

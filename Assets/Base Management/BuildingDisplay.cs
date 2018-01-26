using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildingDisplay : MonoBehaviour {

    private string buildingName;
    private int buildingLevel;
    private int buildingHealth;
    private int buildingMaxHealth;

    private BaseBuilding.BUILDING_TYPE buildingType;

    private GameObject uniquePanel;

	public void SetInformation(BaseBuilding baseBuilding, GameObject infoPanel)
    {
        buildingType = baseBuilding.GetBuildingType();

        uniquePanel = baseBuilding.GetInfoPanel();

		switch(buildingType)
        {
		case BaseBuilding.BUILDING_TYPE.BUILDING_BARRACKS:
			buildingName = "Barracks";
			baseBuilding.gameObject.GetComponent<Building_Barracks> ().SetUpInfoPanel ();
                break;

            case BaseBuilding.BUILDING_TYPE.BUILDING_BLACKSMITH:
			baseBuilding.gameObject.GetComponent<Building_Blacksmith> ().SetUpInfoPanel ();
                buildingName = "Blacksmith";
                break;

            case BaseBuilding.BUILDING_TYPE.BUILDING_DOCK:
			baseBuilding.gameObject.GetComponent<Building_Dock> ().SetUpInfoPanel ();
                buildingName = "Dock";
                break;

            case BaseBuilding.BUILDING_TYPE.BUILDING_FARM:
			baseBuilding.gameObject.GetComponent<Building_Farm> ().SetUpInfoPanel ();
                buildingName = "Farm";
                break;

            case BaseBuilding.BUILDING_TYPE.BUILDING_HOUSE:
			baseBuilding.gameObject.GetComponent<Building_House> ().SetUpInfoPanel ();
                buildingName = "House";
                break;

            case BaseBuilding.BUILDING_TYPE.BUILDING_LUMBERCAMP:
			baseBuilding.gameObject.GetComponent<Building_LumberCamp> ().SetUpInfoPanel ();
                buildingName = "Lumber Camp";
                break;

            case BaseBuilding.BUILDING_TYPE.BUILDING_MILL:
			baseBuilding.gameObject.GetComponent<Building_Mill> ().SetUpInfoPanel ();
                buildingName = "Mill";
                break;

            case BaseBuilding.BUILDING_TYPE.BUILDING_MININGCAMP:
			baseBuilding.gameObject.GetComponent<Building_MiningCamp> ().SetUpInfoPanel ();
                buildingName = "Mining Camp";
                break;

            case BaseBuilding.BUILDING_TYPE.BUILDING_OUTPOST:
			baseBuilding.gameObject.GetComponent<Building_Outpost> ().SetUpInfoPanel ();
                buildingName = "Outpost";
                break;

            case BaseBuilding.BUILDING_TYPE.BUILDING_TOWNHALL:
			baseBuilding.gameObject.GetComponent<Building_TownHall> ().SetUpInfoPanel ();
                buildingName = "Town Hall";
                break;

            case BaseBuilding.BUILDING_TYPE.BUILDING_WALL:
			baseBuilding.gameObject.GetComponent<Building_Walls> ().SetUpInfoPanel ();
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

		uniquePanel.SetActive (true);
    }

	public void ClosePanel()
	{
		uniquePanel.SetActive (false);
	}
}

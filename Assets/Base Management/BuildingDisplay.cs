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

    public List<GameObject> uniquePanels = new List<GameObject>();
    private GameObject uniquePanel;

    private void Awake()
    {
        foreach(GameObject panel in uniquePanels)
        {
            panel.SetActive(false);
        }
    }

    public void SetInformation(BaseBuilding baseBuilding, GameObject infoPanel)
    {
        buildingType = baseBuilding.GetBuildingType();
        
		switch(buildingType)
        {
		case BaseBuilding.BUILDING_TYPE.BUILDING_BARRACKS:
			buildingName = "Barracks";
                baseBuilding.infoPanel = uniquePanels[0];
			baseBuilding.gameObject.GetComponent<Building_Barracks> ().SetUpInfoPanel ();
                break;

            case BaseBuilding.BUILDING_TYPE.BUILDING_BLACKSMITH:
                buildingName = "Blacksmith";
                baseBuilding.infoPanel = uniquePanels[1];
                baseBuilding.gameObject.GetComponent<Building_Blacksmith>().SetUpInfoPanel();
                break;

            case BaseBuilding.BUILDING_TYPE.BUILDING_DOCK:
                buildingName = "Dock";
                baseBuilding.infoPanel = uniquePanels[2];
                baseBuilding.gameObject.GetComponent<Building_Dock>().SetUpInfoPanel();
                break;

            case BaseBuilding.BUILDING_TYPE.BUILDING_FARM:
                buildingName = "Farm";
                baseBuilding.infoPanel = uniquePanels[3];
                baseBuilding.gameObject.GetComponent<Building_Farm>().SetUpInfoPanel();
                break;

            case BaseBuilding.BUILDING_TYPE.BUILDING_HOUSE:
                buildingName = "House";
                baseBuilding.infoPanel = uniquePanels[4];
                baseBuilding.gameObject.GetComponent<Building_House>().SetUpInfoPanel();
                break;

            case BaseBuilding.BUILDING_TYPE.BUILDING_LUMBERCAMP:
                buildingName = "Lumber Camp";
                baseBuilding.infoPanel = uniquePanels[5];
                baseBuilding.gameObject.GetComponent<Building_LumberCamp>().SetUpInfoPanel();
                break;

            case BaseBuilding.BUILDING_TYPE.BUILDING_MILL:
                buildingName = "Mill";
                baseBuilding.infoPanel = uniquePanels[6];
                baseBuilding.gameObject.GetComponent<Building_Mill>().SetUpInfoPanel();
                break;

            case BaseBuilding.BUILDING_TYPE.BUILDING_MININGCAMP:
                buildingName = "Mining Camp";
                baseBuilding.infoPanel = uniquePanels[7];
                baseBuilding.gameObject.GetComponent<Building_MiningCamp>().SetUpInfoPanel();
                break;

            case BaseBuilding.BUILDING_TYPE.BUILDING_OUTPOST:
                buildingName = "Outpost";
                baseBuilding.infoPanel = uniquePanels[8];
                baseBuilding.gameObject.GetComponent<Building_Outpost>().SetUpInfoPanel();
                break;

            case BaseBuilding.BUILDING_TYPE.BUILDING_TOWNHALL:
                buildingName = "Town Hall";
                baseBuilding.infoPanel = uniquePanels[9];
                baseBuilding.gameObject.GetComponent<Building_TownHall>().SetUpInfoPanel();
                break;

            case BaseBuilding.BUILDING_TYPE.BUILDING_WALL:
                buildingName = "Wall";
                baseBuilding.infoPanel = uniquePanels[10];
                baseBuilding.gameObject.GetComponent<Building_Walls>().SetUpInfoPanel();
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

        uniquePanel = baseBuilding.infoPanel;

        uniquePanel.SetActive (true);
    }

	public void ClosePanel()
	{
		uniquePanel.SetActive (false);
	}
}

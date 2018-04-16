using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildingDisplay : MonoBehaviour {

    private string buildingName;
    private int buildingLevel;
    private int buildingHealth;
    private int buildingMaxHealth;

	private BaseBuilding buildingReference;

    private BaseBuilding.BUILDING_TYPE buildingType;

    public List<GameObject> uniquePanels = new List<GameObject>();
    private GameObject uniquePanel;
    
    public void Init()
    {
        foreach (GameObject panel in uniquePanels)
        {
            panel.SetActive(false);
        }
    }

	public BaseBuilding GetBuildingReference()
	{
		return buildingReference;
	}

	public void DestroyButton()
	{
        int limit = buildingReference.GetWorkingVillagers().Count;

        for(int i = 0; i < limit; i++)
        {
            RemoveCharacter(i);
            //buildingReference.RemoveVillagerFromWork(i);
        }

		buildingReference.DemolishBuilding ();
        ClosePanel();
	}

	public void UpgradeButton()
	{
		buildingReference.UpgradeBuilding ();
	}

    public void StartWorkButton(Button button)
    {
        if (buildingReference.IsWorking())
        {
            buildingReference.StopWorking();
            button.GetComponentInChildren<Text>().text = "Start Working";
        }
        else
        {
            buildingReference.StartWorking();
            button.GetComponentInChildren<Text>().text = "Cancel";
        }
    }

    public void SetWorkingItem()
    {
        buildingReference.SetWorkingItem();
    }

    public void SetInformation(BaseBuilding baseBuilding)
    {
		buildingReference = baseBuilding;

        buildingType = buildingReference.GetBuildingType();
        buildingName = buildingReference.GetBuildingName();
        
		switch(buildingType)
        {

            case BaseBuilding.BUILDING_TYPE.BUILDING_TOWNHALL:
                baseBuilding.infoPanel = uniquePanels[0];
                baseBuilding.gameObject.GetComponent<Building_TownHall>().SetUpInfoPanel();
                break;

            case BaseBuilding.BUILDING_TYPE.BUILDING_WALL:
                baseBuilding.infoPanel = uniquePanels[1];
                baseBuilding.gameObject.GetComponent<Building_Walls>().SetUpInfoPanel();
                break;

            case BaseBuilding.BUILDING_TYPE.BUILDING_HOUSE:
                baseBuilding.infoPanel = uniquePanels[2];
                baseBuilding.gameObject.GetComponent<Building_House>().SetUpInfoPanel();
                break;

            case BaseBuilding.BUILDING_TYPE.BUILDING_FARM:
                baseBuilding.infoPanel = uniquePanels[3];
                baseBuilding.gameObject.GetComponent<Building_Farm>().SetUpInfoPanel();
                break;

            case BaseBuilding.BUILDING_TYPE.BUILDING_LUMBERCAMP:
                baseBuilding.infoPanel = uniquePanels[4];
                baseBuilding.gameObject.GetComponent<Building_LumberCamp>().SetUpInfoPanel();
                break;

            case BaseBuilding.BUILDING_TYPE.BUILDING_MININGCAMP:
                baseBuilding.infoPanel = uniquePanels[5];
                baseBuilding.gameObject.GetComponent<Building_MiningCamp>().SetUpInfoPanel();
                break;

            case BaseBuilding.BUILDING_TYPE.BUILDING_DOCK:
                baseBuilding.infoPanel = uniquePanels[6];
                baseBuilding.gameObject.GetComponent<Building_Dock>().SetUpInfoPanel();
                break;

            case BaseBuilding.BUILDING_TYPE.BUILDING_BARRACKS:
                baseBuilding.infoPanel = uniquePanels[7];
				baseBuilding.gameObject.GetComponent<Building_Barracks> ().SetUpInfoPanel ();
                break;

            case BaseBuilding.BUILDING_TYPE.BUILDING_OUTPOST:
                baseBuilding.infoPanel = uniquePanels[8];
                baseBuilding.gameObject.GetComponent<Building_Outpost>().SetUpInfoPanel();
                break;

            case BaseBuilding.BUILDING_TYPE.BUILDING_BLACKSMITH:
                baseBuilding.infoPanel = uniquePanels[9];
                baseBuilding.gameObject.GetComponent<Building_Blacksmith>().SetUpInfoPanel();
                break;
                
            default:
                
                break;
        }

        buildingLevel = baseBuilding.GetBuildingLevel();
        buildingHealth = (int)baseBuilding.GetCurrentHealth();
        buildingMaxHealth = (int)baseBuilding.GetMaxHealth();

        GetComponentsInChildren<Text>()[0].text = buildingName;
        GetComponentsInChildren<Text>()[2].text = buildingLevel.ToString();
		GetComponentsInChildren<Text> () [4].text = baseBuilding.GetConstructor ();

        GetComponentInChildren<Slider>().value = buildingHealth / buildingMaxHealth;

        uniquePanel = baseBuilding.infoPanel;

        uniquePanel.SetActive (true);
    }

    public void RefreshPanel()
    {
        SetInformation(buildingReference);
    }

	public void ClosePanel()
	{
        if (uniquePanel != null)
        {
            uniquePanel.SetActive(false);
            uniquePanel = null;
        }
    }

	public void AddCharacter(BaseVillager newVillager, int buttonIndex)
	{
		buildingReference.AddVillagerToWork (newVillager, buttonIndex);
		buildingReference.SetUpInfoPanel ();
	}

    public void RemoveCharacter(int buttonIndex)
    {
        buildingReference.RemoveVillagerFromWork(buttonIndex);
        buildingReference.SetUpInfoPanel();
        RefreshPanel();
    }
}

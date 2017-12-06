using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class BaseManager : MonoBehaviour {

    int supplyStone;
    int supplyWood;
    int supplyFood;
    int supplyMorale;

    bool placingBuilding;

    GameObject heldBuilding;

	public GameObject buildingMenu;
	private GameObject buildingPanel;
	private Vector3 buildingPanelPositionStart;
	private Vector3 buildingPanelPositionEnd;
	private bool buildingMenuOpen = true;

    bool isUnderAttack;

    public List<BaseVillager> villagerList = new List<BaseVillager>();
    public List<BaseBuilding> toBeBuilt = new List<BaseBuilding>();
    public List<BaseBuilding> buildingList = new List<BaseBuilding>();
    public List<BaseEnemy> enemyList = new List<BaseEnemy>();

    private Camera cameraReference;
    private CameraMovement cameraMovement;

	InventoryBase inventoryReference;

    public GameObject woodText;
    public GameObject stoneText;
    public GameObject foodText;

    public void LaunchAttack()
    {
        for (int i = 0; i < 5; i++)
        {
            //Create some goblins
            GameObject newGoblin = (GameObject)Instantiate(Resources.Load("Characters/GoblinActor"));

            if (newGoblin != null)
            {
                enemyList.Add(newGoblin.GetComponent<BaseEnemy>());
                newGoblin = null;
            }
        }
    }

    protected BaseManager()
    {
        placingBuilding = false;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
			Debug.Log ("Base Manager Click");
			if (heldBuilding != null) {
				PlaceBuilding ();
			}
        }

        if(FindEnemies())
        {
            isUnderAttack = true;
        }
        else
        {
            isUnderAttack = false;
        }

        UIUpdate();
    }

    private void UIUpdate()
    {
        woodText.GetComponent<Text>().text = "Wood: " + supplyWood;
        stoneText.GetComponent<Text>().text = "Stone: " + supplyStone;
        foodText.GetComponent<Text>().text = "Food: " + supplyFood;
    }

    private bool FindEnemies()
    {
        enemyList.Clear();
        if (FindObjectsOfType<BaseEnemy>().Length > 0)
        {
            enemyList.AddRange(FindObjectsOfType<BaseEnemy>());

            return true;
        }

        return false;
    }

    private void Awake()
    {
        villagerList.AddRange(FindObjectsOfType<BaseVillager>());

		buildingPanel = buildingMenu.GetComponentInChildren<HorizontalLayoutGroup> ().gameObject;
		buildingPanelPositionStart = buildingPanel.transform.position;
		buildingPanelPositionEnd = new Vector3 (buildingPanel.transform.position.x - 880, buildingPanel.transform.position.y, buildingPanel.transform.position.z);

		cameraReference = Camera.main;
        cameraMovement = cameraReference.GetComponent<CameraMovement>();

        inventoryReference = GetComponent<InventoryBase>();
    }

    public InventoryBase GetInventory()
    {
        return inventoryReference;
    }

	public void ScrollBuildingMenu(Scrollbar scrollReference)
	{
		//Zero value = 60
		//1 value = -820
		float scrollValue = scrollReference.value;

		Vector3 newPosition = Vector3.Lerp(buildingPanelPositionStart, buildingPanelPositionEnd, scrollValue);

		buildingPanel.transform.position = newPosition;
	}

	public void SelectBuilding(int buildingType)
	{
		heldBuilding = (GameObject)Instantiate (Resources.Load("Buildings/BuildingActor"));
		heldBuilding.GetComponent<BaseBuilding>().InitBuilding ((BaseBuilding.BUILDING_TYPE)buildingType, this);
		ToggleBuildingMenu ();
		PlaceBuilding ();
	}

	void PlaceBuilding()
    {
        if (!placingBuilding)
        {
            //Create building reference to place
			I_Building buildingInterface = (I_Building)heldBuilding.GetComponent<BaseBuilding>();
			buildingInterface.SetBaseManager (this);

            placingBuilding = true;
        }
        else
        {
			Debug.Log ("Place Construction Down");
            //Place construction site on mouse position and add to list of construction areas.

            if(heldBuilding.GetComponent<BaseBuilding>().PlaceInWorld(this))
            {
                toBeBuilt.Add(heldBuilding.GetComponent<BaseBuilding>());
				Debug.Log ("Building added to list");
            }

            placingBuilding = false;
            heldBuilding = null;
        }
    }

	public void ToggleBuildingMenu()
	{
		if (buildingMenuOpen) {
			buildingMenu.SetActive (false);
			buildingMenuOpen = false;
            cameraMovement.SetCameraMovement(true);
		} else {
			buildingMenu.SetActive (true);
			buildingMenuOpen = true;
            cameraMovement.SetCameraMovement(false);
		}
	}

    public bool GetUnderAttack()
    {
        return isUnderAttack;
    }

    private void Start()
    {
		ToggleBuildingMenu ();

        supplyFood = 100;
        supplyMorale = 50;
        supplyStone = 100;
        supplyWood = 100;

        for (int i = 0; i < 5; i++)
        {
            //Create some villagers
            GameObject newVillager = (GameObject)Instantiate(Resources.Load("Characters/VillagerActor"));

            if (newVillager != null)
            {
                villagerList.Add(newVillager.GetComponent<BaseVillager>());
                newVillager = null;
            }
        }
    }

	public GameObject SpawnVillager()
    {
            //Create some villagers
            GameObject newVillager = (GameObject)Instantiate(Resources.Load("Characters/VillagerActor"));

            if (newVillager != null)
            {
                villagerList.Add(newVillager.GetComponent<BaseVillager>());
            }
		return newVillager;
    }

    public void AddResources(int resourceValue, int resourceType)
    {
        switch(resourceType)
        {
            case 0:
                supplyStone += resourceValue;
                break;

            case 1:
                supplyWood += resourceValue;
                break;

            case 2:
                supplyFood += resourceValue;
                break;
        }

    }

    public bool RemoveResources(int resourceValue, int resourceType)
    {
        int tempValue;

        switch(resourceType)
        {
            case 0:
                tempValue = supplyStone;
                tempValue -= resourceValue;

                if (tempValue >= 0)
                {
                    supplyStone -= resourceValue;
                    return true;
                }
                else { return false; }

            case 1:
                tempValue = supplyWood;
                tempValue -= resourceValue;

                if(tempValue >= 0)
                {
                    supplyWood -= resourceValue;
                    return true;
                }
                else { return false; }

            case 2:
                tempValue = supplyFood;
                tempValue -= resourceValue;

                if(tempValue >= 0)
                {
                    supplyFood -= resourceValue;
                    return true;
                }
                else { return false; }
        }

        return false;
    }

	public void SaveGame()
	{
		BinaryFormatter formatter = new BinaryFormatter ();
		FileStream fileStream = File.Create (Application.persistentDataPath + "/baseInfo.dat");

		GameData gameData = new GameData ();

		gameData.supplyStone = supplyStone;
		gameData.supplyWood = supplyWood;
		gameData.supplyFood = supplyFood;
		gameData.supplyMorale = supplyMorale;

		//Save Villagers
		foreach (BaseVillager villager in villagerList) {
			gameData.villagerList.Add (villager.Save ());
		}

		//Save Buildings
		foreach (BaseBuilding building in toBeBuilt) {
			gameData.toBeBuilt.Add (building.Save ());
		}
		foreach(BaseBuilding building in buildingList)
		{
			gameData.buildingList.Add(building.Save ());
		}

        gameData.inventoryData = inventoryReference.Save();

		formatter.Serialize (fileStream, gameData);
		fileStream.Close ();

		Debug.Log ("Game Saved");
	}

    public void Load()
    {
        LoadGame();
    }

	public bool LoadGame()
	{
		Debug.Log ("Loading Game");

		if (File.Exists (Application.persistentDataPath + "/baseInfo.dat")) {
			BinaryFormatter formatter = new BinaryFormatter ();
			FileStream fileStream = File.Open (Application.persistentDataPath + "/baseInfo.dat", FileMode.Open);
			GameData gameData = (GameData)formatter.Deserialize (fileStream);
			fileStream.Close ();

			supplyStone = gameData.supplyStone;
			supplyWood = gameData.supplyWood;
			supplyFood = gameData.supplyFood;
			supplyMorale = gameData.supplyMorale;

			foreach (BaseVillager villager in villagerList) {
				Destroy (villager.gameObject);
			}

			//Load Villagers
			foreach (VillagerData villager in gameData.villagerList) {

				GameObject newVillager = SpawnVillager ();

				BaseVillager toAdd = newVillager.GetComponent<BaseVillager> ();
				toAdd.Load (villager);
			}

			foreach (BaseBuilding building in buildingList) {
				Destroy (building.gameObject);
			}
			foreach (BaseBuilding building in toBeBuilt) {
				Destroy (building.gameObject);
			}

			//Load Buildings
			foreach (BuildingData building in gameData.toBeBuilt) {
				GameObject newBuilding = Instantiate (Resources.Load ("BuildingActor")) as GameObject;

				BaseBuilding toAdd = newBuilding.GetComponent<BaseBuilding>();
				toAdd.Load (building, this);
			}
			foreach (BuildingData building in gameData.buildingList) {
				GameObject newBuilding = Instantiate (Resources.Load ("BuildingActor")) as GameObject;

				BaseBuilding toAdd = newBuilding.GetComponent<BaseBuilding>();
				toAdd.Load (building, this);
			}

			foreach (BaseItem item in inventoryReference.itemList) {
				inventoryReference.RemoveItem (item);
			}

            //Load Inventory
            inventoryReference.Load(gameData.inventoryData);
            

			Debug.Log ("Game Loaded");

			return true;
		}
		Debug.Log ("Game Failed to Load");

		return false;
	}

}

[Serializable]
class GameData
{
	public int supplyStone;
	public int supplyWood;
	public int supplyFood;
	public int supplyMorale;

	public List<VillagerData> villagerList = new List<VillagerData>();
	public List<BuildingData> toBeBuilt = new List<BuildingData>();
	public List<BuildingData> buildingList = new List<BuildingData>();

    public InventoryData inventoryData;
}

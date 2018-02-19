using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
using UnityEngine.SceneManagement;

using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class BaseManager : MonoBehaviour {

    int supplyStone;
    int supplyWood;
    int supplyFood;
    int maxVillagers;

    bool placingBuilding;

    GameObject heldBuilding;

	public PlayerController controller;

	public GameObject questMenu;
	public GameObject characterMenu;
	public GameObject characterScroll;

	public GameObject buildingMenu;
	public GameObject buildingPanel;
	private Vector3 buildingPanelPositionStart;
	private Vector3 buildingPanelPositionEnd;

	public GameObject buildingInfo;

    bool isUnderAttack;
	bool settingUpQuest;
	bool addingToBuilding;

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
    public GameObject villagerText;

	private float attackTimer;
	private bool attackTimerSet;

    private int buildingButtonIndex;
    
    //====================//
    //Awake, Start, Update//
    //====================//

    private void Awake()
    {
        inventoryReference = GetComponent<InventoryBase>();
        villagerList.AddRange(FindObjectsOfType<BaseVillager>());

        buildingPanelPositionStart = buildingPanel.transform.position;
        buildingPanelPositionEnd = new Vector3(buildingPanel.transform.position.x - 730, buildingPanel.transform.position.y, buildingPanel.transform.position.z);

        cameraReference = Camera.main;
        cameraMovement = cameraReference.GetComponent<CameraMovement>();
    }

    private void Start()
    {
        //Make sure all menus are running so init values can be set
        buildingMenu.SetActive(true);
        questMenu.SetActive(true);
        characterMenu.SetActive(true);
        buildingInfo.SetActive(true);

        supplyFood = 100;
        maxVillagers = 0;
        supplyStone = 100;
        supplyWood = 100;

        for (int i = 0; i < 5; i++)
        {
            //Create some villagers

            characterScroll.GetComponent<CharacterDisplay>().Init(SpawnVillager());
        }

        LoadGame();


        //Create new quests
        if (GetComponent<QuestManager>().GetQuestList().Count < 1)
            GetComponent<QuestManager>().Init();

        //Close all menus after init
        buildingMenu.SetActive(false);
        questMenu.SetActive(false);
        characterMenu.SetActive(false);
        buildingInfo.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            //Debug.Log ("Base Manager Click");
            if (heldBuilding != null)
            {
                PlaceBuilding();
            }
        }

        if (FindEnemies())
        {
            isUnderAttack = true;
            attackTimer = 1.0f;
            attackTimerSet = false;
        }
        else
        {
            isUnderAttack = false;
            if (!attackTimerSet)
            {
                attackTimer = UnityEngine.Random.Range(60.0f, 120.0f);
                attackTimerSet = true;
            }
            else
            {
                attackTimer -= Time.deltaTime;
            }

        }

        if (attackTimer <= 0.0f)
        {
            LaunchAttack();
        }

        UIUpdate();
    }

    private void UIUpdate()
    {
        woodText.GetComponent<Text>().text = "Wood: " + supplyWood;
        stoneText.GetComponent<Text>().text = "Stone: " + supplyStone;
        foodText.GetComponent<Text>().text = "Food: " + supplyFood;
        villagerText.GetComponent<Text>().text = "Villagers: " + villagerList.Count + "/" + maxVillagers;
    }
    //===============//

    public void OpenQuestMenu()
	{
		ToggleQuestMenu ();
	}

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

    public void IncreaseVillagerCap(int amountToAdd)
    {
        maxVillagers += amountToAdd;
    }

    public void DecreaseVillagerCap(int amountToRemove)
    {
        maxVillagers -= amountToRemove;
    }

    public int GetVillagerCap()
    {
        return maxVillagers;
    }

    public float GetAttackTimer()
    {
        return attackTimer;
    }

	void OnApplicationQuit()
	{
		SaveGame ();
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

	public void SelectBuilding(GameObject buildingType)
	{
		heldBuilding = Instantiate (buildingType);
		heldBuilding.GetComponent<BaseBuilding>().InitBuilding (this);
		ToggleBuildingMenu ();
		PlaceBuilding ();
	}

	void PlaceBuilding()
    {
        if (!placingBuilding)
        {
            //Set placingBuilding to true
            placingBuilding = true;
        }
        else
        {
            //Debug.Log ("Place Construction Down");
            //Place construction site on mouse position and add to list of construction areas.

            if(heldBuilding.GetComponent<BaseBuilding>().PlaceInWorld())
                toBeBuilt.Add(heldBuilding.GetComponent<BaseBuilding>());
            
            placingBuilding = false;
            heldBuilding = null;
        }
    }

	public void ToggleBuildingMenu()
	{
		if (buildingMenu.activeSelf) {
			buildingMenu.SetActive (false);
            cameraMovement.SetCameraMovement(true);
		} else {
			buildingMenu.SetActive (true);
            cameraMovement.SetCameraMovement(false);
		}
	}

	public void ToggleCharacterMenu()
	{
		if (characterMenu.activeSelf) {
			characterMenu.SetActive (false);
		} else {
			characterMenu.SetActive (true);
		}	
	}

	public void SettingUpQuest(Quest activeQuest)
	{
		ToggleQuestMenu ();
		characterScroll.GetComponent<CharacterDisplay> ().OpenMenuForQuests (activeQuest);
		ToggleCharacterMenu ();
		settingUpQuest = true;
	}

	public void AddingCharacter(int buttonIndex)
	{
        if(buildingInfo.GetComponentInChildren<BuildingDisplay>().GetBuildingReference().FindVillagerSet(buttonIndex))
        {
            buildingInfo.GetComponentInChildren<BuildingDisplay>().RemoveCharacter(buttonIndex);
            //Debug.Log("Remove Character");
            return;
        }

		ToggleBuildingInfo ();
		ToggleCharacterMenu ();
		addingToBuilding = true;

        buildingButtonIndex = buttonIndex;
	}

	public void SelectCharacter(BaseVillager chosenVillager)
	{
		if (settingUpQuest) {
			characterScroll.GetComponent<CharacterDisplay> ().GetActiveQuest ().AddCharacter (chosenVillager);
			ToggleQuestMenu ();
			ToggleCharacterMenu ();
			settingUpQuest = false;
		} else if (addingToBuilding) {
			buildingInfo.GetComponentInChildren<BuildingDisplay> ().AddCharacter (chosenVillager, buildingButtonIndex);
			ToggleCharacterMenu ();
			ToggleBuildingInfo ();
			addingToBuilding = false;
		}else {
			ToggleCharacterMenu ();
		}
	}

	public void ToggleQuestMenu()
	{
		if (questMenu.activeSelf) {
			questMenu.SetActive (false);
		} else {
			questMenu.SetActive (true);
		}
	}

	public void ToggleBuildingInfo()
	{
		if (buildingInfo.activeSelf) {
			buildingInfo.SetActive (false);
		} else {
			buildingInfo.SetActive (true);
		}
	}

    public bool GetUnderAttack()
    {
        return isUnderAttack;
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

    //Resource Management//

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

    public bool RemoveBuildingResources(Dictionary<ResourceTile.RESOURCE_TYPE, int> resourceDictionary)
    {
        int tempStone = supplyStone;
        int tempFood = supplyFood;
        int tempWood = supplyWood;

        tempWood -= resourceDictionary[ResourceTile.RESOURCE_TYPE.WOOD];
        tempFood -= resourceDictionary[ResourceTile.RESOURCE_TYPE.FOOD];
        tempStone -= resourceDictionary[ResourceTile.RESOURCE_TYPE.STONE];

        if(tempWood >= 0
            && tempFood >= 0
            && tempStone >= 0)
        {
            supplyFood = tempFood;
            supplyStone = tempStone;
            supplyWood = tempWood;
            return true;
        }

        return false;
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

    //Saving and Loading//

	public void SaveGame()
	{
		BinaryFormatter formatter = new BinaryFormatter ();
		FileStream fileStream = File.Create (Application.persistentDataPath + "/baseInfo.dat");

		GameData gameData = new GameData ();

		gameData.supplyStone = supplyStone;
		gameData.supplyWood = supplyWood;
		gameData.supplyFood = supplyFood;
		gameData.maxVillagers = maxVillagers;

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

        //Save Inventory
        gameData.inventoryData = inventoryReference.Save();

        //Save timer for next attack
		gameData.attackTimer = attackTimer;

        //Save quests
        foreach(Quest quest in GetComponent<QuestManager>().GetQuestList())
        {
            gameData.questList.Add(quest.Save());
        }

		formatter.Serialize (fileStream, gameData);
		fileStream.Close ();

		Debug.Log ("Game Saved");
	}

    public void ResetSave()
    {
        File.Delete(Application.persistentDataPath + "/baseInfo.dat");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

	public bool LoadGame()
	{
		//Debug.Log ("Loading Game");

		controller.Reset ();

        //Load file
		if (File.Exists (Application.persistentDataPath + "/baseInfo.dat")) {
			BinaryFormatter formatter = new BinaryFormatter ();
			FileStream fileStream = File.Open (Application.persistentDataPath + "/baseInfo.dat", FileMode.Open);
			GameData gameData = (GameData)formatter.Deserialize (fileStream);
			fileStream.Close ();

            //Set supply values
			supplyStone = gameData.supplyStone;
			supplyWood = gameData.supplyWood;
			supplyFood = gameData.supplyFood;
			maxVillagers = gameData.maxVillagers;

            //Remove villagers from world
            foreach (BaseVillager villager in villagerList) {
				Destroy (villager.gameObject);
			}

            //Clear villager array
            villagerList.Clear();

            //Clear Character Menu
            characterScroll.GetComponent<CharacterDisplay>().RemoveAllButtons();

			//Load Villagers
			foreach (VillagerData villager in gameData.villagerList) {

				GameObject newVillager = SpawnVillager ();

				BaseVillager toAdd = newVillager.GetComponent<BaseVillager> ();
				toAdd.Load (villager);
			}

            //Load Quests
            GetComponent<QuestManager>().Load(gameData.questList);

            //Destroy all buildings in world ready to load
            foreach (BaseBuilding building in buildingList) {
				Destroy (building.gameObject);
			}
			buildingList.Clear ();
			foreach (BaseBuilding building in toBeBuilt) {
				Destroy (building.gameObject);
			}
			toBeBuilt.Clear ();

			//Load Buildings
			foreach (BuildingData building in gameData.toBeBuilt) {
				GameObject newBuilding = Instantiate (Resources.Load (building.loadPath)) as GameObject;

				BaseBuilding toAdd = newBuilding.GetComponent<BaseBuilding>();
				toAdd.Load (building, this);
                toBeBuilt.Add(toAdd);
			}
			foreach (BuildingData building in gameData.buildingList) {
				GameObject newBuilding = Instantiate (Resources.Load (building.loadPath)) as GameObject;

				BaseBuilding toAdd = newBuilding.GetComponent<BaseBuilding>();
				toAdd.Load (building, this);
                buildingList.Add(toAdd);
			}


			inventoryReference.ClearInventory ();

            //Load Inventory
            inventoryReference.Load(gameData.inventoryData);

			attackTimer = gameData.attackTimer;
			attackTimerSet = true;

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
	public int maxVillagers;

	public List<VillagerData> villagerList = new List<VillagerData>();
	public List<BuildingData> toBeBuilt = new List<BuildingData>();
	public List<BuildingData> buildingList = new List<BuildingData>();
    public List<QuestData> questList = new List<QuestData>();

	public float attackTimer;

    public InventoryData inventoryData;
}

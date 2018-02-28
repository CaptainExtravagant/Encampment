using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
using UnityEngine.SceneManagement;

using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class BaseManager : MonoBehaviour {

    protected GameObject mainUI;

    protected int supplyStone;
    protected int supplyWood;
    protected int supplyFood;
    protected int maxVillagers;

    protected bool placingBuilding;

    protected GameObject heldBuilding;

	protected PlayerController controller;

	protected GameObject questMenu;
	protected GameObject characterMenu;
	protected GameObject characterScroll;
    private GameObject characterInfo;
    private CharacterDisplay characterDisplay;

	protected GameObject buildingMenu;
	protected GameObject buildingPanel;
	private Vector3 buildingPanelPositionStart;
	private Vector3 buildingPanelPositionEnd;

    private GameObject inventoryPanel;

	protected GameObject buildingInfo;
    private BuildingDisplay buildingDisplay;

    protected bool isUnderAttack;
	protected bool settingUpQuest;
	protected bool addingToBuilding;

    protected List<BaseVillager> villagerList = new List<BaseVillager>();
    protected List<BaseBuilding> toBeBuilt = new List<BaseBuilding>();
    protected List<BaseBuilding> buildingList = new List<BaseBuilding>();
	protected List<BaseBuilding> toBeUpgraded = new List<BaseBuilding>();
    protected List<BaseEnemy> enemyList = new List<BaseEnemy>();

    private Camera cameraReference;
    private CameraMovement cameraMovement;

	InventoryBase inventoryReference;

    protected Text woodText;
    protected Text stoneText;
    protected Text foodText;
    protected Text villagerText;

	protected GameObject lossPanel;

    private QuestManager questManager;

	protected float attackTimer;
	protected bool attackTimerSet;

    protected int buildingButtonIndex;
    
    //==========================//
    //Awake, Start, Update, Quit//
    //==========================//
    private void Start()
    {
        Init();

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

            characterScroll.GetComponent<CharacterDisplay>().AddVillager(SpawnVillager());
        }

        //LoadGame();


        //Create new quests
        int questCount = questManager.GetQuestList().Count;

        for (int i = 3; i > questCount; i--)
            questManager.AddQuest();

        //Close all menus after init
        buildingMenu.SetActive(false);
        questMenu.SetActive(false);
        characterMenu.SetActive(false);
        buildingInfo.SetActive(false);
		mainUI.SetActive (true);
		lossPanel.SetActive (false);
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
    protected void UIUpdate()
    {
        woodText.GetComponent<Text>().text = "Wood: " + supplyWood;
        stoneText.GetComponent<Text>().text = "Stone: " + supplyStone;
        foodText.GetComponent<Text>().text = "Food: " + supplyFood;
        villagerText.GetComponent<Text>().text = "Villagers: " + villagerList.Count + "/" + maxVillagers;
    }
    void OnApplicationQuit()
    {
        SaveGame();
    }
    public void RestartGame()
    {
        ResetSave();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void ReturnToMenu()
    {
        File.Delete(Application.persistentDataPath + "/baseInfo.dat");
        SceneManager.LoadScene(0);
    }
    private void GameLost()
    {
        buildingInfo.SetActive(false);
        buildingMenu.SetActive(false);
        characterMenu.SetActive(false);
        mainUI.SetActive(false);

        lossPanel.SetActive(true);
    }

    protected void Init()
    {
        villagerList.AddRange(FindObjectsOfType<BaseVillager>());

        cameraReference = Camera.main;
        cameraMovement = cameraReference.GetComponent<CameraMovement>();

        controller = cameraReference.GetComponent<PlayerController>();

        mainUI = Instantiate(Resources.Load("UI/MainUI")) as GameObject;
        woodText = mainUI.GetComponentsInChildren<Text>()[0];
        stoneText = mainUI.GetComponentsInChildren<Text>()[1];
        foodText = mainUI.GetComponentsInChildren<Text>()[2];
        villagerText = mainUI.GetComponentsInChildren<Text>()[3];


        questMenu = Instantiate(Resources.Load("UI/QuestMenuPanel")) as GameObject;
        characterMenu = Instantiate(Resources.Load("UI/CharacterMenuPanel")) as GameObject;
        characterScroll = characterMenu.GetComponentInChildren<CharacterDisplay>().gameObject;
        characterDisplay = characterScroll.GetComponent<CharacterDisplay>();
        characterInfo = Instantiate(Resources.Load("UI/SelectedCharacterInfo")) as GameObject;

        buildingMenu = Instantiate(Resources.Load("UI/BuildingMenuPanel")) as GameObject;
        buildingPanel = buildingMenu.GetComponentInChildren<HorizontalLayoutGroup>().gameObject;
        buildingInfo = Instantiate(Resources.Load("UI/BuildingInfoPanel")) as GameObject;
        buildingDisplay = buildingInfo.GetComponentInChildren<BuildingDisplay>();

        lossPanel = Instantiate(Resources.Load("UI/GameOverPanel")) as GameObject;

        inventoryPanel = Instantiate(Resources.Load("UI/InventoryPanel")) as GameObject;

        inventoryReference = gameObject.AddComponent<InventoryBase>();
        questManager = gameObject.AddComponent<QuestManager>();

        
        questMenu.GetComponentInChildren<Button>().onClick.AddListener(ToggleQuestMenu);
        characterMenu.GetComponentInChildren<Button>().onClick.AddListener(ToggleCharacterMenu);
        
        //Loss Panel
        lossPanel.GetComponentsInChildren<Button>()[0].onClick.AddListener(RestartGame);
        lossPanel.GetComponentsInChildren<Button>()[1].onClick.AddListener(ReturnToMenu);
        
        //Build Menu
        buildingMenu.GetComponentsInChildren<Button>()[0].onClick.AddListener(delegate { SelectBuilding(Resources.Load("Buildings/BuildingTownHall") as GameObject); });
        buildingMenu.GetComponentsInChildren<Button>()[1].onClick.AddListener(delegate { SelectBuilding(Resources.Load("Buildings/BuildingWall") as GameObject); });
        buildingMenu.GetComponentsInChildren<Button>()[2].onClick.AddListener(delegate { SelectBuilding(Resources.Load("Buildings/BuildingHouse") as GameObject); });
        buildingMenu.GetComponentsInChildren<Button>()[3].onClick.AddListener(delegate { SelectBuilding(Resources.Load("Buildings/BuildingFarm") as GameObject); });
        buildingMenu.GetComponentsInChildren<Button>()[4].onClick.AddListener(delegate { SelectBuilding(Resources.Load("Buildings/BuildingLumbercamp") as GameObject); });
        buildingMenu.GetComponentsInChildren<Button>()[5].onClick.AddListener(delegate { SelectBuilding(Resources.Load("Buildings/BuildingMiningcamp") as GameObject); });
        buildingMenu.GetComponentsInChildren<Button>()[6].onClick.AddListener(delegate { SelectBuilding(Resources.Load("Buildings/BuildingDocks") as GameObject); });
        buildingMenu.GetComponentsInChildren<Button>()[7].onClick.AddListener(delegate { SelectBuilding(Resources.Load("Buildings/BuildingBarracks") as GameObject); });
        buildingMenu.GetComponentsInChildren<Button>()[8].onClick.AddListener(delegate { SelectBuilding(Resources.Load("Buildings/BuildingOutpost") as GameObject); });
        buildingMenu.GetComponentsInChildren<Button>()[9].onClick.AddListener(delegate { SelectBuilding(Resources.Load("Buildings/BuildingBlacksmith") as GameObject); });

        //Barracks Buttons
        buildingInfo.GetComponentsInChildren<Button>()[0].onClick.AddListener(delegate { AddingCharacter(0); });
        buildingInfo.GetComponentsInChildren<Button>()[1].onClick.AddListener(delegate { AddingCharacter(1); });
        buildingInfo.GetComponentsInChildren<Button>()[2].onClick.AddListener(delegate { AddingCharacter(2); });
        buildingInfo.GetComponentsInChildren<Button>()[3].onClick.AddListener(delegate { AddingCharacter(3); });

        //Blacksmith Buttons
        buildingInfo.GetComponentsInChildren<Button>()[5].onClick.AddListener(delegate { AddingCharacter(0); });
        buildingInfo.GetComponentsInChildren<Button>()[6].onClick.AddListener(delegate { AddingCharacter(1); });

        //Farm Buttons
        buildingInfo.GetComponentsInChildren<Button>()[8].onClick.AddListener(delegate { AddingCharacter(0); });
        buildingInfo.GetComponentsInChildren<Button>()[9].onClick.AddListener(delegate { AddingCharacter(1); });
        buildingInfo.GetComponentsInChildren<Button>()[10].onClick.AddListener(delegate { AddingCharacter(2); });
        buildingInfo.GetComponentsInChildren<Button>()[11].onClick.AddListener(delegate { AddingCharacter(3); });

        //Lumbermill Buttons
        buildingInfo.GetComponentsInChildren<Button>()[12].onClick.AddListener(delegate { AddingCharacter(0); });
        buildingInfo.GetComponentsInChildren<Button>()[13].onClick.AddListener(delegate { AddingCharacter(1); });
        buildingInfo.GetComponentsInChildren<Button>()[14].onClick.AddListener(delegate { AddingCharacter(2); });
        buildingInfo.GetComponentsInChildren<Button>()[15].onClick.AddListener(delegate { AddingCharacter(3); });

        //MiningCamp Buttons
        buildingInfo.GetComponentsInChildren<Button>()[16].onClick.AddListener(delegate { AddingCharacter(0); });
        buildingInfo.GetComponentsInChildren<Button>()[17].onClick.AddListener(delegate { AddingCharacter(1); });
        buildingInfo.GetComponentsInChildren<Button>()[18].onClick.AddListener(delegate { AddingCharacter(2); });
        buildingInfo.GetComponentsInChildren<Button>()[19].onClick.AddListener(delegate { AddingCharacter(3); });

        //Outpost Buttons
        buildingInfo.GetComponentsInChildren<Button>()[20].onClick.AddListener(delegate { AddingCharacter(0); });
        buildingInfo.GetComponentsInChildren<Button>()[21].onClick.AddListener(delegate { AddingCharacter(1); });

        //Townhall Buttons
        buildingInfo.GetComponentsInChildren<Button>()[22].onClick.AddListener(delegate { AddingCharacter(0); });
        buildingInfo.GetComponentsInChildren<Button>()[23].onClick.AddListener(delegate { AddingCharacter(1); });
        buildingInfo.GetComponentsInChildren<Button>()[24].onClick.AddListener(delegate { AddingCharacter(2); });
        buildingInfo.GetComponentsInChildren<Button>()[25].onClick.AddListener(delegate { AddingCharacter(3); });

        controller.Init(inventoryPanel, characterInfo, buildingInfo, this);
        characterDisplay.Init(this, characterMenu.GetComponentInChildren<Scrollbar>());
        questManager.Init(questMenu, questMenu.GetComponentInChildren<HorizontalLayoutGroup>().gameObject);
        inventoryReference.Init(inventoryPanel, controller);
        
        buildingPanelPositionStart = buildingPanel.transform.position;
        buildingPanelPositionEnd = new Vector3(buildingPanel.transform.position.x - 730, buildingPanel.transform.position.y, buildingPanel.transform.position.z);

        buildingMenu.GetComponentInChildren<Scrollbar>().onValueChanged.AddListener(ScrollBuildingMenu);

        buildingDisplay.Init();
        
        mainUI.GetComponentsInChildren<Button>()[0].onClick.AddListener(controller.OpenInventory);
        mainUI.GetComponentsInChildren<Button>()[1].onClick.AddListener(ToggleBuildingMenu);
        mainUI.GetComponentsInChildren<Button>()[2].onClick.AddListener(OpenQuestMenu);
        mainUI.GetComponentsInChildren<Button>()[3].onClick.AddListener(ToggleCharacterMenu);
        mainUI.GetComponentsInChildren<Button>()[4].onClick.AddListener(RestartGame);

        Debug.Log("Init Successful");
    }
    //===============//

    //====//
    //GETS//
    //====//

    public GameObject GetMainUI()
    {
        return mainUI;
    }
    public BuildingDisplay GetBuildingDisplay()
    {
        return buildingDisplay;
    }
    public CharacterDisplay GetCharacterDisplay()
    {
        return characterDisplay;
    }
    public int GetVillagerCount()
    {
        return villagerList.Count;
    }

    public int GetBuildingCount()
    {
        return buildingList.Count;
    }
    public List<BaseBuilding> GetBuildingList()
    {
        return buildingList;
    }

    public int GetToBeBuiltCount()
    {
        return toBeBuilt.Count;
    }
    public List<BaseBuilding> GetToBeBuiltList()
    {
        return toBeBuilt;
    }

    public int GetUpgradedCount()
    {
        return toBeUpgraded.Count;
    }
    public List<BaseBuilding> GetUpgradedList()
    {
        return toBeUpgraded;
    }

    public List<BaseEnemy> GetEnemyList()
    {
        return enemyList;
    }
    public int GetEnemyCount()
    {
        return enemyList.Count;
    }

    public GameObject GetCharacterScroll()
    {
        return characterScroll;
    }

    public QuestManager GetQuestManager()
    {
        return questManager;
    }
    //====//


    //======//
    //Quests//
    //======//
    public void OpenQuestMenu()
	{
		ToggleQuestMenu ();
    }
    public void SettingUpQuest(Quest activeQuest)
    {
        ToggleQuestMenu();
        characterScroll.GetComponent<CharacterDisplay>().OpenMenuForQuests(activeQuest);
        ToggleCharacterMenu();
        settingUpQuest = true;
    }
    public void ToggleQuestMenu()
    {
        if (questMenu.activeSelf)
        {
            questMenu.SetActive(false);
        }
        else
        {
            questMenu.SetActive(true);
        }
    }
    //======//
    

    //=========//
    //Villagers//
    //=========//
    public void IncreaseVillagerCap(int amountToAdd)
    {
        maxVillagers += amountToAdd;
    }
    public void DecreaseVillagerCap(int amountToRemove)
    {
        maxVillagers -= amountToRemove;
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
    public void CheckVillagerCount()
    {
        if (villagerList.Count <= 0)
        {
            GameLost();
        }
    }
    public int GetVillagerCap()
    {
        return maxVillagers;
    }
    public virtual void SelectCharacter(BaseVillager chosenVillager)
    {
        if (settingUpQuest)
        {
            characterScroll.GetComponent<CharacterDisplay>().GetActiveQuest().AddCharacter(chosenVillager);
            ToggleQuestMenu();
            ToggleCharacterMenu();
            settingUpQuest = false;
        }
        else if (addingToBuilding)
        {
            buildingInfo.GetComponentInChildren<BuildingDisplay>().AddCharacter(chosenVillager, buildingButtonIndex);
            ToggleCharacterMenu();
            ToggleBuildingInfo();
            addingToBuilding = false;
        }
        else
        {
            ToggleCharacterMenu();
        }
    }

    public void AddVillagerToList(BaseVillager villager)
    {
        villagerList.Add(villager);
    }
    public void RemoveVillagerFromList(BaseVillager villager)
    {
        villagerList.Remove(villager);
    }

    public List<BaseVillager> GetVillagerList()
    {
        return villagerList;
    }

    public int GetVillagerIndex(BaseVillager villager)
    {
        return villagerList.IndexOf(villager);
    }
    public BaseVillager GetVillager(int index)
    {
        return villagerList[index];
    }
    //=========//


    //======//
    //Attack//
    //======//
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
    public float GetAttackTimer()
    {
        return attackTimer;
    }
    protected bool FindEnemies()
    {
        enemyList.Clear();
        if (FindObjectsOfType<BaseEnemy>().Length > 0)
        {
            enemyList.AddRange(FindObjectsOfType<BaseEnemy>());

            return true;
        }

        return false;
    }
    public bool GetUnderAttack()
    {
        return isUnderAttack;
    }

    //======//


    //=====//
    //Menus//
    //=====//
    public virtual void ToggleBuildingInfo()
    {
        if (buildingInfo.activeSelf)
        {
            //buildingInfo.SetActive(false);
        }
        else
        {
           //buildingInfo.SetActive(true);
        }
    }
    public void ScrollBuildingMenu(float value)
	{
		//Zero value = 60
		//1 value = -820

		Vector3 newPosition = Vector3.Lerp(buildingPanelPositionStart, buildingPanelPositionEnd, value);

		buildingPanel.transform.position = newPosition;
    }
    public void ToggleBuildingMenu()
    {
        if (buildingMenu.activeSelf)
        {
            buildingMenu.SetActive(false);
        }
        else
        {
            buildingMenu.SetActive(true);
        }
    }
    public void ToggleCharacterMenu()
    {
        if (characterMenu.activeSelf)
        {
            characterMenu.SetActive(false);
        }
        else
        {
            characterMenu.SetActive(true);
        }
    }
    public void AddingCharacter(int buttonIndex)
    {
        if (buildingInfo.GetComponentInChildren<BuildingDisplay>().GetBuildingReference().FindVillagerSet(buttonIndex))
        {
            buildingInfo.GetComponentInChildren<BuildingDisplay>().RemoveCharacter(buttonIndex);
            //Debug.Log("Remove Character");
            return;
        }

        ToggleBuildingInfo();
        ToggleCharacterMenu();
        addingToBuilding = true;

        buildingButtonIndex = buttonIndex;
    }

    //=====//


    //===================//
    //Resource Management//
    //===================//
    public virtual void AddResources(int resourceValue, int resourceType)
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
    protected virtual void PlaceBuilding()
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

            if (heldBuilding.GetComponent<BaseBuilding>().PlaceInWorld())
                toBeBuilt.Add(heldBuilding.GetComponent<BaseBuilding>());

            placingBuilding = false;
            heldBuilding = null;
        }
    }
    public InventoryBase GetInventory()
    {
        return inventoryReference;
    }
    public void SelectBuilding(GameObject buildingType)
    {
        heldBuilding = Instantiate(buildingType);
        heldBuilding.GetComponent<BaseBuilding>().InitBuilding(this, buildingDisplay.uniquePanels[(int)heldBuilding.GetComponent<BaseBuilding>().GetBuildingType()]);
        ToggleBuildingMenu();
        PlaceBuilding();
    }

    public void AddBuildingToList(BaseBuilding buildingToAdd, int listIndex)
    {
        switch (listIndex)
        {
            case 0:
                toBeBuilt.Add(buildingToAdd);
                break;

            case 1:
                buildingList.Add(buildingToAdd);
                break;

            case 2:
                toBeUpgraded.Add(buildingToAdd);
                break;

            default:
                break;
        }
    }

    public void RemoveBuildingFromList(BaseBuilding buildingToAdd, int listIndex)
    {
        switch (listIndex)
        {
            case 0:
                toBeBuilt.Remove(buildingToAdd);
                break;

            case 1:
                buildingList.Remove(buildingToAdd);
                break;

            case 2:
                toBeUpgraded.Remove(buildingToAdd);
                break;

            default:
                break;
        }
    }

    //===================//


    //==================//
    //Saving and Loading//
    //==================//
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
		foreach (BaseBuilding building in toBeUpgraded) {
			gameData.toBeUpgraded.Add (building.Save ());
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
			foreach (BaseBuilding building in toBeUpgraded) {
				Destroy (building.gameObject);
			}
			toBeUpgraded.Clear ();

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

			foreach (BuildingData building in gameData.toBeUpgraded) {
				GameObject newBuilding = Instantiate (Resources.Load (building.loadPath)) as GameObject;

				BaseBuilding toAdd = newBuilding.GetComponent<BaseBuilding> ();
				toAdd.Load (building, this);
				toBeUpgraded.Add (toAdd);
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

    //==================//
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
	public List<BuildingData> toBeUpgraded = new List<BuildingData> ();
    public List<QuestData> questList = new List<QuestData>();

	public float attackTimer;

    public InventoryData inventoryData;
}

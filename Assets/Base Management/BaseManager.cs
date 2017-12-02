using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    public GameObject woodText;
    public GameObject stoneText;
    public GameObject foodText;

    public void LaunchAttack()
    {
        for (int i = 0; i < 5; i++)
        {
            //Create some villagers
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
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            PlaceBuilding();
            
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
		buildingPanelPositionEnd = new Vector3 (buildingPanel.transform.position.x - 880, buildingPanel.transform.position.y);

        cameraReference = Camera.main;
        cameraMovement = cameraReference.GetComponent<CameraMovement>();
    }

	public void ScrollBuildingMenu(Scrollbar scrollReference)
	{
		//Zero value = 60
		//1 value = -820
		float scrollValue = scrollReference.value;

		Vector3 newPosition = Vector3.Lerp(buildingPanelPositionStart, buildingPanelPositionEnd, scrollValue);

		buildingPanel.transform.position = newPosition;
	}

    void PlaceBuilding()
    {
        if (!placingBuilding)
        {
            //Create building reference to place
            heldBuilding = (GameObject)Instantiate(Resources.Load("Buildings/BuildingActor"));
            heldBuilding.GetComponent<BaseBuilding>().SetBaseManager(this);

            placingBuilding = true;
        }
        else
        {
            //Place construction site on mouse position and add to list of construction areas.

            if(heldBuilding.GetComponent<BaseBuilding>().PlaceInWorld(this))
            {
                toBeBuilt.Add(heldBuilding.GetComponent<BaseBuilding>());
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

        /*for (int i = 0; i < 5; i++)
        {
            //Create some villagers
            GameObject newVillager = (GameObject)Instantiate(Resources.Load("Characters/VillagerActor"));

            if (newVillager != null)
            {
                villagerList.Add(newVillager.GetComponent<BaseVillager>());
                newVillager = null;
            }
        }*/
    }

    public void SpawnVillagers()
    {
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

}

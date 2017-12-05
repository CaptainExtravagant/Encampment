using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;


public class BaseBuilding : MonoBehaviour, I_Building {

	public virtual void WorkBuilding(BaseVillager villagerReference)
	{
	}

	protected GameObject infoPanel;
	protected bool infoPanelOpen;

    public enum BUILDING_TYPE
    {
        BUILDING_TOWNHALL = 0,
        BUILDING_WALL,
        BUILDING_HOUSE,
        BUILDING_MILL,
        BUILDING_FARM,
        BUILDING_LUMBERCAMP,
        BUILDING_MININGCAMP,
        BUILDING_DOCK,
        BUILDING_BARRACKS,
        BUILDING_OUTPOST,
        BUILDING_BLACKSMITH
    }

    protected BaseVillager[] workingVillagers;
    protected int maxWorkingVillagers;

    private bool placedInWorld;
    private bool isBuilt;

	protected int buildingCost;
	protected ResourceTile.RESOURCE_TYPE buildingResource;

    private BUILDING_TYPE buildingType;
    private int buildingLevel;

    private MeshFilter meshFilterReference;

    private Mesh buildingMesh;
    private Mesh constructionMesh;

    protected BaseManager baseManager;

    RaycastHit hit;
    Ray cursorPosition;

    private float buildTime;

	protected float workTime;
	protected float activeTimer;


	protected float baseHealthValue;
	private float maxHealth;
	private float currentHealth;

    private void Start()
    {
        //SetMesh();

		//CloseInfoPanel ();

        buildTime = 10;
		baseHealthValue = 40;

		buildingCost = 50;
		buildingResource = ResourceTile.RESOURCE_TYPE.WOOD;
    }

	public void InitBuilding(BaseBuilding.BUILDING_TYPE newType, BaseManager manager)
	{
		SetBuildingType (newType);
		baseManager = manager;

		switch (buildingType) {
		case BUILDING_TYPE.BUILDING_BARRACKS:
			gameObject.AddComponent<Building_Barracks> ();
			break;
		case BUILDING_TYPE.BUILDING_BLACKSMITH:
			gameObject.AddComponent<Building_Blacksmith> ();
			break;
		case BUILDING_TYPE.BUILDING_DOCK:
			gameObject.AddComponent<Building_Dock> ();
			break;
		case BUILDING_TYPE.BUILDING_FARM:
			gameObject.AddComponent<Building_Farm> ();
			break;
		case BUILDING_TYPE.BUILDING_HOUSE:
			gameObject.AddComponent<Building_House> ();
			break;
		case BUILDING_TYPE.BUILDING_LUMBERCAMP:
			gameObject.AddComponent<Building_LumberCamp> ();
			break;
		case BUILDING_TYPE.BUILDING_MILL:
			gameObject.AddComponent<Building_Mill> ();
			break;
		case BUILDING_TYPE.BUILDING_MININGCAMP:
			gameObject.AddComponent<Building_MiningCamp> ();
			break;
		case BUILDING_TYPE.BUILDING_OUTPOST:
			gameObject.AddComponent<Building_Outpost> ();
			break;
		case BUILDING_TYPE.BUILDING_TOWNHALL:
			gameObject.AddComponent<Building_TownHall> ();
			break;
		case BUILDING_TYPE.BUILDING_WALL:
			gameObject.AddComponent<Building_Walls> ();
			break;
		}

		baseManager.buildingList.Add (this);

	}

	private void SetMaxHealth(float constructionSkill)
	{
		//Create building health value based off construction skill of the worker, percentage value is used to create the new value
		maxHealth = baseHealthValue + (baseHealthValue * (constructionSkill / 100));
	}

	protected void UpdateCurrentHealth(float damage)
	{
		currentHealth -= damage;
	}

	protected void ResetCurrentHealth()
	{
		currentHealth = maxHealth;
	}

	virtual protected void InitInfoPanel()
	{

	}

	public void OpenInfoPanel()
	{
		infoPanel.SetActive (true);
	}

	public void CloseInfoPanel()
	{
		infoPanel.SetActive (false);
	}

    private void Update()
    {
        if (!placedInWorld)
        {
            BindToMouse();
        }
    }

    public virtual void OnClicked(BaseVillager selectedVillager)
    {

    }

    protected bool AddVillagerToWork(BaseVillager selectedVillager)
    {
        for(int i = 0; i < maxWorkingVillagers; i++)
        {
            if(workingVillagers[i] == null)
            {
                workingVillagers[i] = selectedVillager;
                return true;
            }
        }
        return false;
    }

    protected void SetBuildingType(BUILDING_TYPE newBuildingType)
    {
        buildingType = newBuildingType;
    }

    public BUILDING_TYPE GetBuildingType()
    {
        return buildingType;
    }

    private void SetMesh()
    {
        if(placedInWorld && !isBuilt)
        {
            meshFilterReference.mesh = constructionMesh;
        }
        else if(!placedInWorld)
        {
            meshFilterReference.mesh = buildingMesh;
        }
        else if(placedInWorld && isBuilt)
        {
            meshFilterReference.mesh = buildingMesh;
        }
    }

    void LoadBuilding(string path)
    {

    }
    
    void BindToMouse()
    {
        //Get the current mouse position in the world
        Vector3 currentPosition = transform.position;

        cursorPosition = Camera.main.ScreenPointToRay(Input.mousePosition);

        //Set the new position to the next rounded value, making the building snap to the grid
        if (Physics.Raycast(cursorPosition, out hit, Mathf.Infinity))
        {
            transform.position = new Vector3(Mathf.Round(hit.point.x), 0.0f, Mathf.Round(hit.point.z));
        }
    }

    public bool PlaceInWorld(BaseManager managerReference)
    {

        if (managerReference.RemoveResources(buildingCost, (int)buildingResource))
        {
			Debug.Log ("Place in World");
            placedInWorld = true;
            return true;
        }
        else
        {
            Destroy(gameObject);
            return false;
        }
    }

    public bool IsPlaced()
    {
        return placedInWorld;
    }

    public bool IsBuilt()
    {
        return isBuilt;
    }

	private void CreateBuilding(BaseVillager characterReference)
    {
        baseManager.toBeBuilt.Remove(this);
        baseManager.buildingList.Add(this);
		SetMaxHealth (characterReference.GetTaskSkills().construction);
        isBuilt = true;

        //SetMesh();
    }

    private void SetBuildTime(float newBuildTime)
    {
        buildTime = newBuildTime;
    }

	public void AddConstructionPoints(float points, BaseVillager characterReference)
    {
        buildTime -= points;
        if(buildTime <= 0)
        {
			CreateBuilding(characterReference);
        }
    }

    public float GetBuildTime()
    {
        return buildTime;
    }

	void I_Building.SetBaseManager(BaseManager managerReference)
	{
		baseManager = managerReference;
	}

	public void Load(BuildingData building, BaseManager manager)
	{
		transform.position.Set(building.positionX, building.positionY, building.positionZ);

		isBuilt = building.isBuilt;
		workTime = building.workTime;
		SetBuildTime(building.buildTime);
		buildingCost = building.buildingCost;
		buildingResource = (ResourceTile.RESOURCE_TYPE)building.buildingResource;

		buildingLevel = building.level;
		baseHealthValue = building.baseHealthValue;
		maxHealth = building.maxHealth;
		currentHealth = building.currentHealth;

		workingVillagers = building.workingVillagers;
		maxWorkingVillagers = building.maxWorkingVillagers;

		InitBuilding ((BUILDING_TYPE)building.buildingType, manager);
	}

	public BuildingData Save()
	{
		BuildingData buildingData = new BuildingData ();

		buildingData.positionX = transform.position.x;
        buildingData.positionY = transform.position.y;
        buildingData.positionZ = transform.position.z;

		buildingData.isBuilt = isBuilt;
		buildingData.workTime = workTime;
		buildingData.buildTime = buildTime;
		buildingData.buildingCost = buildingCost;
		buildingData.buildingType = (int)buildingType;
		buildingData.buildingResource = (int)buildingResource;

		buildingData.level = buildingLevel;
		buildingData.baseHealthValue = baseHealthValue;
		buildingData.maxHealth = maxHealth;
		buildingData.currentHealth = currentHealth;

		buildingData.workingVillagers = workingVillagers;
		buildingData.maxWorkingVillagers = maxWorkingVillagers;

		return buildingData;
	}

}

[Serializable]
public class BuildingData
{
    public float positionX;
    public float positionY;
    public float positionZ;

	public bool isBuilt;
	public float workTime;
	public float buildTime;
	public int buildingCost;
	public int buildingType;
	public int buildingResource;

	public int level;
	public float baseHealthValue;
	public float maxHealth;
	public float currentHealth;

	public BaseVillager[] workingVillagers;
	public int maxWorkingVillagers;
}
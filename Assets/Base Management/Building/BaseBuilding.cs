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

	protected List <BaseVillager> workingVillagers = new List <BaseVillager>();
    protected int maxWorkingVillagers;

	protected string loadPath;

	protected bool placedInWorld;
    private bool isBuilt;

	protected int buildingCost;
	protected ResourceTile.RESOURCE_TYPE buildingResource;

    private BUILDING_TYPE buildingType;
    private int buildingLevel;

    private MeshFilter meshFilterReference;

    protected Mesh buildingMesh;
    protected Mesh constructionMesh;

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
		currentHealth = baseHealthValue;
		buildingResource = ResourceTile.RESOURCE_TYPE.WOOD;
    }

	public void InitBuilding(BaseManager manager)
	{
		SetBaseManager (manager);
		meshFilterReference = GetComponent<MeshFilter> ();
	}

	protected MeshFilter GetMeshFilter()
	{
		return meshFilterReference;
	}

	protected void SetBaseManager(BaseManager manager)
	{
		baseManager = manager;
	}

	private void SetMaxHealth(float constructionSkill)
	{
		//Create building health value based off construction skill of the worker, percentage value is used to create the new value
		maxHealth = baseHealthValue + (baseHealthValue * (constructionSkill / 100));
		ResetCurrentHealth ();
	}

	public void UpdateCurrentHealth(float damage)
	{
		//Debug.Log ("Building Health: " + currentHealth);
		currentHealth -= damage;

		if (currentHealth <= 0) {
			DestroyBuilding();
		}
	}

	protected void DestroyBuilding()
	{
		if (isBuilt) {
			baseManager.buildingList.Remove (this);
			Destroy (gameObject);
		} else {
			baseManager.toBeBuilt.Remove (this);
			Destroy (gameObject);
		}
	}

	protected void ResetCurrentHealth()
	{
		currentHealth = maxHealth;
	}
    
    public int GetBuildingLevel()
    {
        return buildingLevel;
    }

    public float GetCurrentHealth()
    {
        return currentHealth;
    }

    public float GetMaxHealth()
    {
        return maxHealth;
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

	public void AddVillagerToWork(BaseVillager selectedVillager)
    {
		workingVillagers.Add(selectedVillager);
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
    
    private void BindToMouse()
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

	bool I_Building.PlaceInWorld()
	{
		if (baseManager.RemoveResources(buildingCost, (int)buildingResource))
		{
			//Debug.Log ("Place in World");
			SetPlacedInWorld (true);
			return true;
		}
		else
		{
			Debug.Log ("Failed to place in world");
			Destroy(gameObject);
			return false;
		}
	}

	protected void SetPlacedInWorld(bool isPlaced)
	{
		placedInWorld = isPlaced;
	}

    public bool IsPlaced()
    {
        return placedInWorld;
    }

    public bool IsBuilt()
    {
        return isBuilt;
    }

	public bool BuildSlotAvailable()
	{
		if (workingVillagers.Count < maxWorkingVillagers) {
			return true;
		}
		return false;
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
		transform.position = new Vector3(building.positionX, building.positionY, building.positionZ);

		loadPath = building.loadPath;

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

		SetPlacedInWorld (true);

		InitBuilding (manager);
	}

	public BuildingData Save()
	{
		BuildingData buildingData = new BuildingData ();

		buildingData.positionX = transform.position.x;
        buildingData.positionY = transform.position.y;
        buildingData.positionZ = transform.position.z;

		buildingData.loadPath = loadPath;

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

	public string loadPath;

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

	public List <BaseVillager> workingVillagers;
	public int maxWorkingVillagers;
}
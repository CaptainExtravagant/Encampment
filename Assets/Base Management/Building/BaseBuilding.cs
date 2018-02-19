using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;


public class BaseBuilding : MonoBehaviour, I_Building {

	public virtual void WorkBuilding()
	{
	}

	public GameObject infoPanel;
	protected bool infoPanelOpen;

    public enum BUILDING_TYPE
    {
        BUILDING_TOWNHALL = 0,
        BUILDING_WALL,
        BUILDING_HOUSE,
        BUILDING_FARM,
        BUILDING_LUMBERCAMP,
        BUILDING_MININGCAMP,
        BUILDING_DOCK,
        BUILDING_BARRACKS,
        BUILDING_OUTPOST,
        BUILDING_BLACKSMITH
    }

	protected List <BaseVillager> workingVillagers = new List <BaseVillager>();
	protected List <int> villagerIndexes = new List<int>();
    protected int maxWorkingVillagers;

    private bool isBeingWorked;

	protected string loadPath;

	protected bool placedInWorld;
    private bool isBuilt;

	protected int buildingCost;
	protected ResourceTile.RESOURCE_TYPE buildingResource;

    protected Dictionary<ResourceTile.RESOURCE_TYPE, int> buildingCosts = new Dictionary<ResourceTile.RESOURCE_TYPE, int>();

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
    protected bool isWorking;

	protected float baseHealthValue;
	private float maxHealth;
	private float currentHealth;

    protected void Start()
    {
        //SetMesh();

		//CloseInfoPanel ();
        
        buildTime = 10;
		baseHealthValue = 40;

		buildingCost = 50;
		currentHealth = baseHealthValue;
		buildingResource = ResourceTile.RESOURCE_TYPE.WOOD;

        SetBuildingCost();

    }

    virtual protected void SetBuildingCost()
    {
        buildingCosts.Add(ResourceTile.RESOURCE_TYPE.WOOD, 50);
        buildingCosts.Add(ResourceTile.RESOURCE_TYPE.STONE, 50);
        buildingCosts.Add(ResourceTile.RESOURCE_TYPE.FOOD, 0);
    }

	public void InitBuilding(BaseManager manager)
	{
		SetBaseManager (manager);
		meshFilterReference = GetComponent<MeshFilter> ();
        infoPanel = baseManager.buildingInfo.GetComponentInChildren<BuildingDisplay>().uniquePanels[(int)buildingType];
	}

	protected MeshFilter GetMeshFilter()
	{
		return meshFilterReference;
	}

	protected void SetBaseManager(BaseManager manager)
	{
		baseManager = manager;
	}

	protected void SetMaxHealth(float constructionSkill)
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

	virtual public void DestroyBuilding()
	{
		if (isBuilt) {
			baseManager.buildingList.Remove (this);
			Destroy (gameObject);
		} else {
			baseManager.toBeBuilt.Remove (this);
			Destroy (gameObject);
		}
		baseManager.buildingInfo.SetActive (false);
	}

	public virtual void UpgradeBuilding()
	{

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

	public virtual void SetUpInfoPanel()
	{
        
	}

    protected void Update()
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
		if (BuildSlotAvailable()) {
			workingVillagers.Add (selectedVillager);
			villagerIndexes.Add (baseManager.villagerList.IndexOf(selectedVillager));
		}
    }

    public virtual void StartWorking()
    {

    }

    public virtual void StopWorking()
    {

    }

    public virtual void SetWorkingItem()
    {

    }

    public bool IsWorking()
    {
        return isWorking;
    }

    protected void SetBuildingType(BUILDING_TYPE newBuildingType)
    {
        buildingType = newBuildingType;
    }

    public BUILDING_TYPE GetBuildingType()
    {
        return buildingType;
    }

    protected void SetMesh()
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

	public bool PlaceInWorld()
	{
		if (baseManager.RemoveBuildingResources(buildingCosts))
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
		if (!isBeingWorked) {
			return true;
		}
		return false;
	}

    public void StartWork()
    {
        isBeingWorked = true;
    }

    public bool IsBeingWorked()
    {
        return isBeingWorked;
    }

	virtual protected void CreateBuilding(BaseVillager characterReference)
    {
        gameObject.GetComponent<BoxCollider>().enabled = true;
        baseManager.toBeBuilt.Remove(this);
        baseManager.buildingList.Add(this);
		SetMaxHealth (characterReference.GetTaskSkills().construction);
        isBuilt = true;
        isBeingWorked = false;
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
		baseManager = manager;

		transform.position = new Vector3(building.positionX, building.positionY, building.positionZ);

		loadPath = building.loadPath;

		isBuilt = building.isBuilt;
		workTime = building.workTime;
        isWorking = building.isWorking;
		SetBuildTime(building.buildTime);
        activeTimer = building.activeTimer;
		buildingCost = building.buildingCost;
		buildingResource = (ResourceTile.RESOURCE_TYPE)building.buildingResource;

		buildingLevel = building.level;
		baseHealthValue = building.baseHealthValue;
		maxHealth = building.maxHealth;
		currentHealth = building.currentHealth;

		maxWorkingVillagers = building.maxWorkingVillagers;
		villagerIndexes = building.villagerIndexes;

		Debug.Log ("Villager Indexes: " + villagerIndexes.Count);

		for(int i = 0; i < villagerIndexes.Count; i++)
		{
			workingVillagers.Add(manager.villagerList[villagerIndexes[i]]);
		}

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
        buildingData.isWorking = isWorking;
		buildingData.buildTime = buildTime;
        buildingData.activeTimer = activeTimer;
		buildingData.buildingCost = buildingCost;
		buildingData.buildingType = (int)buildingType;
		buildingData.buildingResource = (int)buildingResource;

		buildingData.level = buildingLevel;
		buildingData.baseHealthValue = baseHealthValue;
		buildingData.maxHealth = maxHealth;
		buildingData.currentHealth = currentHealth;

		buildingData.maxWorkingVillagers = maxWorkingVillagers;
		buildingData.villagerIndexes = villagerIndexes;

		Debug.Log ("Villager Indexes (Save): " + buildingData.villagerIndexes);

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
    public bool isWorking;
	public float buildTime;
    public float activeTimer;
	public int buildingCost;
	public int buildingType;
	public int buildingResource;

	public int level;
	public float baseHealthValue;
	public float maxHealth;
	public float currentHealth;

	public int maxWorkingVillagers;
	public List <int> villagerIndexes;
}
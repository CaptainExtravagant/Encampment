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

    protected Dictionary<ResourceTile.RESOURCE_TYPE, int> buildingCosts = new Dictionary<ResourceTile.RESOURCE_TYPE, int>();

    private BUILDING_TYPE buildingType;
    private int buildingLevel;

    private MeshFilter meshFilterReference;

    protected Mesh buildingMesh;
    protected Mesh constructionMesh;
	protected string constructor;

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
	protected float coreModifier = 1.0f;

    protected void Start()
    {
        //SetMesh();

		//CloseInfoPanel ();
        
        buildTime = 10;
		baseHealthValue = 40;
        
		currentHealth = baseHealthValue;

        SetBuildingCost();
        
        if (isBuilt)
            GetComponent<BoxCollider>().enabled = true;

    }

    virtual protected void SetBuildingCost()
    {
        buildingCosts.Add(ResourceTile.RESOURCE_TYPE.WOOD, 50);
        buildingCosts.Add(ResourceTile.RESOURCE_TYPE.STONE, 50);
        buildingCosts.Add(ResourceTile.RESOURCE_TYPE.FOOD, 0);
    }

	public void InitBuilding(BaseManager manager, GameObject uniquePanel)
	{
		SetBaseManager (manager);
		meshFilterReference = GetComponent<MeshFilter> ();
        if (uniquePanel != null)
            infoPanel = uniquePanel;
        else
            infoPanel = baseManager.GetBuildingDisplay().uniquePanels[(int)buildingType];
	}

	protected MeshFilter GetMeshFilter()
	{
		return meshFilterReference;
	}

	public string GetConstructor()
	{
		return constructor;
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
			baseManager.RemoveBuildingFromList (this, 1);
			Destroy (gameObject);
		} else {
			baseManager.RemoveBuildingFromList(this, 0);
			Destroy (gameObject);
		}
		baseManager.ToggleBuildingInfo();
	}

	public void UpgradeBuilding()
	{
		if(baseManager.RemoveBuildingResources(buildingCosts))
		{
			baseManager.RemoveBuildingFromList(this, 1);
			baseManager.AddBuildingToList (this, 2);
		}
	}

	protected virtual void CompleteUpgrade()
	{
		Debug.Log ("Building Upgraded");

		buildingCosts[ResourceTile.RESOURCE_TYPE.FOOD] = (int)(buildingCosts[ResourceTile.RESOURCE_TYPE.FOOD] * 1.5f);
		buildingCosts[ResourceTile.RESOURCE_TYPE.WOOD] = (int)(buildingCosts[ResourceTile.RESOURCE_TYPE.WOOD] * 1.5f);
		buildingCosts[ResourceTile.RESOURCE_TYPE.STONE] = (int)(buildingCosts[ResourceTile.RESOURCE_TYPE.STONE] * 1.5f);

		coreModifier += 0.5f;
		maxHealth = maxHealth * coreModifier;
		currentHealth = maxHealth;

		buildingLevel += 1;

		SetBuildTime (buildTime * coreModifier);

		baseManager.RemoveBuildingFromList (this, 2);
		baseManager.AddBuildingToList (this, 1);
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

	public virtual void AddVillagerToWork(BaseVillager selectedVillager, int slotIndex)
    {
		if (BuildSlotAvailable()) {
			workingVillagers.Add (selectedVillager);
            selectedVillager.SelectWorkArea(this.gameObject);
            villagerIndexes.Add (baseManager.GetVillagerIndex(selectedVillager));
		}
    }

    public virtual void RemoveVillagerFromWork(int villager)
    {
        workingVillagers[villager].VillagerStopWork();
        villagerIndexes.Remove(baseManager.GetVillagerIndex((workingVillagers[villager])));
        workingVillagers.RemoveAt(villager);
    }

    public virtual bool FindVillagerSet(int index)
    {
        if (workingVillagers.Count > index)
            return true;
        else
            return false;
    }

    public List<BaseVillager> GetWorkingVillagers()
    {
        return workingVillagers;
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
			buildingCosts[ResourceTile.RESOURCE_TYPE.FOOD] = buildingCosts[ResourceTile.RESOURCE_TYPE.FOOD] / 2;
			buildingCosts[ResourceTile.RESOURCE_TYPE.WOOD] = buildingCosts[ResourceTile.RESOURCE_TYPE.WOOD] / 2;
			buildingCosts[ResourceTile.RESOURCE_TYPE.STONE] = buildingCosts[ResourceTile.RESOURCE_TYPE.STONE] / 2;

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
        baseManager.RemoveBuildingFromList(this, 0);
        baseManager.AddBuildingToList(this, 1);
		SetMaxHealth (characterReference.GetTaskSkills().construction);
        isBuilt = true;
        isBeingWorked = false;
		constructor = characterReference.GetName();
		SetBuildTime (buildTime / 2);
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
			if (!isBuilt)
				CreateBuilding (characterReference);
			else
				CompleteUpgrade ();
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

		buildingLevel = building.level;
		baseHealthValue = building.baseHealthValue;
		maxHealth = building.maxHealth;
		currentHealth = building.currentHealth;

		maxWorkingVillagers = building.maxWorkingVillagers;
		villagerIndexes = building.villagerIndexes;

		Debug.Log ("Villager Indexes: " + villagerIndexes.Count);

		for(int i = 0; i < villagerIndexes.Count; i++)
		{
            workingVillagers.Add(manager.GetVillagerList()[villagerIndexes[i]]);
            manager.GetVillagerList()[villagerIndexes[i]].SelectWorkArea(this.gameObject);
            //AddVillagerToWork(manager.villagerList[villagerIndexes[i]], villagerIndexes[i]);
		}

		constructor = building.constructor;

		SetPlacedInWorld (true);

		InitBuilding (manager, infoPanel);
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
		buildingData.buildingType = (int)buildingType;

		buildingData.level = buildingLevel;
		buildingData.baseHealthValue = baseHealthValue;
		buildingData.maxHealth = maxHealth;
		buildingData.currentHealth = currentHealth;
		buildingData.constructor = constructor;

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
	public int buildingType;

	public int level;
	public float baseHealthValue;
	public float maxHealth;
	public float currentHealth;

	public string constructor;

	public int maxWorkingVillagers;
	public List <int> villagerIndexes;
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseBuilding : MonoBehaviour, I_Building {

	public virtual void WorkBuilding(BaseVillager villagerReference)
	{
	}

	public GameObject infoPanel;
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

    public int buildingCost;
    public ResourceTile.RESOURCE_TYPE buildingResource;

    private BUILDING_TYPE buildingType;
    private int buildingLevel;

    private MeshFilter meshFilterReference;

    private Mesh buildingMesh;
    private Mesh constructionMesh;

    protected BaseManager baseManager;

    RaycastHit hit;
    Ray cursorPosition;

    private float buildTime;

    private void Start()
    {
        //SetMesh();

		CloseInfoPanel ();

        buildTime = 10;
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

    private void CreateBuilding()
    {
        baseManager.toBeBuilt.Remove(this);
        baseManager.buildingList.Add(this);
        isBuilt = true;

        //SetMesh();
    }

    private void SetBuildTime(float newBuildTime)
    {
        buildTime = newBuildTime;
    }

    public void AddConstructionPoints(float points)
    {
        buildTime -= points;
        if(buildTime <= 0)
        {
            CreateBuilding();
        }
    }

    public float GetBuildTime()
    {
        return buildTime;
    }

    public void SetBaseManager(BaseManager newManager)
    {
        baseManager = newManager;
    }
}
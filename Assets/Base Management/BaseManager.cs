using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseManager : MonoBehaviour {

    int supplyStone;
    int supplyWood;
    int supplyFood;
    int supplyMorale;

    bool placingBuilding;

    GameObject heldBuilding;

    bool isUnderAttack;

    public List<BaseVillager> villagerList = new List<BaseVillager>();
    public List<ConstructionArea> toBeBuilt = new List<ConstructionArea>();
    public List<BaseBuilding> buildingList = new List<BaseBuilding>();
    public List<BaseEnemy> enemyList = new List<BaseEnemy>();

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
    }

    void PlaceBuilding()
    {
        if (!placingBuilding)
        {
            //Create building reference to place
            heldBuilding = (GameObject)Instantiate(Resources.Load("Buildings/BuildingActor"));
            placingBuilding = true;
        }
        else
        {
            //Place construction site on mouse position and add to list of construction areas.
            GameObject constructionReference;

            constructionReference = heldBuilding.GetComponent<BaseBuilding>().PlaceInWorld(this);

            toBeBuilt.Add(constructionReference.GetComponent<ConstructionArea>());
            placingBuilding = false;
            heldBuilding = null;
        }
    }

    public bool GetUnderAttack()
    {
        return isUnderAttack;
    }

    private void Start()
    {

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

}

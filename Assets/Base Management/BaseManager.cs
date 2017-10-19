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

    List<BaseVillager> villagerList = new List<BaseVillager>();
    public List<ConstructionArea> toBeBuilt = new List<ConstructionArea>();

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

            constructionReference = heldBuilding.GetComponent<BaseBuilding>().PlaceInWorld();

            toBeBuilt.Add(constructionReference.GetComponent<ConstructionArea>());
            placingBuilding = false;
            heldBuilding = null;
        }
    }

    private void Start()
    {

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

}

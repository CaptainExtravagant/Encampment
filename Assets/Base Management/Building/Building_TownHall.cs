using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building_TownHall : BaseBuilding, I_Building {

    int villagerSlots;

	void Awake ()
	{
		SetBuildingType (BUILDING_TYPE.BUILDING_TOWNHALL);
		loadPath = "Buildings/BuildingTownHall";
		workTime = 60.0f;
		maxWorkingVillagers = 4;
        villagerSlots = 10;
		activeTimer = workTime;

        SetBuildingCost();

        SetBuildingName();
        SetBuildingFunction();
    }

	override protected void SetBuildingCost()
	{
		buildingCosts.Add(ResourceTile.RESOURCE_TYPE.WOOD, 100);
		buildingCosts.Add(ResourceTile.RESOURCE_TYPE.STONE, 100);
		buildingCosts.Add(ResourceTile.RESOURCE_TYPE.FOOD, 0);
	}

    protected override void SetBuildingName()
    {
        buildingName = "Town Hall";
    }

    protected override void SetBuildingFunction()
    {
        buildingFunction = "Bring in new villagers. Villagers with higher Charm bring more new arrivals";
    }

    new void Update()
    {
        base.Update();

        if (workingVillagers.Count > 0)
        {
            if (!baseManager.GetUnderAttack())
            {
                activeTimer -= Time.deltaTime;
            }
            infoPanel.GetComponentInChildren<UnityEngine.UI.Slider>().value = activeTimer / workTime;

            if (activeTimer <= 0)
                WorkBuilding();
        }
    }

    protected override void CreateBuilding(BaseVillager characterReference)
    {

        baseManager.IncreaseVillagerCap(villagerSlots);

        base.CreateBuilding(characterReference);
    }

    override public void WorkBuilding()
	{
		float resourceValue = 0;

		for (int i = 0; i < workingVillagers.Count; i++) {
			resourceValue += workingVillagers[i].GetCharacterInfo().characterAttributes.charm;;
		}

		resourceValue = (resourceValue / workingVillagers.Count) / 50;

		for (int i = 0; i < resourceValue; i++) {
            if(baseManager.GetVillagerList().Count < baseManager.GetVillagerCap())
			    baseManager.GetCharacterDisplay().AddVillager(baseManager.SpawnVillager());
		}

		activeTimer = workTime;
	}

	public override void SetUpInfoPanel ()
	{
		float tempValue = 0;

        for(int i = 0; i < 4; i++)
        {
            infoPanel.GetComponentsInChildren<UnityEngine.UI.Text>()[i * 2].text = "Select";
            infoPanel.GetComponentsInChildren<UnityEngine.UI.Text>()[(i * 2) + 1].text = "0";
        }

		for (int i = 0; i < workingVillagers.Count; i++)
		{
			infoPanel.GetComponentsInChildren<UnityEngine.UI.Text>()[i * 2].text = workingVillagers[i].GetCharacterInfo().characterName;
			infoPanel.GetComponentsInChildren<UnityEngine.UI.Text>()[(i * 2) + 1].text = workingVillagers[i].GetCharacterInfo().characterAttributes.charm.ToString();

			tempValue += workingVillagers[i].GetCharacterInfo().characterAttributes.charm;
		}

		tempValue = (tempValue / workingVillagers.Count) / 50;
        if (workingVillagers.Count > 0)
            infoPanel.GetComponentsInChildren<UnityEngine.UI.Text>()[9].text = Mathf.Ceil(tempValue).ToString();
        else
            infoPanel.GetComponentsInChildren<UnityEngine.UI.Text>()[9].text = "No Working Villagers";
            
		infoPanel.GetComponentInChildren<UnityEngine.UI.Slider>().value = activeTimer / workTime;

	}

    public override void DestroyBuilding()
    {
        if(IsBuilt())
            baseManager.DecreaseVillagerCap(villagerSlots);

        base.DestroyBuilding();
    }
}

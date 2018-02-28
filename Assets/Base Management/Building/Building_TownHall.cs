﻿using System.Collections;
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

	}

    new void Update()
    {
        base.Update();

        if (workingVillagers.Count > 0)
        {
            activeTimer -= Time.deltaTime;

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

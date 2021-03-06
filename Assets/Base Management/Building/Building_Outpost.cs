﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building_Outpost : BaseBuilding, I_Building {
    
	// Use this for initialization
	void Awake()
	{
		SetBuildingType (BUILDING_TYPE.BUILDING_OUTPOST);
		loadPath = "Buildings/BuildingOutpost";
        maxWorkingVillagers = 2;

        SetBuildingCost();

        SetBuildingName();
        SetBuildingFunction();
    }

    new void Update()
    {
        base.Update();

        SetUpInfoPanel();
    }

    protected override void SetBuildingName()
    {
        buildingName = "Outpost";
    }

    protected override void SetBuildingFunction()
    {
        buildingFunction = "Provides an estimation on when the next goblin attack occurs. One man gives a basic idea, two man gives an exact time.";
    }

    public override void SetUpInfoPanel()
    {
        infoPanel.GetComponentsInChildren<UnityEngine.UI.Text>()[0].text = "Select";
        infoPanel.GetComponentsInChildren<UnityEngine.UI.Text>()[1].text = "Select";

        infoPanel.GetComponentsInChildren<UnityEngine.UI.Text>()[3].text = "No Lookouts Posted";

        if (workingVillagers.Count > 0)
        {
            infoPanel.GetComponentsInChildren<UnityEngine.UI.Text>()[0].text = workingVillagers[0].GetCharacterInfo().characterName;
            if(workingVillagers.Count > 1)
            {
                infoPanel.GetComponentsInChildren<UnityEngine.UI.Text>()[1].text = workingVillagers[1].GetCharacterInfo().characterName;
                infoPanel.GetComponentsInChildren<UnityEngine.UI.Text>()[3].text = string.Format("{0}:{1:00}", (int)baseManager.GetAttackTimer() / 60, (int)baseManager.GetAttackTimer() % 60);
            }
            else
            {
                
                if (baseManager.GetAttackTimer() < 1800)
                    infoPanel.GetComponentsInChildren<UnityEngine.UI.Text>()[3].text = "Less Than 30 Minutes";
                if (baseManager.GetAttackTimer() < 900)
                    infoPanel.GetComponentsInChildren<UnityEngine.UI.Text>()[3].text = "Less Than 15 Minutes";
                if (baseManager.GetAttackTimer() < 600)
                    infoPanel.GetComponentsInChildren<UnityEngine.UI.Text>()[3].text = "Less Than 10 Minutes";
                if (baseManager.GetAttackTimer() < 300)
                    infoPanel.GetComponentsInChildren<UnityEngine.UI.Text>()[3].text = "Less Than 5 Minutes";
                if (baseManager.GetAttackTimer() < 60)
                    infoPanel.GetComponentsInChildren<UnityEngine.UI.Text>()[3].text = "Less Than A Minute";

            }
        }
    }
}

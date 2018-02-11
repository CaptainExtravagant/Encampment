using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building_MiningCamp : BaseBuilding, I_Building{
    
	void Awake()
	{
		SetBuildingType (BUILDING_TYPE.BUILDING_MININGCAMP);
		loadPath = "Buildings/BuildingMiningcamp";
		workTime = 20.0f;
        maxWorkingVillagers = 4;
        activeTimer = workTime;
	}

	new void Update()
	{
        base.Update();

		if (workingVillagers.Count > 0) {
			activeTimer -= Time.deltaTime;

			if (activeTimer <= 0)
				WorkBuilding ();
		}
	}

	override public void WorkBuilding()
	{
		float resourceValue = 0;

		for (int i = 0; i < workingVillagers.Count; i++) {
			resourceValue += workingVillagers [i].GetTaskSkills ().mining;
		}

		resourceValue = resourceValue / workingVillagers.Count;

		baseManager.AddResources ((int)resourceValue, 0);

		activeTimer = workTime;
	}

    public override void SetUpInfoPanel()
    {
        float tempValue = 0;

        for (int i = 0; i < workingVillagers.Count; i++)
        {
            infoPanel.GetComponentsInChildren<UnityEngine.UI.Text>()[i * 2].text = workingVillagers[i].GetCharacterInfo().characterName;
            infoPanel.GetComponentsInChildren<UnityEngine.UI.Text>()[(i * 2) + 1].text = workingVillagers[i].GetTaskSkills().mining.ToString();

            tempValue += workingVillagers[i].GetTaskSkills().mining;
        }

        tempValue = tempValue / workingVillagers.Count;

        infoPanel.GetComponentsInChildren<UnityEngine.UI.Text>()[9].text = tempValue.ToString();

        infoPanel.GetComponentInChildren<UnityEngine.UI.Slider>().value = activeTimer / workTime;

    }
}

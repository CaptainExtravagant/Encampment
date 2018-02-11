using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building_TownHall : BaseBuilding, I_Building {
	
	void Awake ()
	{
		SetBuildingType (BUILDING_TYPE.BUILDING_TOWNHALL);
		loadPath = "Buildings/BuildingTownHall";
		workTime = 600.0f;
		maxWorkingVillagers = 4;
		activeTimer = workTime;
	}

	override public void WorkBuilding()
	{
		float resourceValue = 0;

		for (int i = 0; i < workingVillagers.Count; i++) {
			resourceValue += workingVillagers[i].GetCharacterInfo().characterAttributes.charm;;
		}

		resourceValue = (resourceValue / workingVillagers.Count) / 10;

		for (int i = 0; i < resourceValue; i++) {
			baseManager.characterScroll.GetComponent<CharacterDisplay>().Init(baseManager.SpawnVillager());
		}

		activeTimer = workTime;
	}

	public override void SetUpInfoPanel ()
	{
		float tempValue = 0;

		for (int i = 0; i < workingVillagers.Count; i++)
		{
			infoPanel.GetComponentsInChildren<UnityEngine.UI.Text>()[i * 2].text = workingVillagers[i].GetCharacterInfo().characterName;
			infoPanel.GetComponentsInChildren<UnityEngine.UI.Text>()[(i * 2) + 1].text = workingVillagers[i].GetCharacterInfo().characterAttributes.charm.ToString();

			tempValue += workingVillagers[i].GetCharacterInfo().characterAttributes.charm;
		}

		tempValue = (tempValue / workingVillagers.Count) / 10;

		infoPanel.GetComponentsInChildren<UnityEngine.UI.Text>()[9].text = Mathf.Ceil(tempValue);

		infoPanel.GetComponentInChildren<UnityEngine.UI.Slider>().value = activeTimer / workTime;

	}
}

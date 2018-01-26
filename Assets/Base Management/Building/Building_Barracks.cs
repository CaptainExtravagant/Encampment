using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Building_Barracks : BaseBuilding, I_Building {

	private float skillBonus;

	private List<Text> textList = new List<Text> ();
	private List<Slider> sliderList = new List<Slider>();
    
	void Awake()
	{
		SetBuildingType (BUILDING_TYPE.BUILDING_BARRACKS);

		loadPath = "Buildings/BuildingBarracks";

		maxWorkingVillagers = 6;

		workTime = 10.0f;
		activeTimer = workTime;
        GetSkillBonus();
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

    private void GetSkillBonus()
    {
        skillBonus = Mathf.Ceil(10 * Mathf.Pow(GetBuildingLevel(), 10));
    }

	override public void WorkBuilding()
	{
		float bonusValue;

		for (int i = 0; i <= workingVillagers.Count; i++) {

			if (
			//If the villagers focus is lower than their curiosity then end the timer 1% of the curiostity value earlier than normal and improve the skill
				workingVillagers[i].GetCharacterInfo ().characterAttributes.focus < workingVillagers[i].GetCharacterInfo ().characterAttributes.curiosity
				&&
				activeTimer <= activeTimer - workingVillagers[i].GetCharacterInfo ().characterAttributes.curiosity / 100) {

				GetSkillBonus ();
				workingVillagers[i].AddExperience ((int)skillBonus);
				activeTimer = workTime;

			} else if (activeTimer <= 0.0f) {
				GetSkillBonus ();
				bonusValue = skillBonus + (workingVillagers[i].GetCharacterInfo ().characterAttributes.focus / 100);

				workingVillagers[i].AddExperience ((int)bonusValue);
				activeTimer = workTime;
			}
		}
		
	}

	public override void SetUpInfoPanel ()
	{
		textList.AddRange (infoPanel.GetComponentsInChildren<Text> ());
		sliderList.AddRange (infoPanel.GetComponentsInChildren<Slider> ());

		//Set values for the info panel, skip the first text entry as this isn't button text
		for (int i = 1; i <= textList.Count; i++) {
			if (workingVillagers.Count >= i) {
				textList [i].text = workingVillagers [i - 1].GetCharacterInfo ().characterName;
				sliderList [i - 1].value = workingVillagers [i - 1].GetExperience () / workingVillagers [i - 1].GetNextLevelExperience ();
			} else {
				//textList [i].text = "Select";
			}
		}
	}
}

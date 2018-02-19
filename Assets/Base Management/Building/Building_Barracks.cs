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

		maxWorkingVillagers = 4;

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
        skillBonus = Mathf.Ceil(10 * Mathf.Pow(GetBuildingLevel(), 10)) * 10;
    }

	override public void WorkBuilding()
	{
		float bonusValue;

		for (int i = 0; i < workingVillagers.Count; i++) {

			if (
			//If the villagers focus is lower than their curiosity then end the timer 1% of the curiostity value earlier than normal and improve the skill
				workingVillagers[i].GetCharacterInfo ().characterAttributes.focus < workingVillagers[i].GetCharacterInfo ().characterAttributes.curiosity
				&&
				activeTimer <= activeTimer - workingVillagers[i].GetCharacterInfo ().characterAttributes.curiosity / 100) {

				GetSkillBonus ();
				workingVillagers[i].AddExperience ((int)skillBonus);
				activeTimer = workTime;
                SetUpInfoPanel();

			} else if (activeTimer <= 0.0f) {
				GetSkillBonus ();
				bonusValue = skillBonus + (workingVillagers[i].GetCharacterInfo ().characterAttributes.focus / 100);

				workingVillagers[i].AddExperience ((int)bonusValue);
				activeTimer = workTime;
                SetUpInfoPanel();
			}
		}
		
	}

	public override void SetUpInfoPanel ()
	{
		textList.Clear ();
		sliderList.Clear ();

		textList.AddRange (infoPanel.GetComponentsInChildren<Text> ());
		sliderList.AddRange (infoPanel.GetComponentsInChildren<Slider> ());

        for(int i = 0; i < 4; i++)
        {
            textList[i + 1].text = "Select";
            sliderList[i].value = 0;
        }

		//Debug.Log ("Text " + textList.Count);
		//Debug.Log ("Sliders " + sliderList.Count);
		//Debug.Log ("Workers " + workingVillagers.Count);

		//Set for the info panel, skip the first text entry as this isn't button text
		if (workingVillagers.Count > 0) {
			for (int i = 0; i < workingVillagers.Count; i++) {
				textList [i+1].text = 
					workingVillagers [i].GetCharacterInfo ().characterName;

                Debug.Log(workingVillagers[i].GetExperience());
                Debug.Log(workingVillagers[i].GetNextLevelExperience());

				sliderList [i].value = 
					workingVillagers [i].GetExperience () / 
					workingVillagers [i].GetNextLevelExperience ();
			}
		}
	}
}

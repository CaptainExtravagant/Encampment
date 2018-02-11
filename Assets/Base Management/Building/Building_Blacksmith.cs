using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building_Blacksmith : BaseBuilding, I_Building {

	private float itemValue;
    private UnityEngine.UI.Dropdown dropdown;

	BaseWeapon chosenItem;

    private void Awake()
    {
        SetBuildingType(BUILDING_TYPE.BUILDING_BLACKSMITH);

		loadPath = "Buildings/BuildingBlacksmith";

        maxWorkingVillagers = 2;

		workTime = 20.0f;
		activeTimer = workTime;
    }

    public override void OnClicked(BaseVillager selectedVillager)
    {
		if (selectedVillager != null) {
			if (IsBuilt ()) {
				if (BuildSlotAvailable()) {
					selectedVillager.SetTarget (this.gameObject);
					AddVillagerToWork (selectedVillager);
				}
			}
		}
    }

	new void Update()
	{
        base.Update();

		if (workingVillagers.Count > 0 && isWorking) {
			activeTimer -= Time.deltaTime;
            infoPanel.GetComponentInChildren<UnityEngine.UI.Slider>().value = activeTimer / workTime;

			if (activeTimer <= 0)
				WorkBuilding ();
		}
	}

    public override void StartWorking()
    {
        isWorking = true;
    }

    public override void StopWorking()
    {
        isWorking = false;
        activeTimer = workTime;
    }

    public override void SetWorkingItem()
    {
        chosenItem = null;

        switch(dropdown.value)
        {
            case 0:
                chosenItem = new Weapon_Axe();
                break;

            case 1:
                chosenItem = new Weapon_Bow();
                break;

            case 2:
                chosenItem = new Weapon_Longsword();
                break;

            case 3:
                chosenItem = new Weapon_Polearm();
                break;

            case 4:
                chosenItem = new Weapon_Sword();
                break;

            case 5:
                chosenItem = new Item_Shield();
                break;
        }
    }

    override public void WorkBuilding()
	{
        isWorking = false;
        activeTimer = workTime;
        
		I_Inventory inventory = baseManager.GetInventory();

		I_Item itemInterface = chosenItem;

        itemInterface.CalculateBaseStats(workingVillagers[0]);

		inventory.AddItem (chosenItem);

        SetUpInfoPanel();

	}

    public override void SetUpInfoPanel()
    {
        dropdown = infoPanel.GetComponentInChildren<UnityEngine.UI.Dropdown>();

        if (workingVillagers.Count > 0)
            infoPanel.GetComponentsInChildren<UnityEngine.UI.Text>()[1].text = workingVillagers[0].GetCharacterInfo().characterName;
        else
            infoPanel.GetComponentsInChildren<UnityEngine.UI.Text>()[1].text = "Select";

        if (workingVillagers.Count > 1)
            infoPanel.GetComponentsInChildren<UnityEngine.UI.Text>()[3].text = workingVillagers[1].GetCharacterInfo().characterName;
        else
            infoPanel.GetComponentsInChildren<UnityEngine.UI.Text>()[3].text = "Select";

        if(IsWorking())
            infoPanel.GetComponentsInChildren<UnityEngine.UI.Button>()[2].GetComponentInChildren<UnityEngine.UI.Text>().text = "Cancel";
        else
            infoPanel.GetComponentsInChildren<UnityEngine.UI.Button>()[2].GetComponentInChildren<UnityEngine.UI.Text>().text = "Start Working";
    }
}

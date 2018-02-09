using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building_Blacksmith : BaseBuilding, I_Building {

	private float itemValue;
    private UnityEngine.UI.Dropdown dropdown;

	BaseItem chosenItem;

    private void Awake()
    {
        SetBuildingType(BUILDING_TYPE.BUILDING_BLACKSMITH);

		loadPath = "Buildings/BuildingBlacksmith";

        maxWorkingVillagers = 2;

		workTime = 120.0f;
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

            case 6:
                chosenItem = new BaseArmor();
                break;
        }
    }

    override public void WorkBuilding()
	{
		GameObject newObject;

		I_Inventory inventory = baseManager.GetInventory();
			
		newObject = Instantiate (chosenItem.gameObject);

		I_Item itemInterface = (I_Item)newObject.GetComponent<BaseItem> ();

		itemInterface.CalculateBaseStats (workingVillagers[0]);

		inventory.AddItem (newObject.GetComponent<BaseItem>());

		activeTimer = workTime;
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
    }
}

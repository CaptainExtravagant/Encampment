using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building_Blacksmith : BaseBuilding, I_Building {

	private float itemValue;
    private UnityEngine.UI.Dropdown dropdown;

    private BaseVillager masterVillager;
    private BaseVillager apprenticeVillager;

	BaseWeapon chosenItem;

    private void Awake()
    {
        SetBuildingType(BUILDING_TYPE.BUILDING_BLACKSMITH);

		loadPath = "Buildings/BuildingBlacksmith";

        maxWorkingVillagers = 2;

		workTime = 20.0f;
		activeTimer = workTime;
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
        
        if (masterVillager)
            infoPanel.GetComponentsInChildren<UnityEngine.UI.Text>()[1].text = masterVillager.GetCharacterInfo().characterName;
        else
            infoPanel.GetComponentsInChildren<UnityEngine.UI.Text>()[1].text = "Select";

        if (apprenticeVillager)
            infoPanel.GetComponentsInChildren<UnityEngine.UI.Text>()[3].text = apprenticeVillager.GetCharacterInfo().characterName;
        else
            infoPanel.GetComponentsInChildren<UnityEngine.UI.Text>()[3].text = "Select";

        if(IsWorking())
            infoPanel.GetComponentsInChildren<UnityEngine.UI.Button>()[2].GetComponentInChildren<UnityEngine.UI.Text>().text = "Cancel";
        else
            infoPanel.GetComponentsInChildren<UnityEngine.UI.Button>()[2].GetComponentInChildren<UnityEngine.UI.Text>().text = "Start Working";
    }

    public override void AddVillagerToWork(BaseVillager selectedVillager, int slotIndex)
    {
        if(slotIndex == 0)
        {
            workingVillagers.Add(selectedVillager);
            selectedVillager.SelectWorkArea(this.gameObject);
            villagerIndexes.Add(baseManager.GetVillagerIndex(selectedVillager));
            masterVillager = selectedVillager;
        }
        else if(slotIndex == 1)
        {
            workingVillagers.Add(selectedVillager);
            selectedVillager.SelectWorkArea(this.gameObject);
            villagerIndexes.Add(baseManager.GetVillagerIndex(selectedVillager));
            apprenticeVillager = selectedVillager;
        }
    }

    public override void RemoveVillagerFromWork(int villager)
    {
        if(villager == 0)
        {
        masterVillager.VillagerStopWork();
        villagerIndexes.Remove(baseManager.GetVillagerIndex(masterVillager));
        workingVillagers.RemoveAt(workingVillagers.IndexOf(masterVillager));
        masterVillager = null;
        }
        else if(villager == 1)
        {
            apprenticeVillager.VillagerStopWork();
            villagerIndexes.Remove(baseManager.GetVillagerIndex(apprenticeVillager));
            workingVillagers.RemoveAt(workingVillagers.IndexOf(apprenticeVillager));
            apprenticeVillager = null;
        }
    }

    public override bool FindVillagerSet(int index)
    {
        if(index == 0)
        {
            if(masterVillager != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else if(index == 1)
        {
            if(apprenticeVillager != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        return false;
    }
}

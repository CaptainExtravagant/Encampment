using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building_Blacksmith : BaseBuilding, I_Building {

	private float itemValue;

	public BaseItem chosenItem;

	bool I_Building.PlaceInWorld()
	{
		BaseBuilding buildingParent = GetComponent<BaseBuilding> ();

		if (buildingParent.IsPlaced ()) {
			SetPlacedInWorld (true);
		}

		return false;
	}

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

	void Update()
	{
		if (workingVillagers.Count > 0) {
			activeTimer -= Time.deltaTime;

			if (activeTimer <= 0)
				WorkBuilding ();
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
}

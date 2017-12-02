using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building_Blacksmith : BaseBuilding {

	private float itemValue;

	public BaseItem chosenItem;

	protected override void InitInfoPanel ()
	{
		
	}

    private void Awake()
    {
        SetBuildingType(BUILDING_TYPE.BUILDING_BLACKSMITH);
        maxWorkingVillagers = 2;

		workTime = 120.0f;
		activeTimer = workTime;
    }

    public override void OnClicked(BaseVillager selectedVillager)
    {
		if (selectedVillager != null) {
			if (IsBuilt ()) {
				if (AddVillagerToWork (selectedVillager)) {
					selectedVillager.SetTarget (this.gameObject);
				}
			}
		} else if (!infoPanelOpen) {
			OpenInfoPanel ();
			infoPanelOpen = true;
		} else if (infoPanelOpen) {
			CloseInfoPanel ();
			infoPanelOpen = false;
		}
    }

	override public void WorkBuilding(BaseVillager villagerReference)
	{
		GameObject newObject;
		activeTimer -= Time.deltaTime;


		if (activeTimer <= 0.0f) {
			
			I_Inventory inventory = baseManager.inventoryReference;
			
			newObject = Instantiate (chosenItem.gameObject);

			I_Item itemInterface = (I_Item)newObject.GetComponent<BaseItem> ();

			itemInterface.CalculateBaseStats (villagerReference);

			inventory.AddItem (newObject.GetComponent<BaseItem>());

			activeTimer = workTime;
		}
	}
}

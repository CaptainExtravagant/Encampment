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

	}
}

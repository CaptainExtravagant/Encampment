using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building_Blacksmith : BaseBuilding {

    private void Awake()
    {
        SetBuildingType(BUILDING_TYPE.BUILDING_BLACKSMITH);
        maxWorkingVillagers = 2;
    }

    public override void OnClicked(BaseVillager selectedVillager)
    {
        if(selectedVillager != null)
        {
            if(IsBuilt())
            {
                if(AddVillagerToWork(selectedVillager))
                {
                    selectedVillager.SetTarget(this.gameObject);
                }

            }
        }
        else
        {
            //Open Building info panel
        }
    }
}

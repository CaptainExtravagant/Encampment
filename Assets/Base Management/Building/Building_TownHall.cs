using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building_TownHall : BaseBuilding, I_Building {
	
	void Awake ()
	{
		loadPath = "Buildings/BuildingTownHall";
	}
}

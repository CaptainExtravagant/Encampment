using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface I_Building{

	void WorkBuilding ();
	void SetBaseManager (BaseManager managerReference);
	bool PlaceInWorld ();
}

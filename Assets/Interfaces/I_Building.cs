using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface I_Building{

	void WorkBuilding (BaseVillager workingVillager);
	void SetBaseManager (BaseManager managerReference);
}

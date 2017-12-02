using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface I_Inventory{

	bool AddItem(BaseItem itemToAdd);
	bool RemoveItem(BaseItem itemToRemove);
    
}

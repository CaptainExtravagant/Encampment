using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface I_Inventory{

    bool AddItem(GameObject itemToAdd);
    bool RemoveItem(GameObject itemToRemove);
    
}

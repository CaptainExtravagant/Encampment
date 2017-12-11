using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using System;

public class InventoryBase : MonoBehaviour, I_Inventory {

	public PlayerController controller;

	private GameObject itemButtonReference;
	private GameObject[] itemButtonList = new GameObject[itemCap];

	public List<BaseItem> itemList = new List <BaseItem>(itemCap);

	private List<BaseItem> displayItems = new List <BaseItem>(itemCap);

	private List <BaseWeapon> weaponList = new List <BaseWeapon>(itemCap);
	private List <BaseArmor> armorList = new List <BaseArmor>(itemCap);

	private static int itemCap = 10;

	public GameObject inventoryScrollBox;
	public Dropdown inventoryDropdown;

	void Awake()
	{
		itemButtonReference = (GameObject)Resources.Load ("InventoryItem");
	}

	public void SortItems()
	{
		if (inventoryDropdown.value == 0) {
			displayItems.Clear ();
			displayItems.AddRange(itemList);
		} else if (inventoryDropdown.value == 1) {
			displayItems.Clear ();
			displayItems.AddRange(weaponList.ToArray());
		} else if (inventoryDropdown.value == 2) {
			displayItems.Clear ();
			displayItems.AddRange(armorList.ToArray());
		}
	}

	public void AddItem(BaseItem itemToAdd)
	{
		I_Inventory inventoryInterface = this as I_Inventory;

		inventoryInterface.AddItem (itemToAdd);
	}

	public void ClearInventory()
	{
		for (int i = 0; i < itemList.Count; i++) {
			if(itemList[i] != null)
				Destroy(itemList [i]);
		}

		for (int i = 0; i < itemButtonList.Length; i++) {
			if (itemButtonList [i] != null)
				Destroy (itemButtonList [i]);
		}
	}

	bool I_Inventory.AddItem(BaseItem itemToAdd)
    {
        //Run through entire inventory
		for(int i = 0; i < itemList.Capacity; i++)
        {
            //Find the next empty slot
            if(itemList[i] == null)
            {
				//Debug.Log ("Spawned Sword");
				GameObject newItem = new GameObject();
				newItem.AddComponent(itemToAdd.GetType ());

				itemList.Insert(i, newItem.GetComponent<BaseItem>());

                //Add the item to the inventory, the object itself is somewhere in the world when it's picked up/created
				itemButtonList[i] = 
					Instantiate(itemButtonReference,
						inventoryScrollBox.transform, true);
				
                //Create the button for the inventory
				itemButtonList [i].GetComponent<InventoryButton> ().Init (itemList[i].GetItemType(),
					itemList[i].GetItemName(),
					itemList[i].GetItemSprite(),
					this,
					newItem.GetComponent<BaseItem>(),
					controller);

				switch (itemList [i].GetItemType ()) {
				case BaseItem.ITEM_TYPE.ITEM_WEAPON:
					weaponList.Insert(i, itemList [i].GetComponent<BaseWeapon>());
					break;

				case BaseItem.ITEM_TYPE.ITEM_ARMOR:
					armorList.Insert(i, itemList [i].GetComponent<BaseArmor>());
					break;
				}
				
                return true;
            }
        }
        return false;
    }

	bool I_Inventory.RemoveItem(BaseItem itemToRemove)
    {
        //Go through entire inventory
		for (int i = 0; i < itemList.Capacity; i++)
        {
            //Find the item that needs to be removed
            if(itemList[i] == itemToRemove)
            {
                //Set the item and the inventory button to false
                itemList[i] = null;
                itemButtonList[i] = null;

                return true;
            }
        }

        return false;
    }


    public void RemoveItem(BaseItem itemToRemove)
	{
		int i = itemList.IndexOf (itemToRemove);
		Destroy (itemButtonList [i]);
		itemButtonList[i] = null;

		if (weaponList.Contains(itemToRemove as BaseWeapon)) {
			weaponList.Remove (itemToRemove as BaseWeapon);
		} else if (armorList.Contains(itemToRemove as BaseArmor)) {
			armorList.Remove (itemToRemove as BaseArmor);
		}
		itemList.Remove (itemToRemove);
	}

    public InventoryData Save()
    {
        InventoryData inventoryData = new InventoryData();

        foreach(BaseWeapon weapon in weaponList)
        {
            if(weapon != null)
                inventoryData.weaponDataList.Add(weapon.Save());
        }

        foreach(BaseArmor armor in armorList)
        {
            if(armor != null)
                inventoryData.armorDataList.Add(armor.Save());
        }

        return inventoryData;
    }

    public void Load(InventoryData inventoryData)
    {
        foreach(WeaponData weapon in inventoryData.weaponDataList)
        {
			BaseWeapon newWeapon;

			switch (weapon.weaponType) {
			case 0:
				newWeapon = new Weapon_Sword ();
				break;
			case 1:
				newWeapon = new Weapon_Fists();
				break;
			case 2:
				newWeapon = new Weapon_Axe ();
				break;
			case 3:
				newWeapon = new Weapon_Polearm ();
				break;
			case 4:
				newWeapon = new Weapon_Bow ();
				break;
			case 5:
				newWeapon = new Weapon_Longsword();
				break;
			case 6:
				newWeapon = new Item_Shield ();
				break;

			default:
				newWeapon = new Weapon_Fists ();
				break;
			}
			newWeapon.Load(weapon);

			AddItem(newWeapon);
        }

        foreach(ArmorData armor in inventoryData.armorDataList)
        {
            BaseArmor newArmor = new BaseArmor();
            newArmor.Load(armor);

			AddItem(newArmor);
        }
    }


}

[Serializable]
public class InventoryData
{
	public List<WeaponData> weaponDataList = new List<WeaponData>();
	public List<ArmorData> armorDataList = new List<ArmorData>();
}
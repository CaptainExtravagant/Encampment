using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using UnityEngine.EventSystems;

public class PlayerController : MonoBehaviour {

    Ray cursorPosition;
    RaycastHit hit;

	private bool equippingToMain;

    GameObject selectedObject;

    BaseVillager villagerReference;
    ResourceTile resourceReference;
    BaseBuilding buildingReference;

    Character.CharacterInfo selectedCharacterInfo;
    BaseVillager.TaskSkills selectedCharacterTaskSkills;

	private GameObject inventoryPanel;
    private GameObject characterPanel;
    private GameObject buildingPanel;
    private BuildingDisplay buildingDisplay;
    Text[] infoText;

    private BaseManager baseManager;

    public bool isPaused;

    public void Init(GameObject inventoryPanelIn, GameObject characterPanelIn, GameObject buildingPanelIn, BaseManager newManager)
    {
        inventoryPanel = inventoryPanelIn;
        characterPanel = characterPanelIn;
        buildingPanel = buildingPanelIn;
        buildingDisplay = buildingPanel.GetComponentInChildren<BuildingDisplay>();
        
        infoText = characterPanel.GetComponentsInChildren<Text>();

        baseManager = newManager;

        baseManager.GetMainUI().GetComponentsInChildren<Button>()[0].onClick.AddListener(OpenInventory);

        characterPanel.GetComponentsInChildren<Button>()[0].onClick.AddListener(EquipWeaponButton);
        characterPanel.GetComponentsInChildren<Button>()[1].onClick.AddListener(EquipOffhandButton);
        characterPanel.GetComponentsInChildren<Button>()[2].onClick.AddListener(EquipArmorButton);

		characterPanel.GetComponentInChildren<InputField> ().onEndEdit.AddListener (RenameCharacter);

        inventoryPanel.GetComponentInChildren<Button>().onClick.AddListener(CloseInventory);

        Reset();
    }

	public void Reset()
	{
		CloseCharacterInfoPanel ();
		CloseInventory ();
		selectedObject = null;

		villagerReference = null;
		resourceReference = null;
		buildingReference = null;

	}

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            baseManager.TogglePauseMenu(isPaused);
            isPaused = !isPaused;
        }

        if (!isPaused)
        {

            if (Input.GetKeyDown(KeyCode.Mouse1))
            {
                CloseAllMenus();
            }

            if (!EventSystem.current.IsPointerOverGameObject())
            {

                if (Input.GetKeyDown(KeyCode.Mouse0))
                {
                    //Debug.Log ("Mouse Click");
                    selectedObject = FindObjectUnderMouse();

                    if (selectedObject != null)
                    {
                        //Debug.Log ("Object Selected");

                        if (selectedObject.GetComponent<BaseVillager>())
                        {
                            //Debug.Log ("Villager Found");

                            if (villagerReference == null)
                            {
                                villagerReference = selectedObject.GetComponent<BaseVillager>();

                                villagerReference.SetSelected(true);
                                OpenCharacterInfoPanel();
                            }
                            else
                            {
                                villagerReference.SetSelected(false);
                                villagerReference = null;
                                CloseCharacterInfoPanel();
                            }

                            if (buildingReference != null)
                            {
                                CloseBuildingInfoPanel();
                            }
                        }
                        else if (selectedObject.GetComponent<ResourceTile>())
                        {
                            //Debug.Log ("Resources Found");
                            resourceReference = selectedObject.GetComponent<ResourceTile>();

                            if (villagerReference != null)
                            {
                                //Debug.Log ("Setting Work Area (Controller)");
                                villagerReference.SelectWorkArea(resourceReference.gameObject);
                                villagerReference.SetSelected(false);
                                villagerReference = null;
                                CloseCharacterInfoPanel();
                            }

                            if (buildingReference != null)
                            {
                                CloseBuildingInfoPanel();
                            }

                            resourceReference = null;
                        }
                        else if (selectedObject.GetComponent<BaseBuilding>())
                        {

                            //Debug.Log ("Building Found");

                            if (buildingReference != null)
                            {
                                //Close panel and remove reference
                                CloseBuildingInfoPanel();
                            }
                            else
                            {
                                //Set building reference and open info panel
                                buildingReference = selectedObject.GetComponent<BaseBuilding>();
                                if (buildingReference.IsBuilt() == true)
                                    OpenBuildingInfoPanel();
                                else
                                    buildingReference = null;
                            }

                            if (villagerReference != null)
                            {

                                //buildingReference.OnClicked(villagerReference);

                                villagerReference.SetSelected(false);
                                villagerReference = null;
                                CloseCharacterInfoPanel();
                            }
                        }
                        else if (villagerReference != null)
                        {
                            villagerReference.SetTargetPosition(Camera.main.ScreenToWorldPoint(Input.mousePosition));
                            villagerReference.SetSelected(false);
                            villagerReference = null;
                            CloseCharacterInfoPanel();
                        }
                    }
                    else
                    {
                        //Debug.Log ("Object is null");
                    }

                }
            }
        }
    }

    GameObject FindObjectUnderMouse()
    {
		GameObject foundObject;

		cursorPosition = Camera.main.ScreenPointToRay (Input.mousePosition);

		if (Physics.Raycast (cursorPosition, out hit, Mathf.Infinity)) {
			foundObject = hit.collider.gameObject;
		} else {
			foundObject = null;
		}

		return foundObject;
    }

	void CloseAllMenus()
	{
		CloseBuildingInfoPanel ();
		CloseCharacterInfoPanel ();
		CloseInventory ();
		baseManager.CloseAllMenus ();
	}

    //============================
    //CHARACTER INFO PANEL METHODS
    //============================
	void RenameCharacter(string newName)
	{
		villagerReference.RenameCharacter (newName);
	}

    void OpenCharacterInfoPanel()
    {
		selectedCharacterInfo = villagerReference.GetCharacterInfo();
        selectedCharacterTaskSkills = villagerReference.GetTaskSkills();
        
		for (int i = 0; i < infoText.Length; i++) {
			switch (i)
            {
                case 1:
                    infoText[i].text = selectedCharacterInfo.characterName;
                    break;

                case 2:
                    characterPanel.GetComponentInChildren<InputField>().text = selectedCharacterInfo.characterName;
				break;

			case 4:
				if (selectedCharacterInfo.characterSex == 1) {
					infoText [i].text = "Male";
				} else {
					infoText [i].text = "Female";
				}
				break;

			case 6:
				infoText [i].text = selectedCharacterInfo.characterLevel.ToString();
				break;

			case 9:
				infoText [i].text = selectedCharacterInfo.characterAttributes.fitness.ToString ();
				break;

			case 11:
				infoText [i].text = selectedCharacterInfo.characterAttributes.nimbleness.ToString ();
				break;

			case 13:
				infoText [i].text = selectedCharacterInfo.characterAttributes.curiosity.ToString ();
				break;

			case 15:
				infoText [i].text = selectedCharacterInfo.characterAttributes.focus.ToString ();
				break;

			case 17:
				infoText [i].text = selectedCharacterInfo.characterAttributes.charm.ToString ();
				break;

			case 20:
				infoText [i].text = selectedCharacterInfo.characterCombatSkills.brawling.ToString ();
				break;

			case 22:
				infoText [i].text = selectedCharacterInfo.characterCombatSkills.sword.ToString ();
				break;

			case 24:
				infoText [i].text = selectedCharacterInfo.characterCombatSkills.longsword.ToString ();
				break;

			case 26:
				infoText [i].text = selectedCharacterInfo.characterCombatSkills.axe.ToString ();
				break;

			case 28:
				infoText [i].text = selectedCharacterInfo.characterCombatSkills.polearm.ToString ();
				break;

			case 30:
				infoText [i].text = selectedCharacterInfo.characterCombatSkills.bow.ToString ();
				break;

			case 32:
				infoText [i].text = selectedCharacterInfo.characterCombatSkills.dodge.ToString ();
				break;

			case 34:
				infoText [i].text = selectedCharacterInfo.characterCombatSkills.armor.ToString ();
				break;

			case 37:
				infoText [i].text = selectedCharacterTaskSkills.mining.ToString ();
				break;

			case 39:
				infoText [i].text = selectedCharacterTaskSkills.woodcutting.ToString ();
				break;

			case 41:
				infoText [i].text = selectedCharacterTaskSkills.blacksmithing.ToString ();
				break;

			case 43:
				infoText [i].text = selectedCharacterTaskSkills.weaponCrafting.ToString ();
				break;

			case 45:
				infoText [i].text = selectedCharacterTaskSkills.armorCrafting.ToString ();
				break;

			case 47:
				infoText [i].text = selectedCharacterTaskSkills.tailoring.ToString ();
				break;

			case 49:
				infoText [i].text = selectedCharacterTaskSkills.farming.ToString ();
				break;

			case 51:
				infoText [i].text = selectedCharacterTaskSkills.construction.ToString ();
				break;

			case 53:
				infoText [i].text = selectedCharacterTaskSkills.sailing.ToString ();
				break;

			case 55:
				infoText [i].text = villagerReference.GetEquippedWeapon ().GetItemName ();
				break;

			case 57:
				infoText [i].text = villagerReference.GetOffHandWeapon ().GetItemName ();
				break;

			case 59:
				infoText [i].text = villagerReference.GetEquippedArmor ().GetItemName ();
				break;
			}
		}

        characterPanel.SetActive(true);
    }
        public void OpenCharacterInfoPanel(BaseVillager villager)
        {
        villagerReference = villager;

        selectedCharacterInfo = villagerReference.GetCharacterInfo();
        selectedCharacterTaskSkills = villagerReference.GetTaskSkills();

		for (int i = 0; i < infoText.Length; i++) {
			switch (i) {
			case 1:
				infoText [i].text = selectedCharacterInfo.characterName;
				break;

                case 2:
                    infoText[i].text = selectedCharacterInfo.characterName;
                    break;

                case 3:
				if (selectedCharacterInfo.characterSex == 1) {
					infoText [i].text = "Male";
				} else {
					infoText [i].text = "Female";
				}
				break;

			case 5:
				infoText [i].text = selectedCharacterInfo.characterLevel.ToString();
				break;

			case 8:
				infoText [i].text = selectedCharacterInfo.characterAttributes.fitness.ToString ();
				break;

			case 10:
				infoText [i].text = selectedCharacterInfo.characterAttributes.nimbleness.ToString ();
				break;

			case 12:
				infoText [i].text = selectedCharacterInfo.characterAttributes.curiosity.ToString ();
				break;

			case 14:
				infoText [i].text = selectedCharacterInfo.characterAttributes.focus.ToString ();
				break;

			case 16:
				infoText [i].text = selectedCharacterInfo.characterAttributes.charm.ToString ();
				break;

			case 19:
				infoText [i].text = selectedCharacterInfo.characterCombatSkills.brawling.ToString ();
				break;

			case 21:
				infoText [i].text = selectedCharacterInfo.characterCombatSkills.sword.ToString ();
				break;

			case 23:
				infoText [i].text = selectedCharacterInfo.characterCombatSkills.longsword.ToString ();
				break;

			case 25:
				infoText [i].text = selectedCharacterInfo.characterCombatSkills.axe.ToString ();
				break;

			case 27:
				infoText [i].text = selectedCharacterInfo.characterCombatSkills.polearm.ToString ();
				break;

			case 29:
				infoText [i].text = selectedCharacterInfo.characterCombatSkills.bow.ToString ();
				break;

			case 31:
				infoText [i].text = selectedCharacterInfo.characterCombatSkills.dodge.ToString ();
				break;

			case 33:
				infoText [i].text = selectedCharacterInfo.characterCombatSkills.armor.ToString ();
				break;

			case 36:
				infoText [i].text = selectedCharacterTaskSkills.mining.ToString ();
				break;

			case 38:
				infoText [i].text = selectedCharacterTaskSkills.woodcutting.ToString ();
				break;

			case 40:
				infoText [i].text = selectedCharacterTaskSkills.blacksmithing.ToString ();
				break;

			case 42:
				infoText [i].text = selectedCharacterTaskSkills.weaponCrafting.ToString ();
				break;

			case 44:
				infoText [i].text = selectedCharacterTaskSkills.armorCrafting.ToString ();
				break;

			case 46:
				infoText [i].text = selectedCharacterTaskSkills.tailoring.ToString ();
				break;

			case 48:
				infoText [i].text = selectedCharacterTaskSkills.farming.ToString ();
				break;

			case 50:
				infoText [i].text = selectedCharacterTaskSkills.construction.ToString ();
				break;

			case 52:
				infoText [i].text = selectedCharacterTaskSkills.sailing.ToString ();
				break;

			case 54:
				infoText [i].text = villagerReference.GetEquippedWeapon ().GetItemName ();
				break;

			case 56:
				infoText [i].text = villagerReference.GetOffHandWeapon ().GetItemName ();
				break;

			case 58:
				infoText [i].text = villagerReference.GetEquippedArmor ().GetItemName ();
				break;
			}
		}

        characterPanel.SetActive(true);
        }

	    public void CloseCharacterInfoPanel()
    {
        characterPanel.SetActive(false);
    }

        string AttributeTextLoop()
    {
        string attributeString = "";

        for (int i = 0; i < 5; i++)
        {
            switch (i)
            {
                case 0:
                    attributeString += "Fitness: " + selectedCharacterInfo.characterAttributes.fitness.ToString() + "\n";
                    break;

                case 1:
                    attributeString += "Nimbleness: " + selectedCharacterInfo.characterAttributes.nimbleness.ToString() + "\n";
                    break;

                case 2:
                    attributeString += "Curiosity: " + selectedCharacterInfo.characterAttributes.curiosity.ToString() + "\n";
                    break;

                case 3:
                    attributeString += "Focus: " + selectedCharacterInfo.characterAttributes.focus.ToString() + "\n";
                    break;

                case 4:
                    attributeString += "Charm: " + selectedCharacterInfo.characterAttributes.charm.ToString() + "\n";
                    break;
            }
        }
        return attributeString;
    }

        string CombatTextLoop()
    {
        string skillString = "";

        for(int i = 0; i < 8; i++)
        {
            switch(i)
            {
                case 0:
                    skillString += "Brawling: " + selectedCharacterInfo.characterCombatSkills.brawling.ToString() + "\n";
                    break;

                case 1:
                    skillString += "Sword: " + selectedCharacterInfo.characterCombatSkills.sword.ToString() + "\n";
                    break;

                case 2:
                    skillString += "Longsword: " + selectedCharacterInfo.characterCombatSkills.longsword.ToString() + "\n";
                    break;

                case 3:
                    skillString += "Axe: " + selectedCharacterInfo.characterCombatSkills.axe.ToString() + "\n";
                    break;

                case 4:
                    skillString += "Polearm: " + selectedCharacterInfo.characterCombatSkills.polearm.ToString() + "\n";
                    break;

                case 5:
                    skillString += "Bow: " + selectedCharacterInfo.characterCombatSkills.bow.ToString() + "\n";
                    break;

                case 6:
                    skillString += "Dodge: " + selectedCharacterInfo.characterCombatSkills.dodge.ToString() + "\n";
                    break;

                case 7:
                    skillString += "Armor: " + selectedCharacterInfo.characterCombatSkills.armor.ToString() + "\n";
                    break;
            }
        }

        return skillString;
    }

        string TaskTextLoop()
    {
        string taskString = "";

        for(int i = 0; i < 9; i++)
        {
            switch(i)
            {
                case 0:
                    taskString += "Mining: " + selectedCharacterTaskSkills.mining.ToString() + "\n";
                    break;

                case 1:
                    taskString += "Woodcutting: " + selectedCharacterTaskSkills.woodcutting.ToString() + "\n";
                    break;

                case 2:
                    taskString += "Blacksmithing: " + selectedCharacterTaskSkills.blacksmithing.ToString() + "\n";
                    break;

                case 3:
                    taskString += "Weapon Crafting: " + selectedCharacterTaskSkills.weaponCrafting.ToString() + "\n";
                    break;

                case 4:
                    taskString += "Armor Crafting: " + selectedCharacterTaskSkills.armorCrafting.ToString() + "\n";
                    break;

                case 5:
                    taskString += "Tailoring: " + selectedCharacterTaskSkills.tailoring.ToString() + "\n";
                    break;

                case 6:
                    taskString += "Farming: " + selectedCharacterTaskSkills.farming.ToString() + "\n";
                    break;

                case 7:
                    taskString += "Construction: " + selectedCharacterTaskSkills.construction.ToString() + "\n";
                    break;

                case 8:
                    taskString += "Sailing: " + selectedCharacterTaskSkills.sailing.ToString() + "\n";
                    break;
            }
        }

        return taskString;
    }
    //============================



    //===========================
    //BUILDING INFO PANEL METHODS
    //===========================

    void OpenBuildingInfoPanel()
    {
        baseManager.ToggleBuildingInfo();
        buildingDisplay.SetInformation(buildingReference);

        buildingPanel.SetActive(true);
    }

    public void CloseBuildingInfoPanel()
    {
        baseManager.ToggleBuildingInfo();
        buildingReference = null;

        buildingDisplay.ClosePanel ();

        buildingPanel.SetActive(false);
    }

    //===========================


    //=======================
    //INVENTORY AND EQUIPPING
    //=======================
	    public void OpenInventory()
	{
		inventoryPanel.SetActive (true);
	}

	    public void CloseInventory()
	{
		inventoryPanel.SetActive (false);
	}

	    public void EquipWeaponButton()
	{
		equippingToMain = true;
		OpenInventory ();
		inventoryPanel.GetComponentInChildren<Dropdown> ().value = 1;
	}

	    public void EquipOffhandButton()
	{
		equippingToMain = false;
		OpenInventory ();
		inventoryPanel.GetComponentInChildren<Dropdown> ().value = 1;
	}

	    public void EquipArmorButton()
	{
		OpenInventory ();
		inventoryPanel.GetComponentInChildren<Dropdown> ().value = 2;
	}

	    public void InventoryButtonPressed(BaseItem item)
	{
		CloseInventory ();
		if (villagerReference != null) {
			//Debug.Log ("Villager Reference is Valid");
			if (item.GetItemType() == BaseItem.ITEM_TYPE.ITEM_WEAPON) {
				if (item.GetComponent<Item_Shield> ()) {
					villagerReference.UnequipOffHand ();
					villagerReference.EquipWeaponToOffHand (item as BaseWeapon);
					//Debug.Log ("Shield Equipped");
					OpenCharacterInfoPanel ();
					return;
				} else if (equippingToMain) {
					villagerReference.UnequipMainHand ();
					villagerReference.EquipWeaponToMainHand (item as BaseWeapon);
					//Debug.Log ("Main hand Equipped");
					OpenCharacterInfoPanel ();
					return;
				} else if (!equippingToMain) {
					villagerReference.UnequipOffHand ();
					villagerReference.EquipWeaponToOffHand (item as BaseWeapon);
					//Debug.Log ("Offhand Equipped");
					OpenCharacterInfoPanel ();
					return;
				}
			} else if (item.GetItemType() == BaseItem.ITEM_TYPE.ITEM_ARMOR) {
				villagerReference.UnequipArmor ();
				villagerReference.EquipArmor (item as BaseArmor);
				//Debug.Log ("Armor Equipped");
				OpenCharacterInfoPanel ();
				return;
			}
		}

        //If no villager reference open dialog box to destroy or sell item

		//Debug.Log ("Nothing Equipped");
	}
    //=======================
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour {

    Camera cameraReference;
    CameraMovement cameraMovement;
    Ray cursorPosition;
    RaycastHit hit;

    GameObject selectedObject;

    BaseVillager villagerReference;
    ResourceTile resourceReference;
    BaseBuilding buildingReference;

    Character.CharacterInfo selectedCharacterInfo;
    BaseVillager.TaskSkills selectedCharacterTaskSkills;

	public GameObject inventoryPanel;
    public GameObject characterPanel;
    Text[] infoText;

    private void Awake()
    {
        cameraReference = Camera.main;
        cameraMovement = cameraReference.GetComponent<CameraMovement>();

		infoText = characterPanel.GetComponentsInChildren<Text>();

        CloseCharacterInfoPanel();
		CloseInventory ();
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Mouse0))
        {
			//Debug.Log ("Mouse Click");
			selectedObject = FindObjectUnderMouse ();

			if (selectedObject != null) {
				Debug.Log ("Object Selected");

				if (selectedObject.GetComponent<BaseVillager> ()) {
					Debug.Log ("Villager Found");

                    if (villagerReference == null)
                    {
                        villagerReference = selectedObject.GetComponent<BaseVillager>();

                        villagerReference.SetSelected(true);
                        OpenCharacterInfoPanel();
                        cameraMovement.SetCameraMovement(false);
                    }
                    else
                    {
                        villagerReference.SetSelected(false);
                        villagerReference = null;
                        CloseCharacterInfoPanel();
                        cameraMovement.SetCameraMovement(true);
                    }
				}
				else if (selectedObject.GetComponent<ResourceTile> ()) {
					Debug.Log ("Resources Found");
					resourceReference = selectedObject.GetComponent<ResourceTile> ();

					if (villagerReference != null)
					{
						Debug.Log ("Setting Work Area (Controller)");
						villagerReference.SelectWorkArea (resourceReference.gameObject);
						villagerReference.SetSelected (false);
						villagerReference = null;
                        CloseCharacterInfoPanel();
                        cameraMovement.SetCameraMovement(true);
					}

					resourceReference = null;
				} else if (selectedObject.GetComponent<BaseBuilding> ()) {

					Debug.Log ("Building Found");

					buildingReference = selectedObject.GetComponent<BaseBuilding> ();
					//Assign villagers to buildings, open building menu

					if (villagerReference != null) {

                        buildingReference.OnClicked(villagerReference);

						villagerReference.SetSelected (false);
						villagerReference = null;
                        CloseCharacterInfoPanel();
                        cameraMovement.SetCameraMovement(true);
					}
				}
			}
			else
			{
				Debug.Log ("Object is null");
			}
           
        }
    }

    //============================
    //CHARACTER INFO PANEL METHODS
    //============================
    void OpenCharacterInfoPanel()
    {
        selectedCharacterInfo = villagerReference.GetCharacterInfo();
        selectedCharacterTaskSkills = villagerReference.GetTaskSkills();

		for (int i = 0; i < infoText.Length; i++) {
			switch (i) {
			case 1:
				infoText [i].text = selectedCharacterInfo.characterName;
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
			}
		}

        characterPanel.SetActive(true);
    }
    void CloseCharacterInfoPanel()
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

	public void OpenInventory()
	{
		inventoryPanel.SetActive (true);
        cameraMovement.SetCameraMovement(false);
	}

	public void CloseInventory()
	{
		inventoryPanel.SetActive (false);
        cameraMovement.SetCameraMovement(true);
	}
}

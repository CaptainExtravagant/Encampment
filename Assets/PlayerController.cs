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
    public GameObject characterPanel;
    Text infoText;

    private void Awake()
    {
        cameraReference = Camera.main;
        cameraMovement = cameraReference.GetComponent<CameraMovement>();

        infoText = characterPanel.GetComponentInChildren<Text>();

        CloseCharacterInfoPanel();
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

					villagerReference = selectedObject.GetComponent<BaseVillager> ();

					villagerReference.SetSelected (true);
                    OpenCharacterInfoPanel();
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
					}

					resourceReference = null;
				} else if (selectedObject.GetComponent<BaseBuilding> ()) {

					Debug.Log ("Building Found");

					buildingReference = selectedObject.GetComponent<BaseBuilding> ();
					//Assign villagers to buildings, open building menu

					if (villagerReference != null) {
						villagerReference.SetSelected (false);
						villagerReference = null;
                        CloseCharacterInfoPanel();
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

        infoText.text = "CHARACTER INFO \n" +
                        "\n" +
                        "Name: " + selectedCharacterInfo.characterName + "\n" +
                        "Level: " + selectedCharacterInfo.characterLevel + "\n" +
                        "\n" +
                        "CHARACTER ATTRIBUTES \n" +
                        AttributeTextLoop() + "\n" +
                        "\n" +
                        "CHARACTER COMBAT SKILLS \n" +
                        CombatTextLoop() + "\n" +
                        "\n" +
                        "CHARACTER TASK SKILLS \n" +
                        TaskTextLoop() + "\n" +
                        "\n" +
                        "EQUIPPED WEAPON: " + villagerReference.GetEquippedWeapon().GetWeaponName() + "\n" +
                        "\n" +
                        "EQUIPPED ARMOR: " + villagerReference.GetEquippedArmor().GetArmorName() + "\n"

            ;

        characterPanel.SetActive(true);
    }
    void CloseCharacterInfoPanel()
    {
        infoText.text = "";
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
}

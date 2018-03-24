using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

public class TutorialManager : BaseManager {

    public string[] adviceSubjects;
    public string[] adviceContents;

    private Text subjectText;
    private Text contentsText;

    private bool demoAttack;
    private bool demoAttackComplete;

    private bool[] resourcesCollected = new bool[3];
    private bool resourcePanelOpened = false;
    private bool buildingInfoOpened = false;

    private void Start()
    {
        ResetSave();

        Init();

        subjectText = GetComponentsInChildren<Text>()[0];
        contentsText = GetComponentsInChildren<Text>()[1];
        
        //Make sure all menus are running so init values can be set
        //buildingMenu.SetActive(true);
        //questMenu.SetActive(true);
        //characterMenu.SetActive(true);
        //buildingInfo.SetActive(true);

        supplyFood = 100;
        maxVillagers = 0;
        supplyStone = 100;
        supplyWood = 100;

        for (int i = 0; i < 2; i++)
        {
            //Create some villagers

            characterScroll.GetComponent<CharacterDisplay>().AddVillager(SpawnVillager());
        }

        int questCount = GetQuestManager().GetQuestCount();

        //Create new quests
        for (int i = 3; i > questCount; i--)
            GetQuestManager().AddQuest();

        //Close all menus after init
        buildingMenu.SetActive(false);
        questMenu.SetActive(false);
        characterMenu.SetActive(false);
        buildingInfo.SetActive(false);
        mainUI.SetActive(true);
        lossPanel.SetActive(false);
        pauseMenu.SetActive(false);

        attackTimer = 1;
        attackTimerSet = false;
        SetActiveAdvice(0);
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            //Debug.Log ("Base Manager Click");
            if (heldBuilding != null)
            {
                PlaceBuilding();
            }
        }

        if (demoAttack)
        {
            if (FindEnemies())
            {
                isUnderAttack = true;
                attackTimer = 1.0f;
                attackTimerSet = false;
            }
            else
            {
                isUnderAttack = false;
                if (!attackTimerSet)
                {
                    attackTimer = Random.Range(60.0f, 120.0f);
                    attackTimerSet = true;

                    if(!demoAttackComplete)
                    {
                        demoAttackComplete = true;
                        SetActiveAdvice(5);
                    }
                }
                else
                {
                    attackTimer -= Time.deltaTime;
                }

            }
            if (attackTimer <= 0.0f)
            {
                LaunchAttack();
                if (!demoAttackComplete)
                    SetActiveAdvice(4);
            }
        }


        UIUpdate();
    }
    public override void AddResources(int resourceValue, int resourceType)
    {
        switch (resourceType)
        {
            case 0:
                supplyStone += resourceValue;
                resourcesCollected[0] = true;
                break;

            case 1:
                supplyWood += resourceValue;
                resourcesCollected[1] = true;
                break;

            case 2:
                supplyFood += resourceValue;
                resourcesCollected[2] = true;
                break;
        }

        //Check resources collected//
        if (!resourcePanelOpened)
            if (resourcesCollected[0] && resourcesCollected[1] && resourcesCollected[2])
            {
                resourcePanelOpened = true;
                SetActiveAdvice(1);
            }
    }
    public override void ToggleBuildingInfo()
    {
        if (buildingInfo.activeSelf)
        {
        }
        else
        {
            if (!buildingInfoOpened)
            {
                SetActiveAdvice(2);
                buildingInfoOpened = true;
            }
        }
    }
    void SetActiveAdvice(int index)
    {
        subjectText.text = adviceSubjects[index];
        contentsText.text = adviceContents[index];
    }
    public override void SelectCharacter(BaseVillager chosenVillager)
    {
        if (settingUpQuest)
        {
            characterScroll.GetComponent<CharacterDisplay>().GetActiveQuest().AddCharacter(chosenVillager);
            ToggleQuestMenu();
            ToggleCharacterMenu();
            settingUpQuest = false;

            SetActiveAdvice(6);
        }
        else if (addingToBuilding)
        {
            buildingInfo.GetComponentInChildren<BuildingDisplay>().AddCharacter(chosenVillager, buildingButtonIndex);
            ToggleCharacterMenu();
            ToggleBuildingInfo();
            addingToBuilding = false;

            SetActiveAdvice(3);
            demoAttack = true;

            attackTimer = Random.Range(30.0f, 60.0f);
            attackTimerSet = true;
        }
        else
        {
            controller.OpenCharacterInfoPanel(chosenVillager);
            ToggleCharacterMenu();
            SetActiveAdvice(8);
        }
    }
    protected override void PlaceBuilding()
    {
        if (!placingBuilding)
        {
            //Set placingBuilding to true
            placingBuilding = true;
        }
        else
        {
            //Debug.Log ("Place Construction Down");
            //Place construction site on mouse position and add to list of construction areas.

            if (heldBuilding.GetComponent<BaseBuilding>().PlaceInWorld())
            {
				toBeBuilt.Add(heldBuilding.GetComponent<BaseBuilding>());

                if (heldBuilding.GetComponent<Building_Farm>() || heldBuilding.GetComponent<Building_LumberCamp>() || heldBuilding.GetComponent<Building_MiningCamp>())
                {
                    SetActiveAdvice(7);
				}

                if (heldBuilding.GetComponent<Building_Walls>())
                {
                    heldBuilding = Instantiate(Resources.Load("Buildings/BuildingWall") as GameObject, new Vector3(0, 0.5f, 0), Quaternion.identity);
                    heldBuilding.GetComponent<BaseBuilding>().InitBuilding(this, GetBuildingDisplay().uniquePanels[(int)heldBuilding.GetComponent<BaseBuilding>().GetBuildingType()]);
                }
                else
                {
                    placingBuilding = false;
                    heldBuilding = null;
                }
            } else {
				Debug.Log ("Construction Failed");
				CancelBuilding ();
			}

        }
    }
}

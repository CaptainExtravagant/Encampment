using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BaseVillager : Character{

	[System.Serializable]
    public struct TaskSkills
    {
        public float mining;
        public float woodcutting;
        public float blacksmithing;
        public float weaponCrafting;
        public float armorCrafting;
        public float tailoring;
        public float farming;
        public float construction;
        public float sailing;
    }

    private bool onQuest;
    private int activeQuest;

	protected Sprite characterPortrait;

    private bool isSelected;

    private TaskSkills taskSkills;

    public float wanderRadius;
    public float wanderTimer;

    private float buildTimer;

    private float timer;
    
    private bool isBuilding;

    private ResourceTile workingResource;
    private BaseBuilding workingBuilding;
    private bool isWorking;
    private float workTimer;

	protected override void Awake()
	{
		base.Awake ();

		//Debug.Log ("Villager Start");

		agent = GetComponent<NavMeshAgent>();
		CreateCharacter(new Vector3(Random.Range(-10, 10), 1, Random.Range(-10, 10)));
		CreateTaskSkills();

		Weapon_Fists fists = new GameObject ("Fists").AddComponent<Weapon_Fists> ();
		BaseArmor armor = new GameObject ("No Armor").AddComponent<BaseArmor> ();

		EquipWeaponToMainHand(fists);
		EquipWeaponToOffHand(fists);
		EquipArmor (armor);

		timer = wanderTimer;

		CalculateNextLevelExperience ();
	}

    public TaskSkills GetTaskSkills()
    {
        return taskSkills;
    }

	public Sprite GetPortrait()
	{
		return characterPortrait;
	}

	public string GetName()
	{
		return characterInfo.characterName;
	}

	public int GetLevel()
	{
		return characterInfo.characterLevel;
	}

	public void ImproveTaskSkill(TaskSkills targetSkill, float bonusValue)
	{
		//targetSkill += bonusValue;
	}

	public void ImproveCombatSkill(BaseWeapon.WEAPON_TYPE targetSkill, float bonusValue)
	{
		switch (targetSkill) {
		case BaseWeapon.WEAPON_TYPE.WEAPON_AXE:
			characterInfo.characterCombatSkills.axe = (GetCombatSkills ().axe + bonusValue);
			break;

		case BaseWeapon.WEAPON_TYPE.WEAPON_BOW:
			characterInfo.characterCombatSkills.bow = (GetCombatSkills ().bow + bonusValue);
			break;

		case BaseWeapon.WEAPON_TYPE.WEAPON_LONGSWORD:
			characterInfo.characterCombatSkills.longsword = (GetCombatSkills ().longsword + bonusValue);
			break;

		case BaseWeapon.WEAPON_TYPE.WEAPON_POLEARM:
			characterInfo.characterCombatSkills.polearm = (GetCombatSkills ().polearm + bonusValue);
			break;

		case BaseWeapon.WEAPON_TYPE.WEAPON_SHIELD:
			characterInfo.characterCombatSkills.armor = (GetCombatSkills ().armor + bonusValue);
			break;

		case BaseWeapon.WEAPON_TYPE.WEAPON_SWORD:
			characterInfo.characterCombatSkills.sword = (GetCombatSkills ().sword + bonusValue);
			break;

		default:
			break;
		}
	}

    private void CreateTaskSkills()
    {
        taskSkills.mining = baseAttributeValue * (characterInfo.characterAttributes.fitness / 100);

        taskSkills.woodcutting = baseAttributeValue * (characterInfo.characterAttributes.fitness / 100);

        taskSkills.blacksmithing = baseAttributeValue * (((characterInfo.characterAttributes.focus / 100) + (characterInfo.characterAttributes.fitness / 100)) / 2);

        taskSkills.weaponCrafting = baseAttributeValue * (characterInfo.characterAttributes.focus / 100);

        taskSkills.armorCrafting = baseAttributeValue * (characterInfo.characterAttributes.focus / 100);

        taskSkills.tailoring = baseAttributeValue * (characterInfo.characterAttributes.focus / 100);

        taskSkills.farming = baseAttributeValue * (characterInfo.characterAttributes.fitness / 100);

        taskSkills.construction = baseAttributeValue * (((characterInfo.characterAttributes.focus / 100) + (characterInfo.characterAttributes.fitness / 100)) / 2);

        taskSkills.sailing = baseAttributeValue * (((characterInfo.characterAttributes.focus / 100) + (characterInfo.characterAttributes.fitness / 100)) / 2);
    }

	private void SetTaskSkills(TaskSkills newSkills)
	{
		taskSkills.mining = newSkills.mining;
		taskSkills.woodcutting = newSkills.woodcutting;
		taskSkills.blacksmithing = newSkills.blacksmithing;
		taskSkills.weaponCrafting = newSkills.weaponCrafting;
		taskSkills.armorCrafting = newSkills.armorCrafting;
		taskSkills.tailoring = newSkills.tailoring;
		taskSkills.farming = newSkills.farming;
		taskSkills.construction = newSkills.construction;
		taskSkills.sailing = newSkills.sailing;
	}

    protected override void AIUpdate()
    {
        

        switch(currentState)
        {
            case CHARACTER_STATE.CHARACTER_ATTACKING:
                base.AIAttack();
                break;

            case CHARACTER_STATE.CHARACTER_BUILDING:
                VillagerBuild();
                break;

            case CHARACTER_STATE.CHARACTER_DEAD:
                base.AIDead();
                break;

            case CHARACTER_STATE.CHARACTER_MOVING:
                AIMoveToTarget();
                break;

            case CHARACTER_STATE.CHARACTER_WANDER:
                VillagerWander();
                break;

            case CHARACTER_STATE.CHARACTER_WORKING:
                AIFindTarget();
                break;

            case CHARACTER_STATE.CHARACTER_COLLECTING:
                VillagerCollect();
                break;

            default:
                currentState = CHARACTER_STATE.CHARACTER_WANDER;
                VillagerWander();
                break;
        }
    }
    
    void VillagerCollect()
    {
        if(targetObject == null)
        {
            isWorking = false;
            agent.enabled = true;
            currentState = CHARACTER_STATE.CHARACTER_WANDER;
        }

        if(isWorking && targetObject != null)
        {
            timer += Time.deltaTime;

            if(timer >= workTimer)
            {
                switch(workingResource.GetResourceType())
                {
                    case 0:
                        workingResource.MineResource((int)taskSkills.mining);
                        break;

                    case 1:
                        workingResource.MineResource((int)taskSkills.woodcutting);
                        break;

                    case 2:
                        workingResource.MineResource((int)taskSkills.farming);
                        break;
                }
				timer = 0;
            }
        }
        else if(!isWorking && targetObject != null)
        {
            isWorking = true;
            agent.enabled = false;
            workTimer = wanderTimer;
        }
    }

    public void SelectWorkArea(GameObject objectReference)
    {
        //Debug.Log("Work Area Selected");

        targetObject = objectReference;

        if(objectReference.GetComponent<ResourceTile>())
            workingResource = objectReference.GetComponent<ResourceTile>();

        currentState = CHARACTER_STATE.CHARACTER_MOVING;
    }

    public bool GetSelected()
    {
        return isSelected;
    }

    public void SetSelected(bool newValue)
    {
        if (newValue)
        {
            //Debug.Log("Character selected");
        }
        else
        {
            //Debug.Log("Character Deselected");
        }
        isSelected = newValue;
    }

    public CharacterInfo GetCharacterInfo()
    {
        return characterInfo;
    }

    void VillagerWander()
    {
        timer += Time.deltaTime;

        if(agent == null)
        {
            print("Nav Agent doesn't exist");
        }

        if(timer >= wanderTimer)
        {
            Vector3 targetPosition = RandomNavSphere(transform.position, wanderRadius, -1);
            agent.SetDestination(targetPosition);
            timer = 0;
        }

        AIFindTarget();
    }

	protected override void AIMoveToTarget ()
    {
        if (targetObject == null)
		{

			currentState = CHARACTER_STATE.CHARACTER_WANDER;
			//Debug.Log ("Target is null");
			AIFindTarget();

			return;
		}

		if (targetObject != null) {
			targetPosition = targetObject.transform.position;

			agent.SetDestination(targetPosition);

			currentState = CHARACTER_STATE.CHARACTER_MOVING;


			if (targetObject.GetComponent<BaseBuilding> ()) {

				if (manager.toBeUpgraded.Contains (targetObject.GetComponent<BaseBuilding> ()) && AICheckRange ()) {
					currentState = CHARACTER_STATE.CHARACTER_BUILDING;
					return;
				}

                if (targetObject.GetComponent<BaseBuilding> ().IsBuilt () == false && AICheckRange()) {
					currentState = CHARACTER_STATE.CHARACTER_BUILDING;
                    return;
				}

                if(targetObject.GetComponent<BaseBuilding>().IsBuilt() && AICheckRange())
                {
                    VillagerWork();
                }

			} else if (targetObject.GetComponent<Character> () && AICheckRange ()) {
				currentState = CHARACTER_STATE.CHARACTER_ATTACKING;
			} else if (targetObject.GetComponent<ResourceTile> () && AICheckRange ()) {
				currentState = CHARACTER_STATE.CHARACTER_COLLECTING;
			}
		}
	}

    protected override void AIFindTarget()
    {
        //print("Trying to find target");
        if(manager.GetUnderAttack() && currentState == CHARACTER_STATE.CHARACTER_WORKING)
        {
            targetObject = null;
            GetComponent<MeshRenderer>().enabled = true;
            agent.enabled = true;
        }


        if (targetObject == null)
        {
            //Is the base currently under attack?
            if (manager.GetUnderAttack())
            {
                //Find all enemies in the world and target the closest one
                
                for (int i = 0; i < manager.enemyList.Count; i++)
                {
                    if (manager.enemyList[i] != null)
                    {
                        //If this is the first item, set it to be the current target
                        if (i == 0)
                        {
                            SetTarget(manager.enemyList[i].gameObject);
                        }
                        //If this isn't the current item, see if the distance between this character and the new target is less than the distance to the current target
                        else if (Vector3.Distance(transform.position, targetPosition) > Vector3.Distance(transform.position, manager.enemyList[i].transform.position))
                        {
                            SetTarget(manager.enemyList[i].gameObject);
                        }

                    }
                }

                return;
            }

            //print("Target Object is null");

            if (!workingBuilding)
            {

                //print("Finding build targets");
                //If there aren't any construction targets, continue to wander
                if (manager.toBeBuilt.Count <= 0)
                {
                    //print("No Build Targets");
                    currentState = CHARACTER_STATE.CHARACTER_WANDER;
                }
                else //If at least one target has been found, set it as the current target and set the character state to build
                {
                    //print("Target found");
                    for (int i = 0; i < manager.toBeBuilt.Count; i++)
                    {
                        //print("Check target");
                        //If this is the first item, set it to be the current target
                        if (i == 0)
                        {
                            //Does this site have a build slot available?
                            if (manager.toBeBuilt[i].BuildSlotAvailable())
                            {
                                SetTarget(manager.toBeBuilt[i].gameObject);
                                manager.toBeBuilt[i].StartWork();
                            }
                        }//If this isn't the current item, see if the distance between this character and the new target is less than the distance to the current target
                        else if (Vector3.Distance(transform.position, targetPosition) > Vector3.Distance(transform.position, manager.toBeBuilt[i].transform.position))
                        {
                            //Does this site have a build slot available?
							if (manager.toBeBuilt [i].BuildSlotAvailable ()) {
								SetTarget (manager.toBeBuilt [i].gameObject);
								manager.toBeBuilt [i].StartWork();
							}
                        }
                    }
                }

				if (manager.toBeUpgraded.Count <= 0) {
					currentState = CHARACTER_STATE.CHARACTER_WANDER;
				} else {
					//print("Target found");
					for (int i = 0; i < manager.toBeUpgraded.Count; i++) {
						//print("Check target");
						//If this is the first item, set it to be the current target
						if (i == 0) {
							//Does this site have a build slot available?
							if (manager.toBeUpgraded [i].BuildSlotAvailable ()) {
								SetTarget (manager.toBeUpgraded [i].gameObject);
								manager.toBeUpgraded [i].StartWork ();
							}
						}//If this isn't the current item, see if the distance between this character and the new target is less than the distance to the current target
						else if (Vector3.Distance (transform.position, targetPosition) > Vector3.Distance (transform.position, manager.toBeUpgraded [i].transform.position)) {
							//Does this site have a build slot available?
							if (manager.toBeUpgraded [i].BuildSlotAvailable ()) {
								SetTarget (manager.toBeUpgraded [i].gameObject);
								manager.toBeUpgraded [i].StartWork ();
							}
						}
					}
				}

            }
            else
            {
                targetObject = workingBuilding.gameObject;
                currentState = CHARACTER_STATE.CHARACTER_MOVING;
            }

            //Set the current character state
            if (targetObject != null)
            {
                currentState = CHARACTER_STATE.CHARACTER_MOVING;
            }
            else
            {
                currentState = CHARACTER_STATE.CHARACTER_WANDER;
            }
        }
    }

    void VillagerBuild()
    {
		if (manager.toBeUpgraded.Contains (targetObject.GetComponent<BaseBuilding> ())) {
			if (isBuilding) {
				timer += Time.deltaTime;

				if (timer >= buildTimer) {
					//Add construction points to building
					targetObject.GetComponent<BaseBuilding> ().AddConstructionPoints (taskSkills.construction, this);
					timer = 0;
				}
			} else
			{
				//Set building to true, start build timer, stop navigation
				isBuilding = true;

				agent.enabled = false;

				buildTimer = wanderTimer;
			}
		}
		else{
			if (isBuilding && !targetObject.GetComponent<BaseBuilding>().IsBuilt())
			{
			    timer += Time.deltaTime;
			
			    if (timer >= buildTimer)
			    {
			        //Add construction points to building
					targetObject.GetComponent<BaseBuilding>().AddConstructionPoints(taskSkills.construction, this);
			        timer = 0;
			    }
			}
			else if (!isBuilding && !targetObject.GetComponent<BaseBuilding>().IsBuilt())
			{
			        //Set building to true, start build timer, stop navigation
			        isBuilding = true;
			
			        agent.enabled = false;
			
			        buildTimer = wanderTimer;
			} 	
			  	
			if	(targetObject.GetComponent<BaseBuilding>().IsBuilt())
			{ 	
					isBuilding = false;
					agent.enabled = true;
					currentState = CHARACTER_STATE.CHARACTER_WANDER;
			} 	
		}
    }

    void VillagerWork()
    {
        workingBuilding = targetObject.GetComponent<BaseBuilding>();
        currentState = CHARACTER_STATE.CHARACTER_WORKING;
        GetComponent<MeshRenderer>().enabled = false;
        agent.enabled = false;
    }

    public void VillagerStopWork()
    {
        workingBuilding = null;
        currentState = CHARACTER_STATE.CHARACTER_WANDER;
        GetComponent<MeshRenderer>().enabled = true;
        agent.enabled = true;
    }

    public void SetTarget(GameObject newTarget)
    {
        currentState = CHARACTER_STATE.CHARACTER_MOVING;
        targetPosition = newTarget.transform.position;
        targetObject = newTarget;
    }

	new protected void AIDead()
	{
		manager.villagerList.Remove (this);
		manager.CheckVillagerCount ();
		base.AIDead ();
	}

    public VillagerData Save()
    {
        VillagerData villagerData = new VillagerData
        {
            positionX = transform.position.x,
            positionY = transform.position.y,
            positionZ = transform.position.z,

            taskSkills = GetTaskSkills(),
            characterInfo = GetCharacterInfo(),

            currentHealth = GetCurrentHealth(),

            onQuest = onQuest,
            questNumber = activeQuest
        };

        if (GetEquippedWeapon() != null)
        	villagerData.equippedWeapon = GetEquippedWeapon().Save();
		if(GetOffHandWeapon() != null)
			villagerData.offhandWeapon = GetOffHandWeapon().Save();
		if(GetEquippedArmor() != null)
        	villagerData.equippedArmor = GetEquippedArmor().Save();
        
		return villagerData;
	}

	public void Load(VillagerData villagerData)
	{
		//Debug.Log ("Villager Load");
		transform.position = new Vector3(villagerData.positionX, villagerData.positionY, villagerData.positionZ);

		SetTaskSkills (villagerData.taskSkills);
		characterInfo = villagerData.characterInfo;
		CalculateNextLevelExperience ();

		SetCurrentHealth (villagerData.currentHealth);

		if (villagerData.equippedWeapon != null) {
			GetEquippedWeapon().Load (villagerData.equippedWeapon);
		}
		if (villagerData.offhandWeapon != null) {
			GetOffHandWeapon().Load (villagerData.offhandWeapon);
		}
		if (villagerData.equippedArmor != null) {
			GetEquippedArmor().Load(villagerData.equippedArmor);
		}

        onQuest = villagerData.onQuest;

        if(onQuest)
        {
            activeQuest = villagerData.questNumber;
            //manager.gameObject.GetComponent<QuestManager>().GetQuestList()[activeQuest].AddCharacter(this);
        }

        manager.characterScroll.GetComponent<CharacterDisplay>().Init(this.gameObject);
	}

    public void SendOnQuest(int questNumber)
    {
        onQuest = true;
        activeQuest = questNumber;
    }
}

[System.Serializable]
public class VillagerData
{
    public float positionX;
    public float positionY;
    public float positionZ;
    

	public BaseVillager.TaskSkills taskSkills;
	public Character.CharacterInfo characterInfo;

	public float currentHealth;

	public WeaponData equippedWeapon;
	public WeaponData offhandWeapon;
	public ArmorData equippedArmor;

    public bool onQuest;
    public int questNumber;
}
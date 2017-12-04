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

    private bool isSelected;

    private TaskSkills taskSkills;

    public float wanderRadius;
    public float wanderTimer;

    private float buildTimer;

    private float timer;
    
    private bool isBuilding;

    private BaseManager managerReference;

    private ResourceTile workingResource;
    private BaseBuilding workingBuilding;
    private bool isWorking;
    private float workTimer;

    public override void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        CreateTaskSkills();

		managerReference = FindObjectOfType<BaseManager>();

        Weapon_Fists fists = new Weapon_Fists();

        EquipWeaponToMainHand(fists);
        EquipWeaponToOffHand(fists);

        timer = wanderTimer;
    }

    public TaskSkills GetTaskSkills()
    {
        return taskSkills;
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
                base.AIMoveToTarget();
                break;

            case CHARACTER_STATE.CHARACTER_WANDER:
                VillagerWander();
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
        Debug.Log("Work Area Selected");

        targetObject = objectReference;

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
            Debug.Log("Character selected");
        }
        else
        {
            Debug.Log("Character Deselected");
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

    protected override void AIFindTarget()
    {
        //print("Trying to find target");

        if (targetObject == null)
        {
            //print("Target Object is null");

            //Is the base currently under attack?
            if (managerReference.GetUnderAttack())
            {
                //Find all enemies in the world and target the closest one

                for (int i = 0; i < managerReference.enemyList.Count; i++)
                {
                    //If this is the first item, set it to be the current target
                    if (i == 0)
                    {
                        SetTarget(managerReference.enemyList[i].gameObject);
                    }
                    //If this isn't the current item, see if the distance between this character and the new target is less than the distance to the current target
                    else if (Vector3.Distance(transform.position, targetPosition) > Vector3.Distance(transform.position, managerReference.enemyList[i].transform.position))
                    {
                        SetTarget(managerReference.enemyList[i].gameObject);
                    }
                }
            }
            else
            {

                //print("Finding build targets");
                //If there aren't any construction targets, continue to wander
                if (managerReference.toBeBuilt.Count <= 0)
                {
                    //print("No Build Targets");
                    currentState = CHARACTER_STATE.CHARACTER_WANDER;
                }
                else //If at least one target has been found, set it as the current target and set the character state to build
                {
                    //print("Target found");
                    for (int i = 0; i < managerReference.toBeBuilt.Count; i++)
                    {
                        //print("Check target");
                        //If this is the first item, set it to be the current target
                        if (i == 0)
                        {
                            SetTarget(managerReference.toBeBuilt[i].gameObject);
                        }//If this isn't the current item, see if the distance between this character and the new target is less than the distance to the current target
                        else if (Vector3.Distance(transform.position, targetPosition) > Vector3.Distance(transform.position, managerReference.toBeBuilt[i].transform.position))
                        {
                            //Does this site have a build slot available?
                            
                            SetTarget(managerReference.toBeBuilt[i].gameObject);
                        }
                    }
                }
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
        if(targetObject.GetComponent<BaseBuilding>().IsBuilt())
        {
            isBuilding = false;
            agent.enabled = true;
            currentState = CHARACTER_STATE.CHARACTER_WANDER;
        }

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
    }

    void VillagerWork()
    {
		I_Building building = targetObject.GetComponent<BaseBuilding>() as I_Building;

		if (building != null) {
			building.WorkBuilding (this);
		}
    }

    public void SetTarget(GameObject newTarget)
    {
        targetPosition = newTarget.transform.position;
        targetObject = newTarget;
    }

    public VillagerData Save()
    {
        VillagerData villagerData = new VillagerData();

        villagerData.positionX = transform.position.x;
        villagerData.positionY = transform.position.y;
        villagerData.positionZ = transform.position.z;

        villagerData.taskSkills = GetTaskSkills();
        villagerData.characterInfo = GetCharacterInfo();

        villagerData.currentHealth = GetCurrentHealth();

        villagerData.equippedWeapon = GetEquippedWeapon().Save();
        villagerData.offhandWeapon = GetOffHandWeapon().Save();
        villagerData.equippedArmor = GetEquippedArmor().Save();

		return villagerData;
	}

	public void Load(VillagerData villagerData)
	{
		transform.position.Set(villagerData.positionX, villagerData.positionY, villagerData.positionZ);

		SetTaskSkills (villagerData.taskSkills);
		characterInfo = villagerData.characterInfo;

		SetCurrentHealth (villagerData.currentHealth);

		equippedWeapon.Load(villagerData.equippedWeapon);
		offHandWeapon.Load(villagerData.offhandWeapon);
		equippedArmor.Load(villagerData.equippedArmor);
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

}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BaseVillager : Character{

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

    private TaskSkills taskSkills;

    public float wanderRadius;
    public float wanderTimer;

    private float buildTimer;

    private float timer;
    
    private bool isBuilding;

    private BaseManager managerReference;

    public override void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        CreateCharacter(new Vector3 (Random.value * 10, 1, Random.value * 10));
        SetTaskSkills();

        managerReference = GameObject.FindGameObjectWithTag("Manager").GetComponent<BaseManager>();

        timer = wanderTimer;
    }

    private void SetTaskSkills()
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

            default:
                currentState = CHARACTER_STATE.CHARACTER_WANDER;
                VillagerWander();
                break;
        }
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
                        targetPosition = managerReference.enemyList[i].transform.position;
                        targetObject = managerReference.enemyList[i].gameObject;
                    }
                    //If this isn't the current item, see if the distance between this character and the new target is less than the distance to the current target
                    else if (Vector3.Distance(transform.position, targetPosition) > Vector3.Distance(transform.position, managerReference.enemyList[i].transform.position))
                    {
                        targetPosition = managerReference.enemyList[i].transform.position;
                        targetObject = managerReference.enemyList[i].gameObject;
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
                            targetPosition = managerReference.toBeBuilt[i].transform.position;
                            targetObject = managerReference.toBeBuilt[i].gameObject;
                        }//If this isn't the current item, see if the distance between this character and the new target is less than the distance to the current target
                        else if (Vector3.Distance(transform.position, targetPosition) > Vector3.Distance(transform.position, managerReference.toBeBuilt[i].transform.position))
                        {
                            //Does this site have a build slot available?

                            targetPosition = managerReference.toBeBuilt[i].transform.position;
                            targetObject = managerReference.toBeBuilt[i].gameObject;
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
        if(targetObject == null)
        {
            isBuilding = false;
            agent.enabled = true;
            currentState = CHARACTER_STATE.CHARACTER_WANDER;
        }

        if (isBuilding && targetObject != null)
        {
            timer += Time.deltaTime;

            if (timer >= buildTimer)
            {
                //Add construction points to building
                targetObject.GetComponent<ConstructionArea>().AddConstructionPoints(taskSkills.construction);
                timer = 0;
            }
        }
        else if (!isBuilding && targetObject != null)
        {
                //Set building to true, start build timer, stop navigation
                isBuilding = true;

                agent.enabled = false;

                buildTimer = wanderTimer;
        }
    }



}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BaseVillager : Character{

    public float wanderRadius;
    public float wanderTimer;

    private float timer;

    private bool baseUnderAttack;

    public BaseVillager()
    {
        CreateCharacter(new Vector3(Random.value * 10, 1.0f, Random.value * 10), "Characters/VillagerActor");

        timer = wanderTimer;
    }

    private void Update()
    {
        AIUpdate();
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
        //Is the base currently under attack?
        if(baseUnderAttack)
        {
            //Find all enemies in the world and target the closest one
            BaseEnemy[] targetList = FindObjectsOfType<BaseEnemy>();

            for (int i = targetList.Length; i < 0; i--)
            {
                //If this is the first item, set it to be the current target
                if (i == targetList.Length)
                {
                    targetPosition = targetList[i].transform.position;
                    targetObject = targetList[i].gameObject;
                }
                //If this isn't the current item, see if the distance between this character and the new target is less than the distance to the current target
                else if (Vector3.Distance(transform.position, targetPosition) > Vector3.Distance(transform.position, targetList[i].transform.position))
                {
                    targetPosition = targetList[i].transform.position;
                    targetObject = targetList[i].gameObject;
                }
            }
        }
        else
        {
            //Find any construction areas available
            ConstructionArea[] targetList = FindObjectsOfType<ConstructionArea>();

            if(targetList.Length <= 0)
            {
                currentState = CHARACTER_STATE.CHARACTER_WANDER;
            }

            for (int i = targetList.Length; i < 0; i--)
            {
                //If this is the first item, set it to be the current target
                if (i == targetList.Length)
                {
                    targetPosition = targetList[i].transform.position;
                    targetObject = targetList[i].gameObject;
                }
                //If this isn't the current item, see if the distance between this character and the new target is less than the distance to the current target
                else if (Vector3.Distance(transform.position, targetPosition) > Vector3.Distance(transform.position, targetList[i].transform.position))
                {
                    //Does this site have a build slot available?

                    targetPosition = targetList[i].transform.position;
                    targetObject = targetList[i].gameObject;
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

    void VillagerBuild()
    {

    }



}

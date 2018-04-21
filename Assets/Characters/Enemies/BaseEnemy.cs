using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BaseEnemy : Character {

    private BaseManager managerReference;

	new public void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        CreateCharacter(new Vector3(Random.value * 10, 1, Random.value * 10));

        managerReference = GameObject.FindGameObjectWithTag("Manager").GetComponent<BaseManager>();
    }

    protected override void AIUpdate()
    {
        switch (currentState)
        {
            case CHARACTER_STATE.CHARACTER_MOVING:
                AIMoveToTarget();
                break;

            case CHARACTER_STATE.CHARACTER_DEAD:
                base.AIDead();
                break;

            case CHARACTER_STATE.CHARACTER_ATTACKING:
                base.AIAttack();
                break;

            default:
                AIFindTarget();
                break;
        }
    }

	protected override void AIMoveToTarget ()
	{
		if(targetObject == null)
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


			if (targetObject.GetComponent<BaseBuilding> () && AICheckRange ()) {
				currentState = CHARACTER_STATE.CHARACTER_ATTACKING;
			} else if (targetObject.GetComponent<Character> () && AICheckRange ()) {
				currentState = CHARACTER_STATE.CHARACTER_ATTACKING;
			}
		}
	}

    protected override void AIFindTarget()
    {
        if(targetObject == null)
        {
			for (int i = 0; i < managerReference.GetVillagerCount(); i++) {
				if (managerReference.GetVillagerList() [i] != null) {
                    if (!managerReference.GetVillagerList()[i].IsOnQuest())
                    {
                        if (i == 0)
                        {
                            targetPosition = managerReference.GetVillagerList()[i].transform.position;
                            targetObject = managerReference.GetVillagerList()[i].gameObject;
                        }
                        else if (Vector3.Distance(transform.position, targetPosition) > Vector3.Distance(transform.position, managerReference.GetVillagerList()[i].transform.position))
                        {
                            targetPosition = managerReference.GetVillagerList()[i].transform.position;
                            targetObject = managerReference.GetVillagerList()[i].gameObject;
                        }
                    }
				}
			}
			for (int i = 0; i < managerReference.GetBuildingCount(); i++) {
				if (managerReference.GetBuildingList() [i] != null) {
					if (Vector3.Distance (transform.position, targetPosition) > Vector3.Distance (transform.position, managerReference.GetBuildingList() [i].transform.position)) {
						targetPosition = managerReference.GetBuildingList() [i].transform.position;
						targetObject = managerReference.GetBuildingList() [i].gameObject;
					}
				}
			}
			for(int i = 0; i < managerReference.GetToBeBuiltCount(); i++)
			{
				if (managerReference.GetToBeBuiltList() [i] != null) {
					if (Vector3.Distance (transform.position, targetPosition) > Vector3.Distance (transform.position, managerReference.GetToBeBuiltList() [i].transform.position)) {
						targetPosition = managerReference.GetToBeBuiltList() [i].transform.position;
						targetObject = managerReference.GetToBeBuiltList() [i].gameObject;
					}
				}
            }

            if(targetObject != null)
            {
                currentState = CHARACTER_STATE.CHARACTER_MOVING;
            }
            else
            {
                print("Target not set; enemy");
            }
        }
    }
}

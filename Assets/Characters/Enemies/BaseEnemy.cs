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
			Debug.Log ("Target is null");
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
			for (int i = 0; i < managerReference.villagerList.Count; i++) {
				if (managerReference.villagerList [i] != null) {
					if (i == 0) {
						targetPosition = managerReference.villagerList [i].transform.position;
						targetObject = managerReference.villagerList [i].gameObject;
					} else if (Vector3.Distance (transform.position, targetPosition) > Vector3.Distance (transform.position, managerReference.villagerList [i].transform.position)) {
						targetPosition = managerReference.villagerList [i].transform.position;
						targetObject = managerReference.villagerList [i].gameObject;
					}
				}
			}
			for (int i = 0; i < managerReference.buildingList.Count; i++) {
				if (managerReference.buildingList [i] != null) {
					if (Vector3.Distance (transform.position, targetPosition) > Vector3.Distance (transform.position, managerReference.buildingList [i].transform.position)) {
						targetPosition = managerReference.buildingList [i].transform.position;
						targetObject = managerReference.buildingList [i].gameObject;
					}
				}
			}
			for(int i = 0; i < managerReference.toBeBuilt.Count; i++)
			{
				if (managerReference.toBeBuilt [i] != null) {
					if (Vector3.Distance (transform.position, targetPosition) > Vector3.Distance (transform.position, managerReference.toBeBuilt [i].transform.position)) {
						targetPosition = managerReference.toBeBuilt [i].transform.position;
						targetObject = managerReference.toBeBuilt [i].gameObject;
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

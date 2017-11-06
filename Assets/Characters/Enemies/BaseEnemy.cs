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
                base.AIMoveToTarget();
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

    protected override void AIFindTarget()
    {
        if(targetObject == null)
        {
            for (int i = 0; i < managerReference.villagerList.Count; i++)
            {
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

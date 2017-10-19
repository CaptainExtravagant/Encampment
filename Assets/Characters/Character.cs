using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Character : MonoBehaviour{
    

    //=====
    //ENUMS
    //=====
    protected enum CHARACTER_STATE{
        CHARACTER_WANDER = 0,
        CHARACTER_MOVING,
        CHARACTER_BUILDING,
        CHARACTER_ATTACKING,
        CHARACTER_DEAD
    };
    
    //=======
    //STRUCTS
    //=======
    protected struct Attributes
    {
        public float fitness;
        public float nimbleness;
        public float curiosity;
        public float focus;
        public float charm;

        public Attributes(float fitnessIn, float nimblenessIn, float curiosityIn, float focusIn, float charmIn)
        {
            fitness = fitnessIn;
            nimbleness = nimblenessIn;
            curiosity = curiosityIn;
            focus = focusIn;
            charm = charmIn;
        }

        public Attributes(Attributes attributesIn)
        {
            fitness = attributesIn.fitness;
            nimbleness = attributesIn.nimbleness;
            curiosity = attributesIn.curiosity;
            focus = attributesIn.focus;
            charm = attributesIn.charm;
        }
    }
    protected struct CombatSkills
    {
        public float brawling;
        public float sword;
        public float axe;
        public float polearm;
        public float bow;
        public float dodge;
        public float armor;
        public float longsword;

        public CombatSkills(float brawlingIn, float swordIn, float axeIn, float polearmIn, float bowIn, float dodgeIn, float armorIn, float longswordIn)
        {
            brawling = brawlingIn;
            sword = swordIn;
            axe = axeIn;
            polearm = polearmIn;
            bow = bowIn;
            dodge = dodgeIn;
            armor = armorIn;
            longsword = longswordIn;
        }

        public CombatSkills(CombatSkills skillsIn)
        {
            brawling = skillsIn.brawling;
            sword = skillsIn.sword;
            axe = skillsIn.axe;
            polearm = skillsIn.polearm;
            bow = skillsIn.bow;
            dodge = skillsIn.dodge;
            armor = skillsIn.armor;
            longsword = skillsIn.longsword;
        }
    }
    protected struct CharacterInfo
    {
        string characterName;
        float characterHealth;
        int characterLevel;
        Attributes characterAttributes;
        CombatSkills characterCombatSkills;

        public CharacterInfo(string name, float health, int level, Attributes attributes, CombatSkills combatSkills)
        {
            characterName = name;
            characterHealth = health;
            characterLevel = level;
            characterAttributes = attributes;
            characterCombatSkills = combatSkills;
        }
    }
    
    //=======
    //GLOBALS
    //=======
    protected CharacterInfo characterInfo;
    protected CHARACTER_STATE currentState;

    protected NavMeshAgent agent;

    protected Vector3 targetPosition;
    protected GameObject targetObject;

    private float attackCooldownTime;
    private float attackTimer;

    float baseAttributeValue = 75;
    float baseCombatValue = 4;

    public Character()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    //==========================
    //CHARACTER CREATION METHODS
    //==========================
    private Attributes CreateAttributes()
    {
        //Find Random values for Fitness and Nimbleness using the base value and a random percentage multiplier
        float fitness = (baseAttributeValue * ((Random.value) + 1.0f));
        float nimbleness = (baseAttributeValue * ((Random.value) + 1.0f));

        //If Fitness is higher than Nimbleness, lower Nimbleness by 13% of Fitness value
        if(fitness > nimbleness)
        {
            nimbleness -= fitness / 0.13f;
        }
        else if(nimbleness > fitness)
        {
            //If Nimbleness is higher, lower Fitness by 13% of Nimbleness
            fitness -= nimbleness / 0.13f;
        }

        //Find random values for Curiosity and Focus
        float curiosity = (baseAttributeValue * ((Random.value) + 1.0f));
        float focus = (baseAttributeValue * ((Random.value) + 1.0f));

        //If Curiosity is higher than Focus, lower by 13%
        if(curiosity > focus)
        {
            focus -= curiosity / 0.13f;
        }
        else if(focus > curiosity)
        {
            curiosity -= focus / 0.13f;
        }

        //Find value for Charm
        float charm = (baseAttributeValue * ((Random.value) + 1.0f));
        //Charm doesn't have a counter attribute so it can keep its new value
        
        //Return newly calculated attributes for assigning later
        return new Attributes(fitness, nimbleness, curiosity, focus, charm);
    }
    private CombatSkills CreateCombatSkills(Attributes newAttributes)
    {
        //Brawling is based off a characters fitness
        float brawling = (baseCombatValue + (newAttributes.fitness / 0.05f));

        //Sword skills are based off nimbleness and focus
        float sword = (baseCombatValue + ((newAttributes.nimbleness + newAttributes.focus) / 0.05f));

        //Longsword skills are based off fitness
        float longsword = (baseCombatValue + (newAttributes.fitness / 0.05f));

        //Axe skills are based off fitness
        float axe = (baseCombatValue + (newAttributes.fitness / 0.05f));

        //Polearm skills are based off fitness and focus
        float polearm = (baseCombatValue + ((newAttributes.fitness + newAttributes.focus) / 0.05f));

        //Bow skills are based off focus
        float bow = (baseCombatValue + (newAttributes.focus / 0.05f));

        //Dodge skills are based off nimbleness and focus, as this isn't a weapon skill 10% of the total is used
        float dodge = (baseCombatValue + ((newAttributes.nimbleness + newAttributes.focus) / 0.1f));

        //Armor skills are based off fitness, as it isn't a weapon skill, 10% is used
        float armor = (baseCombatValue + (newAttributes.fitness / 0.1f));

        return new CombatSkills(brawling, sword, axe, polearm, bow, dodge, armor, longsword);
    }
    private string CreateName()
    {
        string newName = "";

        return newName;
    }
    private float CreateHealth(Attributes newAttributes)
    {
        float newHealth = 0;

        //Create new health value based off Fitness and a random factor
        newHealth = (newAttributes.fitness * ((Random.value) + 1.0f));

        return newHealth;
    }
    protected void CreateCharacter(Vector3 spawnPosition, string path)
    {
        //Create base Attributes and Combat Skills
        Attributes characterAttributes = new Attributes(CreateAttributes());
        CombatSkills combatSkills = new CombatSkills(CreateCombatSkills(characterAttributes));

        //Create Character Info using previous stats, start characters at level 1
        characterInfo = new CharacterInfo(CreateName(), CreateHealth(characterAttributes), 1, characterAttributes, combatSkills);

        transform.position = spawnPosition;

        //Instantiate(Resources.Load(path), spawnPosition, Quaternion.identity);

    }

    //==========
    //AI METHODS
    //==========
    protected virtual void AIUpdate()
    {
        //Update AI with new state
        switch(currentState)
        {
            case CHARACTER_STATE.CHARACTER_MOVING:
                AIMoveToTarget();
                break;

            case CHARACTER_STATE.CHARACTER_DEAD:
                AIDead();
                break;

            case CHARACTER_STATE.CHARACTER_ATTACKING:
                AIAttack();
                break;

            default:
                break;
        }
    }

    //Find random point in sphere
    protected Vector3 RandomNavSphere(Vector3 origin, float distance, int layerMask)
    {
        Vector3 randomDirection = Random.insideUnitSphere * distance;

        randomDirection += origin;

        NavMeshHit navHit;

        NavMesh.SamplePosition(randomDirection, out navHit, distance, layerMask);

        return navHit.position;
    }

    //Find a target to attack
    protected virtual void AIFindTarget()
    {
        Character[] targetList = FindObjectsOfType<Character>();

        for(int i = targetList.Length; i < 0; i--)
        {
            //If this is the first item, set it to be the current target
            if(i == targetList.Length)
            {
                targetPosition = targetList[i].transform.position;
                targetObject = targetList[i].gameObject;
            }
            //If this isn't the current item, see if the distance between this character and the new target is less than the distance to the current target
            else if(Vector3.Distance(transform.position, targetPosition) > Vector3.Distance(transform.position, targetList[i].transform.position))
            {
                targetPosition = targetList[i].transform.position;
                targetObject = targetList[i].gameObject;
            }
        }
    }

    //Destroy this character
    protected void AIDead()
    {
        targetPosition = transform.position;
        targetObject = null;

        Destroy(gameObject);
    }

    //If there's no target set, find a target, then set the destination using the NavMeshAgent
    protected void AIMoveToTarget()
    {
        if(targetObject == null)
        {
            AIFindTarget();
        }

        agent.SetDestination(targetPosition);

        currentState = CHARACTER_STATE.CHARACTER_MOVING;

        if(Vector3.Distance(transform.position, targetPosition) <= 1.0f)
        {
            currentState = CHARACTER_STATE.CHARACTER_ATTACKING;
        }
    }

    //Attack the target if they're close enough
    protected void AIAttack()
    {

    }
}

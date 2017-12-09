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
        CHARACTER_COLLECTING,
        CHARACTER_ATTACKING,
        CHARACTER_DEAD
    };
    
    //=======
    //STRUCTS
    //=======
	[System.Serializable]
    public struct Attributes
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

	[System.Serializable]
    public struct CombatSkills
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

	[System.Serializable]
    public struct CharacterInfo
    {
        public string characterName;
        public float characterHealth;
        public int characterSex;
        public int characterLevel;
        public Attributes characterAttributes;
        public CombatSkills characterCombatSkills;

        public CharacterInfo(int sex, string name, float health,  int level, Attributes attributes, CombatSkills combatSkills)
        {
            characterName = name;
            characterHealth = health;
            characterSex = sex;
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
	public BaseWeapon equippedWeapon;
    public BaseWeapon offHandWeapon;
    protected bool offHandEnabled = true;
	public BaseArmor equippedArmor;
    
    private float currentHealth;

    protected bool isAttacking;

    protected NavMeshAgent agent;
	protected InventoryBase inventory;
	protected BaseManager manager;

    protected Vector3 targetPosition;
    protected GameObject targetObject;

    private float attackCooldownTime;
    private float attackTimer;

    protected float baseAttributeValue = 75;
    float baseCombatValue = 4;

    private string[] MaleFirstNames = {"Noah", "Liam", "William", "Mason", "James", "Benjamin", "Jacob", "Michael", "Elijah", "Ethan" };
    private string[] CharacterLastNames = {"Smith", "Johnson", "Williams", "Brown", "Jones", "Miller", "Davis", "Garcia", "Rodriguez", "Wilson" };
    private string[] FemaleFirstNames = {"Emma", "Olivia", "Ava", "Sophia", "Isabella", "Mia", "Charlotte", "Abigail", "Emily", "Kelley" };

	protected virtual void Awake()
	{
		manager = FindObjectOfType<BaseManager>();
		inventory = manager.GetInventory ();
	}

    public virtual void Start()
    {
        agent = GetComponent<NavMeshAgent>();
		//CreateCharacter(new Vector3(Random.Range(-10, 10), 1, Random.Range(-10, 10)));


    }

    public virtual void Update ()
    {
        AIUpdate();
    }

    public CombatSkills GetCombatSkills()
    {
        return characterInfo.characterCombatSkills;
    }

    //==========================
    //CHARACTER CREATION METHODS
    //==========================
    private Attributes CreateAttributes()
    {
        //Find Random values for Fitness and Nimbleness using the base value and a random percentage multiplier
        float fitness = (baseAttributeValue * ((UnityEngine.Random.value) + 1.0f));
		float nimbleness = (baseAttributeValue * ((UnityEngine.Random.value) + 1.0f));

        //If Fitness is higher than Nimbleness, lower Nimbleness by 13% of Fitness value
        if(fitness > nimbleness)
        {
            nimbleness -= fitness * 0.13f;
        }
        else if(nimbleness > fitness)
        {
            //If Nimbleness is higher, lower Fitness by 13% of Nimbleness
            fitness -= nimbleness * 0.13f;
        }

        //Find random values for Curiosity and Focus
		float curiosity = (baseAttributeValue * ((UnityEngine.Random.value) + 1.0f));
		float focus = (baseAttributeValue * ((UnityEngine.Random.value) + 1.0f));

        //If Curiosity is higher than Focus, lower by 13%
        if(curiosity > focus)
        {
            focus -= curiosity * 0.13f;
        }
        else if(focus > curiosity)
        {
            curiosity -= focus * 0.13f;
        }

        //Find value for Charm
		float charm = (baseAttributeValue * ((UnityEngine.Random.value) + 1.0f));
        //Charm doesn't have a counter attribute so it can keep its new value
        
        //Return newly calculated attributes for assigning later
        return new Attributes(fitness, nimbleness, curiosity, focus, charm);
    }
    private CombatSkills CreateCombatSkills(Attributes newAttributes)
    {
        //Brawling is based off a characters fitness
        float brawling = (baseCombatValue + (newAttributes.fitness * 0.05f));

        //Sword skills are based off nimbleness and focus
        float sword = (baseCombatValue + ((newAttributes.nimbleness + newAttributes.focus) * 0.05f));

        //Longsword skills are based off fitness
        float longsword = (baseCombatValue + (newAttributes.fitness * 0.05f));

        //Axe skills are based off fitness
        float axe = (baseCombatValue + (newAttributes.fitness * 0.05f));

        //Polearm skills are based off fitness and focus
        float polearm = (baseCombatValue + ((newAttributes.fitness + newAttributes.focus) * 0.05f));

        //Bow skills are based off focus
        float bow = (baseCombatValue + (newAttributes.focus * 0.05f));

        //Dodge skills are based off nimbleness and focus, as this isn't a weapon skill 10% of the total is used
        float dodge = (baseCombatValue + ((newAttributes.nimbleness + newAttributes.focus) * 0.1f));

        //Armor skills are based off fitness, as it isn't a weapon skill, 10% is used
        float armor = (baseCombatValue + (newAttributes.fitness * 0.1f));

        return new CombatSkills(brawling, sword, axe, polearm, bow, dodge, armor, longsword);
    }
    private string CreateName(int sex)
    {
        string newName = "";
        if(sex == 1)
        {
			newName = MaleFirstNames[UnityEngine.Random.Range(0, MaleFirstNames.Length)] + " " + CharacterLastNames[UnityEngine.Random.Range(0, CharacterLastNames.Length)];
        }
        else if(sex == 2)
        {
			newName = FemaleFirstNames[UnityEngine.Random.Range(0, FemaleFirstNames.Length)] + " " + CharacterLastNames[UnityEngine.Random.Range(0, CharacterLastNames.Length)];
        }

        Debug.Log(newName);

        return newName;
    }
    private float CreateHealth(Attributes newAttributes)
    {
        float newHealth = 0;

        //Create new health value based off Fitness and a random factor
		newHealth = (newAttributes.fitness * ((UnityEngine.Random.value) + 1.0f));

        return newHealth;
    }
    protected void CreateCharacter(Vector3 spawnPosition)
    {
		int tempSex = UnityEngine.Random.Range(1, 3);

        Debug.Log(tempSex);

        //Create base Attributes and Combat Skills
        Attributes characterAttributes = new Attributes(CreateAttributes());
        CombatSkills combatSkills = new CombatSkills(CreateCombatSkills(characterAttributes));

        //Create Character Info using previous stats, start characters at level 1
        characterInfo = new CharacterInfo(tempSex, CreateName(tempSex), CreateHealth(characterAttributes), 1, characterAttributes, combatSkills);

        //Set attack rate
        attackCooldownTime = 5 - (characterInfo.characterAttributes.nimbleness / 10);
        if(attackCooldownTime <= 0)
        {
            attackCooldownTime = 1.0f;
        }

        currentHealth = characterInfo.characterHealth;

        //Set spawn position
        transform.position = spawnPosition;
        

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
		Vector3 randomDirection = UnityEngine.Random.insideUnitSphere * distance;

        randomDirection += origin;

        NavMeshHit navHit;

        NavMesh.SamplePosition(randomDirection, out navHit, distance, layerMask);

        return navHit.position;
    }

    //Find a target to attack
    protected virtual void AIFindTarget()
    {
        Character[] targetList = FindObjectsOfType<Character>();

        for(int i = 0; i < targetList.Length; i++)
        {
            //If this is the first item, set it to be the current target
            if(i == 0)
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
	protected virtual void AIMoveToTarget()
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
				if (targetObject.GetComponent<BaseBuilding> ().IsBuilt () == false) {
					currentState = CHARACTER_STATE.CHARACTER_BUILDING;
				} else {
					targetObject = null;
					currentState = CHARACTER_STATE.CHARACTER_WANDER;
				}
			} else if (targetObject.GetComponent<Character> () && AICheckRange ()) {
				currentState = CHARACTER_STATE.CHARACTER_ATTACKING;
			} else if (targetObject.GetComponent<ResourceTile> () && AICheckRange ()) {
				currentState = CHARACTER_STATE.CHARACTER_COLLECTING;
			}
		}
    }

    protected bool AICheckRange()
    {
        //Check distance to the target object, if "in range" return true
        if (Vector3.Distance(transform.position, targetPosition) <= 2.0f)
            return true;
        else
            return false;
    }

    //Attack the target if they're close enough
    protected void AIAttack()
    {
        if(targetObject == null)
        {
            isAttacking = false;
            agent.enabled = true;
            currentState = CHARACTER_STATE.CHARACTER_WANDER;
        }

        if(isAttacking && targetObject != null)
        {
            attackTimer += Time.deltaTime;

			if (attackTimer >= attackCooldownTime) {
				
				//Attack the target
					
				if (targetObject.GetComponent<Character> ()) {
						DealDamage (targetObject.GetComponent<Character> ());
					} else if (targetObject.GetComponent<BaseBuilding> ()) {
					DamageBuilding (targetObject.GetComponent<BaseBuilding> ());
					}
					attackTimer = 0;
			}
        }
        else if(!isAttacking && targetObject != null)
        {
            //Set attacking to true, start attack timer, stop navigation
            isAttacking = true;
            agent.enabled = false;
        }
    }

	protected void DamageBuilding(BaseBuilding targetBuilding)
	{
		if (equippedWeapon != null) {
			targetBuilding.UpdateCurrentHealth (equippedWeapon.GetDamageValue ());
		} else {
			targetBuilding.UpdateCurrentHealth (GetCombatSkills ().brawling);
		}
	}

    protected void DealDamage(Character targetCharacter)
    {
        if (equippedWeapon != null)
        {
            //Deal damage to target character
            targetCharacter.TakeDamage(equippedWeapon.GetDamageValue(), this);
        }
        else
        {
            //If the character doesn't have an active weapon, use their brawling skill instead
            targetCharacter.TakeDamage(GetCombatSkills().brawling, this);
        }
    }

    protected void TakeDamage(float inDamage, Character attackingCharacter)
    {
        bool dodged = false;
        float totalDefense = 0;
        float totalDamage;

        //Remove health based on damage taken and armor value
        if (equippedArmor != null)
        {
            totalDefense += equippedArmor.GetDefenseValue();
            totalDamage = inDamage - equippedArmor.GetDefenseValue();
        }
        if(offHandWeapon != null && offHandWeapon.GetWeaponType() == BaseWeapon.WEAPON_TYPE.WEAPON_SHIELD)
        {
            totalDefense += offHandWeapon.GetDefenseValue();
        }

		if (equippedWeapon != null) {
			totalDefense += equippedWeapon.GetDefenseValue ();
		}

        totalDamage = inDamage - totalDefense;

        if(totalDamage < 0)
        {
            totalDamage = 0;
        }
        
        //If dodge value is higher than damage being dealt and armor isn't heavy, avoid damage all together
        if (equippedArmor != null)
        {
            if (GetCombatSkills().dodge > totalDamage && !equippedArmor.IsHeavyArmor())
            {
                dodged = true;
            }
        }

        if (!dodged)
        {
            print("Total damage: " + totalDamage);
            currentHealth -= totalDamage;
        }

        CheckHealth();

        //The dude who just hit you is probably more important if he isn't your current target, switch (CHANGE TO AGGRO SYSTEM LATER)
        if(attackingCharacter != targetObject)
        {
            targetObject = attackingCharacter.gameObject;
        }
    }

    protected void CheckHealth()
    {

        if(currentHealth <= 0)
        {
            currentState = CHARACTER_STATE.CHARACTER_DEAD;
        }
    }
    
    public float GetCurrentHealth()
    {
        return currentHealth;
    }

	protected void SetCurrentHealth(float newHealth)
	{
		currentHealth = newHealth;
	}

    //==============
    //EQUIPPED ITEMS
    //==============

	public void EquipWeaponToMainHand(BaseWeapon weaponToEquip)
    {
		if(inventory.itemList.Contains(weaponToEquip))
			inventory.RemoveItem (weaponToEquip);

		equippedWeapon = weaponToEquip;

        if(equippedWeapon.IsTwoHanded())
        {
            offHandEnabled = false;
        }
    }

	public void UnequipMainHand()
	{
		if(GetEquippedWeapon() != null)
			inventory.AddItem (GetEquippedWeapon ());

		BaseWeapon emptyHand = new BaseWeapon ();
		emptyHand.SetItemType (BaseItem.ITEM_TYPE.ITEM_EMPTY);

		equippedWeapon = emptyHand;
	}

	public void UnequipOffHand()
	{
		if(GetOffHandWeapon() != null)
			inventory.AddItem (GetOffHandWeapon ());

		BaseWeapon emptyHand = new BaseWeapon ();
		emptyHand.SetItemType (BaseItem.ITEM_TYPE.ITEM_EMPTY);

		offHandWeapon = emptyHand;
	}

	public void EquipWeaponToOffHand(BaseWeapon weaponToEquip)
    {
        if (offHandEnabled)
        {
			if(inventory.itemList.Contains(weaponToEquip))
				inventory.RemoveItem (weaponToEquip);
            offHandWeapon = weaponToEquip;
        }
    }

	public void EquipArmor(BaseArmor armorToEquip)
    {
		if(inventory.itemList.Contains(armorToEquip))
			inventory.RemoveItem (armorToEquip);
		equippedArmor = armorToEquip;
    }

	public void UnequipArmor()
	{
		if(GetEquippedArmor() != null)
			inventory.AddItem (GetEquippedArmor ());

		BaseArmor emptyArmor = new BaseArmor ();
		emptyArmor.SetItemType (BaseItem.ITEM_TYPE.ITEM_EMPTY);

		equippedArmor = emptyArmor;
	}

    public BaseWeapon GetEquippedWeapon()
    {
		if (equippedWeapon == null) {
			Debug.Log ("No weapon equipped");
		}
        return equippedWeapon;
    }

    public BaseWeapon GetOffHandWeapon()
    {
        if(offHandWeapon == null)
        {
            Debug.Log("No offhand weapon equipped");
        }
        return offHandWeapon;
    }

    public BaseArmor GetEquippedArmor()
    {
        return equippedArmor;
    }

}

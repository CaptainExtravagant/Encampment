﻿using System.Collections;
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
        CHARACTER_WORKING,
        CHARACTER_COLLECTING,
        CHARACTER_ATTACKING,
        CHARACTER_DEAD
    };

    protected enum CHARACTER_TRAIT
    {
        TRAIT_SMART = 0,
        TRAIT_DUMB,
        TRAIT_BRAWLER,
        TRAIT_HARDWORKER,
        TRAIT_LAZY,
        TRAIT_FAST,
        TRAIT_CRAZED
    }
    
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
        public int characterExperience;
        public Attributes characterAttributes;
        public CombatSkills characterCombatSkills;
        public List<int> characterTraits;

        public CharacterInfo(int sex, string name, float health,  int level, int experience, Attributes attributes, CombatSkills combatSkills, List<int> traits)
        {
            characterName = name;
            characterHealth = health;
            characterSex = sex;
            characterLevel = level;
            characterExperience = experience;
            characterAttributes = attributes;
            characterCombatSkills = combatSkills;
            characterTraits = traits;
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

	private float attackRange;

    protected int experienceNextLevel = 1;
    protected float experienceModifier = 1;
    
    private float currentHealth;

    protected bool isAttacking;

    protected NavMeshAgent agent;
	protected InventoryBase inventory;
	protected BaseManager manager;
    protected Rigidbody rb;
    protected Animator animator;

    protected Vector3 targetPosition;
    protected GameObject targetObject;

    private float attackCooldownTime;
    private float attackTimer;

    protected float baseAttributeValue = 50;
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
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();

        CalculateNextLevelExperience();
    }

    public virtual void Update ()
    {
        AIUpdate();
    }

    public CombatSkills GetCombatSkills()
    {
        return characterInfo.characterCombatSkills;
    }

	public int GetExperience()
    {
        return characterInfo.characterExperience;
    }

    public void AddExperience(int experienceValue)
    {
        characterInfo.characterExperience += (int)(experienceValue * experienceModifier);
        CheckExperience();
    }

    protected void CheckExperience()
    {
        if(characterInfo.characterExperience >= experienceNextLevel)
        {
            LevelUp();
        }
    }

    protected void LevelUp()
    {

        characterInfo.characterCombatSkills.armor += ((characterInfo.characterCombatSkills.armor / 100) * ((characterInfo.characterAttributes.focus / 100) * 20));
        characterInfo.characterCombatSkills.axe += ((characterInfo.characterCombatSkills.axe / 100) * ((characterInfo.characterAttributes.focus / 100) * 20));
        characterInfo.characterCombatSkills.bow += ((characterInfo.characterCombatSkills.bow / 100) * ((characterInfo.characterAttributes.focus / 100) * 20));
        characterInfo.characterCombatSkills.brawling += ((characterInfo.characterCombatSkills.brawling / 100) * ((characterInfo.characterAttributes.focus / 100) * 20));
        characterInfo.characterCombatSkills.dodge += ((characterInfo.characterCombatSkills.dodge / 100) * ((characterInfo.characterAttributes.focus / 100) * 20));
        characterInfo.characterCombatSkills.longsword += ((characterInfo.characterCombatSkills.longsword / 100) * ((characterInfo.characterAttributes.focus / 100) * 20));
        characterInfo.characterCombatSkills.polearm += ((characterInfo.characterCombatSkills.polearm / 100) * ((characterInfo.characterAttributes.focus / 100) * 20));
        characterInfo.characterCombatSkills.sword += ((characterInfo.characterCombatSkills.sword / 100) * ((characterInfo.characterAttributes.focus / 100) * 20));

        characterInfo.characterLevel++;
		CalculateNextLevelExperience();

		manager.GetCharacterDisplay ().UpdateButton (manager.GetVillagerIndex(this as BaseVillager));
    }

	public int GetNextLevelExperience()
    {
        return experienceNextLevel;
    }

    protected void CalculateNextLevelExperience()
    {
        experienceNextLevel = (int)Mathf.Ceil(60 * Mathf.Pow(characterInfo.characterLevel, 2.8f) - 60);
		if (experienceNextLevel <= 0)
			experienceNextLevel = 1;
    }

    //==========================
    //CHARACTER CREATION METHODS
    //==========================
    private Attributes CreateAttributes()
    {
        //Find Random values for Fitness and Nimbleness using the base value and a random percentage multiplier
        float fitness = (baseAttributeValue * ((UnityEngine.Random.value) + 1.0f)) / 10;
		float nimbleness = (baseAttributeValue * ((UnityEngine.Random.value) + 1.0f)) / 10;

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
		float curiosity = (baseAttributeValue * ((UnityEngine.Random.value) + 1.0f)) / 10;
		float focus = (baseAttributeValue * ((UnityEngine.Random.value) + 1.0f)) / 10;

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
		float charm = (baseAttributeValue * ((UnityEngine.Random.value) + 1.0f)) / 10;
        //Charm doesn't have a counter attribute so it can keep its new value
        
        fitness = Mathf.Round(fitness);
        fitness = Mathf.Clamp(fitness, 0, 10);

        nimbleness = Mathf.Round(nimbleness);
        nimbleness = Mathf.Clamp(nimbleness, 0, 10);

        curiosity = Mathf.Round(curiosity);
        curiosity = Mathf.Clamp(curiosity, 0, 10);

        focus = Mathf.Round(focus);
        focus = Mathf.Clamp(focus, 0, 10);

        charm = Mathf.Round(charm);
        charm = Mathf.Clamp(charm, 0, 10);

        //Return newly calculated attributes for assigning later
        return new Attributes(fitness, nimbleness, curiosity, focus, charm);
    }
    private CombatSkills CreateCombatSkills(Attributes newAttributes)
    {
        //Brawling is based off a characters fitness
        float brawling = (baseCombatValue + (newAttributes.fitness * 0.05f));
        brawling = Mathf.Round(brawling);

        //Sword skills are based off nimbleness and focus
        float sword = (baseCombatValue + ((newAttributes.nimbleness + newAttributes.focus) * 0.05f));
        sword = Mathf.Round(sword);

        //Longsword skills are based off fitness
        float longsword = (baseCombatValue + (newAttributes.fitness * 0.05f));
        longsword = Mathf.Round(longsword);

        //Axe skills are based off fitness
        float axe = (baseCombatValue + (newAttributes.fitness * 0.05f));
        axe = Mathf.Round(axe);

        //Polearm skills are based off fitness and focus
        float polearm = (baseCombatValue + ((newAttributes.fitness + newAttributes.focus) * 0.05f));
        polearm = Mathf.Round(polearm);

        //Bow skills are based off focus
        float bow = (baseCombatValue + (newAttributes.focus * 0.05f));
        bow = Mathf.Round(bow);

        //Dodge skills are based off nimbleness and focus, as this isn't a weapon skill 10% of the total is used
        float dodge = (baseCombatValue + ((newAttributes.nimbleness + newAttributes.focus) * 0.1f));
        dodge = Mathf.Round(dodge);

        //Armor skills are based off fitness, as it isn't a weapon skill, 10% is used
        float armor = (baseCombatValue + (newAttributes.fitness * 0.1f));
        armor = Mathf.Round(armor);

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

        //Debug.Log(newName);

        return newName;
    }
    private float CreateHealth(Attributes newAttributes)
    {
        float newHealth = 0;

        //Create new health value based off Fitness and a random factor
		newHealth = (newAttributes.fitness * 5) * ((UnityEngine.Random.value) + 1.0f);
        newHealth = Mathf.Round(newHealth);

        return newHealth;
    }
    protected void CreateCharacter(Vector3 spawnPosition)
    {
		int tempSex = UnityEngine.Random.Range(1, 3);

        //Debug.Log(tempSex);

        //Create base Attributes and Combat Skills
        Attributes characterAttributes = new Attributes(CreateAttributes());
        CombatSkills combatSkills = new CombatSkills(CreateCombatSkills(characterAttributes));

        //Create Character Info using previous stats, start characters at level 1
        characterInfo = new CharacterInfo(tempSex, CreateName(tempSex), CreateHealth(characterAttributes), 1, 1, characterAttributes, combatSkills, CreateTraits());

        for(int i = 0; i < characterInfo.characterTraits.Count; i++)
        {
            switch(characterInfo.characterTraits[i])
            {
                case 0:
                    //Smart
                    experienceModifier = 1.18f;
                    break;

                case 1:
                    //Dumb
                    experienceModifier = 0.82f;
                    break;

                case 2:
                    //Brawler - Per Combat
                    break;

                case 3:
                    //HardWorker - Per Build
                    break;

                case 4:
                    //Lazy - Per Build
                    break;

                case 5:
                    //Fast
                    agent.speed *= 1.18f;
                    break;

                case 6:
                    //Crazed - Per Combat
                    break;
            }
        }

        //Set attack rate
        attackCooldownTime = 5 - (characterInfo.characterAttributes.nimbleness / 10);
        if(attackCooldownTime <= 0)
        {
            attackCooldownTime = 1.0f;
        }

        currentHealth = characterInfo.characterHealth;

        //Set spawn position
        transform.position = spawnPosition;
		CalculateNextLevelExperience ();

    }

    protected List<int> CreateTraits()
    {
        List<int> traits = new List<int>();

        for(int i = 0; i < Random.Range(0, 4); i++)
        {
            switch(Random.Range(0, 14))
            {
                case 0:
                    traits.Add((int)CHARACTER_TRAIT.TRAIT_SMART);
                    Debug.Log("Add Smart");
                    break;

                case 2:
                    traits.Add((int)CHARACTER_TRAIT.TRAIT_DUMB);
                    Debug.Log("Add Dumb");
                    break;

                case 4:
                    traits.Add((int)CHARACTER_TRAIT.TRAIT_BRAWLER);
                    Debug.Log("Add Brawler");
                    break;

                case 6:
                    traits.Add((int)CHARACTER_TRAIT.TRAIT_HARDWORKER);
                    Debug.Log("Add Hard Worker");
                    break;

                case 8:
                    traits.Add((int)CHARACTER_TRAIT.TRAIT_LAZY);
                    Debug.Log("Add Lazy");
                    break;

                case 10:
                    traits.Add((int)CHARACTER_TRAIT.TRAIT_FAST);
                    Debug.Log("Add Fast");
                    break;

                case 12:
                    traits.Add((int)CHARACTER_TRAIT.TRAIT_CRAZED);
                    Debug.Log("Add Crazed");
                    break;

                default:
                    break;
            }
        }

        return traits;
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
	protected virtual void AIDead()
    {
        targetPosition = transform.position;
        targetObject = null;

        AudioSource.PlayClipAtPoint(Resources.Load("Sound/Sound_Death") as AudioClip, transform.position);

        Destroy(gameObject);
    }

    //If there's no target set, find a target, then set the destination using the NavMeshAgent
	protected virtual void AIMoveToTarget()
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
				if (targetObject.GetComponent<BaseBuilding> ().IsBuilt () == false) {
					currentState = CHARACTER_STATE.CHARACTER_BUILDING;
				} else {
					targetObject = null;
					currentState = CHARACTER_STATE.CHARACTER_WANDER;
				}
			} else if (targetObject.GetComponent<Character> () && AICheckWeaponRange ()) {
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

	protected bool AICheckWeaponRange()
	{
		if (Vector3.Distance (transform.position, targetPosition) <= attackRange)
			return true;
		else
			return false;
	}

	public float GetWeaponRange()
	{
		return attackRange;
	}

	public void SetWeaponRange(float range)
	{
		attackRange = range;
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

        if (AICheckWeaponRange())
        {
            if (isAttacking && targetObject != null)
            {
                attackTimer += Time.deltaTime;

                if (attackTimer >= attackCooldownTime)
                {

                    //Attack the target
                    AudioSource.PlayClipAtPoint(Resources.Load("Sound/Sound_Combat") as AudioClip, transform.position);
                    animator.SetTrigger("Melee Right Attack 01");

                    if (targetObject.GetComponent<Character>())
                    {
                        DealDamage(targetObject.GetComponent<Character>());
                    }
                    else if (targetObject.GetComponent<BaseBuilding>())
                    {
                        DamageBuilding(targetObject.GetComponent<BaseBuilding>());
                    }
                    attackTimer = 0;
                }
            }
            else if (!isAttacking && targetObject != null)
            {
                //Set attacking to true, start attack timer, stop navigation
                isAttacking = true;
                agent.enabled = false;
            }
        }
        else
        {
            isAttacking = false;
            agent.enabled = true;
            currentState = CHARACTER_STATE.CHARACTER_MOVING;
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
        float combatModifier = 1;

        if (currentHealth <= 10.0f && characterInfo.characterTraits.Contains(6))
            combatModifier += 0.18f;

        if (characterInfo.characterTraits.Contains(4))
            combatModifier += 0.18f;


        if (equippedWeapon != null)
        {
            //Deal damage to target character
            targetCharacter.TakeDamage(equippedWeapon.GetDamageValue() * combatModifier, this);
        }
        else
        {
            //If the character doesn't have an active weapon, use their brawling skill instead
                targetCharacter.TakeDamage(GetCombatSkills().brawling * combatModifier, this);
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
            animator.SetTrigger("Take Damage");
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
		if(inventory.CheckForItem(weaponToEquip))
			inventory.RemoveItem (weaponToEquip);

		SetWeaponRange (weaponToEquip.GetWeaponRange());

		equippedWeapon = weaponToEquip;

        if(equippedWeapon.IsTwoHanded())
        {
            offHandEnabled = false;
        }
    }

	public void UnequipMainHand()
	{
		if(GetEquippedWeapon() != null && GetEquippedWeapon().GetWeaponType() != BaseWeapon.WEAPON_TYPE.WEAPON_FISTS)
			inventory.AddItem (GetEquippedWeapon ());

		BaseWeapon emptyHand = new BaseWeapon ();
		emptyHand.SetItemType (BaseItem.ITEM_TYPE.ITEM_EMPTY);

		equippedWeapon = emptyHand;
	}

	public void UnequipOffHand()
	{
		if(GetOffHandWeapon() != null && GetEquippedWeapon().GetWeaponType() != BaseWeapon.WEAPON_TYPE.WEAPON_FISTS)
			inventory.AddItem (GetOffHandWeapon ());

		BaseWeapon emptyHand = new BaseWeapon ();
		emptyHand.SetItemType (BaseItem.ITEM_TYPE.ITEM_EMPTY);

		offHandWeapon = emptyHand;
	}

	public void EquipWeaponToOffHand(BaseWeapon weaponToEquip)
    {
        if (offHandEnabled)
        {
			if(inventory.CheckForItem(weaponToEquip))
				inventory.RemoveItem (weaponToEquip);
            offHandWeapon = weaponToEquip;
        }
    }

	public void EquipArmor(BaseArmor armorToEquip)
    {
		if(inventory.CheckForItem(armorToEquip))
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
			//Debug.Log ("No weapon equipped");
		}
        return equippedWeapon;
    }

    public BaseWeapon GetOffHandWeapon()
    {
        if(offHandWeapon == null)
        {
            //Debug.Log("No offhand weapon equipped");
        }
        return offHandWeapon;
    }

    public BaseArmor GetEquippedArmor()
    {
        return equippedArmor;
    }

}

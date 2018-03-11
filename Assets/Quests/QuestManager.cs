using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestManager : MonoBehaviour {

    private List<Sprite> imageList = new List<Sprite>() { new Sprite() };

    private List<string> missionNamesStart = new List<string>() { "Banished", "Black", "Bleeding", "Blind", "Blinding", "Bloody", "Broken", "Brutal", "Burning", "Cold", "Crimson", "Cryptic", "Crystal", "Dark", "Defiant", "Demon", "Devil's", "Driving", "Dying", "Empty", "Enduring", "Fading", "Fallen", "Final", "First", "Flying", "Forgotten", "Frozen", "Glass", "Hidden", "Hot", "Lazy", "Lone", "Lost", "Morbid", "Patient", "Purple", "Red", "Rotting", "Sacred", "Secret", "Severed", "Shattered", "Silent", "Soaring", "Spectral", "Stone", "Swift", "Twisted", "Unceasing", "Vengeful" };
	private List<string> missionNamesEnd = new List<string>() {"Apollo", "Bell", "Blade", "Breath", "Calm", "Crone", "Crown", "Daze", "Dream", "Druid", "Empire", "Engine", "Fall", "Father", "Fear", "Fog", "Future", "Grave", "God", "Hammer", "Hawk", "Hydra", "Hymn", "Jester", "Justice", "King", "Line", "Law", "Moon", "Mother", "Mountain", "Night", "Palace", "Paramour", "Pipe", "Priest", "Prophet", "Pyre", "Rain", "Ring", "Savior", "Scepter", "Serpent", "Shield", "Shroud", "Skull", "Smoke", "Stallion", "Star", "Stranger", "Stroke", "Summer", "Sword", "Tears", "Thorn", "Throne", "Thunder", "Vanguard", "Vengeance", "Whisper" };

	protected List<Quest> questList = new List<Quest> ();

	private GameObject questMenu;
	private GameObject questScroll;
	protected GameObject questPanel;

    private BaseManager baseManager;
    
    public void Init(GameObject menu, GameObject scroll)
    {
        baseManager = GetComponent<BaseManager>();

        questPanel = (GameObject)Resources.Load("UI/QuestPanel");
        questMenu = menu;
        questScroll = scroll;

        for (int i = 0; i < 3; i++)
        {
            AddQuest();
        }
    }

	public void ToggleQuestMenu()
	{
		if (questMenu.activeSelf) {
			questMenu.SetActive (false);
		} else {
			questMenu.SetActive (true);
		}
	}

	public void AddQuest()
	{
		//Debug.Log ("Add Quest");

		GameObject newPanel = Instantiate (questPanel, questScroll.transform);

		newPanel.GetComponent<Quest> ().Init (RandomName(), RandomSlots(), RandomImage(), RandomTime(), this, RandomDifficulty());
		questList.Add (newPanel.GetComponent<Quest>());
	}

    int RandomDifficulty()
    {
        return Random.Range(1, 10);
    }

	string RandomName()
	{
		string part1 = missionNamesStart[Random.Range (0, missionNamesStart.Count)];
		string part2 = missionNamesEnd[Random.Range (0, missionNamesEnd.Count)];

		return part1 + " " + part2;
	}

	int RandomSlots()
	{
		return Random.Range (1, 3);
	}

	Sprite RandomImage()
	{
		return imageList [Random.Range (0, imageList.Count)];
	}

    float RandomTime()
    {
        return Random.Range(30, 60);
    }
    
    public List<Quest> GetQuestList()
    {
        return questList;
    }
    public int GetQuestCount()
    {
        return questList.Count;
    }

    public void ActivateQuest(Quest questRef)
    {
        if(questRef.GetVillagerList().Count > 0)
        {
            //Set quest to active - Starts timer
            questRef.active = true;

            //Remove villagers from scene
            foreach(BaseVillager villager in questRef.GetVillagerList())
            {
                villager.SendOnQuest(questList.IndexOf(questRef));
                villager.gameObject.SetActive(false);
            }

            //Save character indexes from BaseManager list
            foreach(BaseVillager villager in questRef.GetVillagerList())
            {
                questRef.AddVillagerIndex(baseManager.GetVillagerIndex(villager));
            }

            if (baseManager.GetCharacterScroll() == null)
                    Debug.Log("Activate Quest - Character Scroll null");

            //Disable display buttons for villagers
            for(int i = 0; i < questRef.GetVillagerList().Count; i++)
            {
                Debug.Log("Disable character button for " + questRef.GetVillagerList()[i].GetName());
                baseManager.GetCharacterDisplay().DisableCharacterButton(questRef.GetVillagerIndexes()[i]);
            }

            //Change all buttons to uninteractable
            foreach(Button buttonRef in questRef.GetComponentsInChildren<Button>())
            {
                buttonRef.interactable = false;
            }
        }
    }

    public void QuestComplete(Quest finishedQuest)
    {
        //Character checks (Is Character Alive, XP Gained etc.)
        CharacterCheck(finishedQuest);

        //Create loot
        baseManager.GetInventory().AddItem(CreateLoot(finishedQuest));

        //Enable villagers
        foreach(BaseVillager villager in finishedQuest.GetVillagerList())
        {
            villager.gameObject.SetActive(true);
            //Enable villagers buttons==
            for (int i = 0; i < finishedQuest.GetVillagerList().Count; i++)
            {
                baseManager.GetCharacterDisplay().EnableCharacterButton(finishedQuest.GetVillagerIndexes()[i]);
            }
        }

        //Remove quest
        questList.Remove(finishedQuest);
        Destroy(finishedQuest.gameObject);

        //Create new quest
        AddQuest();
    }

    private void CharacterCheck(Quest quest)
    {
        for(int i = 0; i < quest.GetVillagerList().Count; i++)
        {
            if(quest.GetVillagerList()[i].IsAlive())
                quest.GetVillagerList()[i].AddExperience(quest.GetExperience());
        }
    }

    public void QuestEvent(Quest quest)
    {

    }

    private BaseItem CreateLoot(Quest quest)
    {

        BaseWeapon rewardItem;
        int rewardValue = Random.Range(1, 3) * quest.GetDifficulty();

        switch(Random.Range(0, 6))
        {
            case 0:
                rewardItem = new Weapon_Sword();
                rewardItem.SetBaseScore(rewardValue);
                break;

            case 1:
                rewardItem = new Weapon_Sword();
                rewardItem.SetBaseScore(rewardValue);
                break;

            case 2:
                rewardItem = new Weapon_Axe();
                rewardItem.SetBaseScore(rewardValue);
                break;

            case 3:
                rewardItem = new Weapon_Polearm();
                rewardItem.SetBaseScore(rewardValue);
                break;

            case 4:
                rewardItem = new Weapon_Bow();
                rewardItem.SetBaseScore(rewardValue);
                break;

            case 5:
                rewardItem = new Weapon_Longsword();
                rewardItem.SetBaseScore(rewardValue);
                break;

            case 6:
                rewardItem = new Item_Shield();
                rewardItem.SetBaseScore(rewardValue);
                break;

            default:
                rewardItem = new Weapon_Sword();
                rewardItem.SetBaseScore(rewardValue);
                break;
        }

        Debug.Log("Quest Reward Made");

        return rewardItem;
    }

    public void Load(List<QuestData> data)
    {

        Debug.Log(questList.Count);

        //Remove any quests that currently exist
        foreach(Quest quest in questList)
        {
            Destroy(quest.gameObject);
        }

        //Clear the quest list array
        questList.Clear();

        //Add each quest in the loaded data to the UI
        foreach(QuestData questData in data)
        {
            //Create a quest panel
            GameObject newPanel = Instantiate(questPanel, questScroll.transform);

            //Update the quest values based on the loaded data
            questList.Add(newPanel.GetComponent<Quest>());
            newPanel.GetComponent<Quest>().Load(questData, this);
        }

    }

}

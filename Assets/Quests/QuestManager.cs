using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestManager : MonoBehaviour {

	public List<Sprite> imageList = new List<Sprite>();

	public List<string> missionNamesStart = new List<string> ();
	public List<string> missionNamesEnd = new List<string> ();

	protected List<Quest> questList = new List<Quest> ();

	public GameObject questMenu;
	public GameObject questScroll;
	protected GameObject questPanel;

    private BaseManager baseManager;

	void Awake()
	{
        baseManager = GetComponent<BaseManager>();

		Debug.Log ("QuestManager start");
		questPanel = (GameObject)Resources.Load ("UI/QuestPanel");
	}

    public void Init()
    {
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

	void AddQuest()
	{
		//Debug.Log ("Add Quest");

		GameObject newPanel = Instantiate (questPanel, questScroll.transform);

		newPanel.GetComponent<Quest> ().Init (RandomName(), RandomSlots(), RandomImage(), RandomTime(), this);
		questList.Add (newPanel.GetComponent<Quest>());
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
                questRef.AddVillagerIndex(baseManager.villagerList.IndexOf(villager));
            }

            if (baseManager.characterScroll == null)
                    Debug.Log("Activate Quest - Character Scroll null");

            //Disable display buttons for villagers
            for(int i = 0; i < questRef.GetVillagerList().Count; i++)
            {
                Debug.Log("Disable character button for " + questRef.GetVillagerList()[i].GetName());
                baseManager.characterScroll.GetComponent<CharacterDisplay>().buttonList[questRef.GetVillagerIndexes()[i]].GetComponent<Button>().interactable = false;
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
        //Character checks (Is Character Alive, XP Gained etc.)==
        CharacterCheck(finishedQuest);

        //Create loot==

        //Enable villagers
        foreach(BaseVillager villager in finishedQuest.GetVillagerList())
        {
            villager.gameObject.SetActive(true);
            //Enable villagers buttons==
            for (int i = 0; i < finishedQuest.GetVillagerList().Count; i++)
            {
                baseManager.characterScroll.GetComponentsInChildren<Button>()[finishedQuest.GetVillagerIndexes()[i]].interactable = true;
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

    }

    private BaseItem CreateLoot()
    {
        BaseItem rewardItem = new BaseItem();


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

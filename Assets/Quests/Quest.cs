using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Quest : MonoBehaviour {

	public Text nameText;
	public GameObject characterPanel;
	public Image questImageSlot;
    public Text questTimeText;

	protected string questName;
	protected int characterSlots;
    protected float time;

	protected Sprite questImage;

	protected List<BaseVillager> activeVillagers = new List<BaseVillager> ();
    protected List<int> villagerIndexes = new List<int>();
	protected List<Button> buttonList = new List<Button> ();

	public GameObject characterButton;
	private BaseManager manager;
    private QuestManager questManager;

    public bool active;

	public void Init(string name, int slot, Sprite image, float newTime, QuestManager newManager)
	{
        manager = FindObjectOfType<BaseManager>();

        SetQuestName (name);
		SetCharacterSlots (slot);
		SetImage (image);
        SetTime(newTime);

        questManager = newManager;

		for (int i = 0; i < characterSlots; i++) {
			buttonList.Add(Instantiate (
				characterButton, 
				characterPanel.transform).GetComponent<Button>());
		}

		for (int i = 0; i < buttonList.Count; i++) {
			buttonList [i].onClick.AddListener (delegate{manager.SettingUpQuest(this);});
		}

		nameText.text = questName;
		questImageSlot.sprite = questImage;
        questTimeText.text = string.Format("{0}:{1:00}", (int)time / 60, (int)time % 60);
    }

    public void ActivateQuest()
    {
        questManager.ActivateQuest(this);
    }

    private void Update()
    {
        if(active)
        {
            time -= Time.deltaTime;
            questTimeText.text = string.Format("{0}:{1:00}", (int)time / 60, (int)time % 60);

            if (time <= 0)
        {
            active = false;
            questManager.QuestComplete(this);
        }

        }

        
    }

    private void UpdateButton(int value, BaseVillager villager)
	{
		buttonList [value].GetComponentInChildren<Image> ().sprite = villager.GetPortrait ();
		buttonList [value].GetComponentsInChildren<Text> () [0].text = villager.GetName ();
		buttonList [value].GetComponentsInChildren<Text> () [1].text = villager.GetLevel ().ToString();
	}

    public void AddVillagerIndex(int i)
    {
        villagerIndexes.Add(i);
    }

    public List<int> GetVillagerIndexes()
    {
        return villagerIndexes;
    }

    protected void SetTime(float newTime)
    {
        time = newTime;
    }
	
	protected void SetQuestName(string newName)
	{
		questName = newName;
	}

	protected void SetCharacterSlots(int maxCharacters)
	{
		characterSlots = maxCharacters;
	}

    public List<BaseVillager> GetVillagerList()
    {
        return activeVillagers;
    }

	protected void SetImage(Sprite newImage)
	{
		questImage = newImage;
	}

	public void AddCharacter(BaseVillager chosenVillager)
	{
		if (activeVillagers.Count < characterSlots) {
			activeVillagers.Add (chosenVillager);
			UpdateButton (activeVillagers.IndexOf(chosenVillager), chosenVillager);
		}
	}

    public QuestData Save()
    {
        QuestData dataToSave = new QuestData
        {
            time = time,
            name = questName,
            slots = characterSlots,
            active = active,

            villagerIndexes = villagerIndexes
        };
        //dataToSave.image = questImage;

        return dataToSave;
    }

    public void Load(QuestData dataToLoad, QuestManager questManager)
    {
        Debug.Log("Quest Load");

        //Set the villager indexes
        villagerIndexes = dataToLoad.villagerIndexes;

        //Init the quest with the loaded data
        Init(dataToLoad.name, dataToLoad.slots, null, dataToLoad.time, questManager);
        if (manager == null)
            Debug.Log("Manager is null");

        if (dataToLoad.active)
        {
            //Add each villager from the index
            for(int i = 0; i < villagerIndexes.Count; i++)
            {
                AddCharacter(manager.GetVillager(villagerIndexes[i]));
            }

            //Activate the quest properly
            questManager.ActivateQuest(this);
        }
    }
}

[System.Serializable]
public class QuestData
{
    public float time;
    public string name;
    public int slots;
    public bool active;

    public List<int> villagerIndexes;
    //public Sprite image;
}
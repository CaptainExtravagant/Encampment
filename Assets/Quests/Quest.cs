using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Quest : MonoBehaviour {

	public Text nameText;
	public GameObject characterPanel;
	public Image questImageSlot;

	protected string questName;
	protected int characterSlots;
    protected float time;

	protected Sprite questImage;

	protected List<BaseVillager> activeVillagers = new List<BaseVillager> ();
	protected List<Button> buttonList = new List<Button> ();

	public GameObject characterButton;
	private BaseManager manager;
    private QuestManager questManager;

    private bool active;

	void Awake () {
		//characterButton = (GameObject)Resources.Load ("UI/QuestCharacterButton");
		manager = FindObjectOfType<BaseManager>();
	}

	public void Init(string name, int slot, Sprite image, float newTime, QuestManager newManager)
	{
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
        }
    }

    private void UpdateButton(int value, BaseVillager villager)
	{
		buttonList [value].GetComponentInChildren<Image> ().sprite = villager.GetPortrait ();
		buttonList [value].GetComponentsInChildren<Text> () [0].text = villager.GetName ();
		buttonList [value].GetComponentsInChildren<Text> () [1].text = villager.GetLevel ().ToString();
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
        QuestData dataToSave = new QuestData();

        dataToSave.time = time;
        dataToSave.name = questName;
        dataToSave.slots = characterSlots;
        dataToSave.active = active;
        //dataToSave.image = questImage;
        
        return dataToSave;
    }

    public void Load(QuestData dataToLoad, QuestManager questManager)
    {
        Debug.Log("Quest Load");
        active = dataToLoad.active;
        Init(dataToLoad.name, dataToLoad.slots, null, dataToLoad.time, questManager);
        
        if(active)
        {
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
    //public Sprite image;
}
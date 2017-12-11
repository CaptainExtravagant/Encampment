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

	protected Sprite questImage;

	protected List<BaseVillager> activeVillagers = new List<BaseVillager> ();
	protected List<Button> buttonList = new List<Button> ();

	public GameObject characterButton;
	private BaseManager manager;

	void Awake () {
		//characterButton = (GameObject)Resources.Load ("UI/QuestCharacterButton");
		manager = FindObjectOfType<BaseManager>();
	}

	public void Init(string name, int slot, Sprite image)
	{
		SetQuestName (name);
		SetCharacterSlots (slot);
		SetImage (image);

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

	private void UpdateButton(int value, BaseVillager villager)
	{
		buttonList [value].GetComponentInChildren<Image> ().sprite = villager.GetPortrait ();
		buttonList [value].GetComponentsInChildren<Text> () [0].text = villager.GetName ();
		buttonList [value].GetComponentsInChildren<Text> () [1].text = villager.GetLevel ().ToString();
	}
	
	public void SetQuestName(string newName)
	{
		questName = newName;
	}

	public void SetCharacterSlots(int maxCharacters)
	{
		characterSlots = maxCharacters;
	}

	public void SetImage(Sprite newImage)
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
}

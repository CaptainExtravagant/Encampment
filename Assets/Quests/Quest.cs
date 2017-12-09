using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Quest : MonoBehaviour {

	public Text nameText;
	public GameObject characterPanel;

	protected string questName;
	protected int characterSlots;

	protected Image questImage;

	protected List<BaseVillager> activeVillagers = new List<BaseVillager> ();

	protected GameObject characterButton;

	void Awake () {
		characterButton = (GameObject)Resources.Load ("UI/QuestCharacterButton");
	}

	public void Init(string name, int slot, Image image)
	{
		SetQuestName (name);
		SetCharacterSlots (slot);
		SetImage (image);

		for (int i = 0; i < characterSlots; i++) {
			Instantiate (characterButton, characterPanel.transform);
		}
	}
	
	public void SetQuestName(string newName)
	{
		questName = newName;
	}

	public void SetCharacterSlots(int maxCharacters)
	{
		characterSlots = maxCharacters;
	}

	public void SetImage(Image newImage)
	{
		questImage = newImage;
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterDisplay : MonoBehaviour {

	public BaseManager manager;

	private GameObject characterPanel;
	private Sprite characterPortrait;
	private string characterName;
	private int characterLevel;

	public Scrollbar displayScroll;

	private Quest activeQuest;

	private GameObject panelParent;

	private Vector3 parentStart;
	private Vector3 parentEnd;

	private Image panelPortrait;
	private Text panelName;
	private Text panelLevel;

	void Awake()
	{
		characterPanel = (GameObject)Resources.Load ("UI/CharacterPanel");
		panelPortrait = characterPanel.GetComponentInChildren<Image> ();
		panelName = characterPanel.GetComponentsInChildren<Text> ()[0];
		panelLevel = characterPanel.GetComponentsInChildren<Text> () [1];

		panelParent = GetComponentInChildren<VerticalLayoutGroup> ().gameObject;
		parentStart = panelParent.transform.position;
		parentEnd = parentStart;
	}

	public void OpenMenuForQuests(Quest chosenQuest)
	{
		activeQuest = chosenQuest;
	}

	public Quest GetActiveQuest()
	{
		return activeQuest;
	}

	public void UpdateScroll()
	{
		Vector3 newPosition = Vector3.Lerp (parentStart, parentEnd, displayScroll.value);

		panelParent.transform.position = newPosition;
	}

	public void Init(GameObject villager)
	{
		characterPortrait = villager.GetComponent<BaseVillager>().GetPortrait();
		characterName = villager.GetComponent<BaseVillager>().GetName();
		characterLevel = villager.GetComponent<BaseVillager>().GetLevel();

		UpdateValues (villager.GetComponent<BaseVillager>());

		CharacterButton button = Instantiate (characterPanel, panelParent.transform).GetComponent<CharacterButton>();

		button.SetVillager (villager);

		parentEnd = new Vector3(parentEnd.x, parentEnd.y + 168, parentEnd.z);
	}

	void UpdateValues(BaseVillager villager)
	{
		characterPortrait = villager.GetPortrait();
		characterName = villager.GetName();
		characterLevel = villager.GetLevel();

		panelPortrait.sprite = characterPortrait;
		panelName.text = characterName;
		panelLevel.text = "Level: " + characterLevel.ToString ();
	}
}

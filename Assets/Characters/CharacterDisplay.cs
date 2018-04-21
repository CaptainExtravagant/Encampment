using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterDisplay : MonoBehaviour {

	private BaseManager manager;

	private GameObject characterPanel;

    private List<GameObject> buttonList = new List<GameObject>();

	private Sprite characterPortrait;
	private string characterName;
	private int characterLevel;

	private Scrollbar displayScroll;

	private Quest activeQuest;

	private GameObject panelParent;

	private Vector3 parentStart;
	private Vector3 parentEnd;

	private Image panelPortrait;
	private Text panelName;
	private Text panelLevel;

    public void Init(BaseManager baseManager, Scrollbar scrollbar)
    {
        manager = baseManager;
        displayScroll = scrollbar;

        characterPanel = (GameObject)Resources.Load("UI/CharacterPanel");
        panelPortrait = characterPanel.GetComponentInChildren<Image>();
        panelName = characterPanel.GetComponentsInChildren<Text>()[0];
        panelLevel = characterPanel.GetComponentsInChildren<Text>()[1];

        panelParent = GetComponentInChildren<VerticalLayoutGroup>().gameObject;
        parentStart = panelParent.transform.position;
        parentEnd = parentStart;
    }

    public void OpenPanel(BaseVillager villager)
    {

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

	public void AddVillager(GameObject villager)
	{
		UpdateValues (villager.GetComponent<BaseVillager>());

		CharacterButton button = Instantiate (characterPanel, panelParent.transform).GetComponent<CharacterButton>();

		button.SetVillager (villager);

        buttonList.Add(button.gameObject);

        if (buttonList.Count == 4)
            parentEnd = new Vector3(parentEnd.x, parentEnd.y + 84, parentEnd.z);
        else if (buttonList.Count > 4)
            parentEnd = new Vector3(parentEnd.x, parentEnd.y + 168, parentEnd.z);
	}

	public void UpdateButton(int index)
	{
		buttonList [index].GetComponentsInChildren<Text> ()[0].text = manager.GetVillager (index).GetName();
		buttonList [index].GetComponentsInChildren<Text> () [1].text = "Level: " + manager.GetVillager (index).GetLevel ().ToString();
	}

    public void DisableCharacterButton(int index)
    {
        buttonList[index].GetComponent<Button>().interactable = false;
    }

    public void EnableCharacterButton(int index)
    {
        buttonList[index].GetComponent<Button>().interactable = true;
    }

    public void RemoveAllButtons()
    {
        foreach(GameObject buttonObject in buttonList)
        {
            Destroy(buttonObject);
        }

        buttonList.Clear();

    }

	public void UpdateValues(BaseVillager villager)
	{
		characterPortrait = villager.GetPortrait();
		characterName = villager.GetName();
		characterLevel = villager.GetLevel();

		panelPortrait.sprite = characterPortrait;
		panelName.text = characterName;
		panelLevel.text = "Level: " + characterLevel.ToString ();
	}
}

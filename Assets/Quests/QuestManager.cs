using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestManager : MonoBehaviour {

	public List<Image> imageList = new List<Image>();

	public List<string> missionNamesStart = new List<string> ();
	public List<string> missionNamesEnd = new List<string> ();

	protected List<Quest> questList = new List<Quest> ();

	public GameObject questMenu;
	public GameObject questScroll;
	protected GameObject questPanel;

	void Awake()
	{
		questPanel = (GameObject)Resources.Load ("UI/QuestPanel");
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
		GameObject newPanel = Instantiate (questPanel, questMenu.transform);

		newPanel.GetComponent<Quest> ().Init (RandomName(), RandomSlots(), RandomImage());
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
		return Random.Range (1, 4);
	}

	Image RandomImage()
	{
		return imageList [Random.Range (0, imageList.Count)];
	}
}

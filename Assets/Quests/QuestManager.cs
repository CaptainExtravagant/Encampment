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

	void Awake()
	{
		Debug.Log ("QuestManager start");
		questPanel = (GameObject)Resources.Load ("UI/QuestPanel");


	}

	void Start()
	{
			for (int i = 0; i < 3; i++) {
				AddQuest ();
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
		return Random.Range (1, 3);
	}

	Sprite RandomImage()
	{
		return imageList [Random.Range (0, imageList.Count)];
	}
}

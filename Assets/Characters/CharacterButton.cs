using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterButton : MonoBehaviour {

	private Button buttonRef;
	private GameObject villager;
	private BaseManager manager;

	void Awake () {
		manager = FindObjectOfType<BaseManager> ();
	}

	public void SetVillager(GameObject newVillager)
	{
		villager = newVillager;
	}

	public void ButtonPressed()
	{
		manager.SelectCharacter (villager.GetComponent<BaseVillager> ());
	}
}

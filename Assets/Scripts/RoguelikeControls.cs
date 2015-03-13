using UnityEngine;
using System.Collections;

public class RoguelikeControls : MonoBehaviour 
{
	private string alphabet = "bcefghijklmnoqrtuvwxyz1234567890"; //minus wasd and p

	public EquipUI equipMenu = null;

	void executeControl(string key)
	{
		switch(key)
		{
			case "i":
				Container inventory = gameObject.GetComponent<Container>();
				inventory.showUI();
				break;
			case "c":
				equipMenu.gameObject.SetActive(true);
				break;
		}//switch
	}//executeControl
	
	// Update is called once per frame
	void Update () 
	{
		if(Time.timeScale == 0)
			return;

		foreach(char letter in alphabet)
		{
			if(Input.GetKeyDown(letter+""))
			{
				executeControl(letter + "");
			}//if
		}//foreach
	}//Update
}//RoguelikeControls

using UnityEngine;
using System.Collections;

public class RoguelikeControls : MonoBehaviour 
{
	private string alphabet = "bcefghijklmnoqrtuvwxyz1234567890"; //minus wasd and p

	void executeControl(string key)
	{
		switch(key)
		{
			case "i":
			print ("inventory");
			break;
		}//switch
	}//executeControl
	
	// Update is called once per frame
	void Update () 
	{
		foreach(char letter in alphabet)
		{
			if(Input.GetKeyDown(letter+""))
			{
				executeControl(letter + "");
			}//if
		}//foreach
	}
}

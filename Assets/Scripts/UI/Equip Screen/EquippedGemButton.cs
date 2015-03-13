using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class EquippedGemButton : MonoBehaviour 
{
	public int colNumber = 0;
	public int rowNumber = 0;

	public CharacterSheet stats = null;
	public Text valueCounter = null;
	public Image gemImage = null;

	// Update is called once per frame
	void Update () 
	{
		if(stats == null)
			return;

		int value = stats.getSlotValue(colNumber, rowNumber);
		valueCounter.text = "" + value;

		//TODO: SET THE IMAGE HERE BASED ON THE TYPE
		gemImage.enabled = (value != 0);
	}//Update
}//EquippedGemButton

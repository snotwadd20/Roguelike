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

	public Image buttonArt = null;

	private Color origColor;

	// Update is called once per frame
	void Update () 
	{
		if(stats == null)
			return;

		int value = stats.getSlotValue(colNumber, rowNumber);
		valueCounter.text = "" + value;

		//TODO: SET THE IMAGE HERE BASED ON THE TYPE
		gemImage.enabled = (value != 0);
		gemImage.sprite = gem.getSprite();

		if(gem.typeI == colNumber)
			gemImage.color = UColor.ChangeBrightness(gem.getColor(), 0.65f);
		else
			gemImage.color = gem.getColor();

	}//Update

	void OnEnable()
	{
		origColor = buttonArt.color;
	}//OnEnable
	
	public void select()
	{
		origColor = buttonArt.color;
		buttonArt.color = UColor.ChangeBrightness(buttonArt.color, 0.5f);
	}//select
	
	public void deSelect()
	{
		buttonArt.color = origColor;
	}//deSelect

	public Gem gem
	{
		get
		{
			return stats.gemSlots[colNumber, rowNumber];
		}//get
		set
		{
			stats.gemSlots[colNumber, rowNumber] = value;
		}//set
	}//gem
}//EquippedGemButton

using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class InventoryGemButton : MonoBehaviour 
{
	private Gem _gem = null;

	public Text valueCounter = null;
	public Image gemImage = null;
	public Image buttonArt = null;

	private Color origColor;



	// Update is called once per frame
	void Update () 
	{
		if(_gem == null)
		{
			valueCounter.text = "N";
			gemImage.enabled = false;
			return;
		}//if

		valueCounter.text = "" + _gem.value;
		gemImage.sprite = _gem.getSprite();


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
			return _gem;
		}//get
		set
		{
			_gem = value;
		}//set
	}//gem
}//InventoryGemButton

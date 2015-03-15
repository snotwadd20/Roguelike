using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class InventoryGemButton : MonoBehaviour 
{
	public Gem _gem = null;

	public Text valueCounter = null;
	public Image gemImage = null;
	public Image buttonArt = null;
	public Text countText = null;
	private Color origColor;

	public int count = 1;

	// Update is called once per frame
	void Update () 
	{
		if(_gem == null)
		{
			valueCounter.text = "N";
			gemImage.color = new Color(1,1,0);
			return;
		}//if

		valueCounter.text = "" + gem.value;
		countText.text = "" + count;

		gemImage.sprite = gem.getSprite();
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
			return _gem;
		}//get
		set
		{
			_gem = value;
		}//set
	}//gem
}//InventoryGemButton

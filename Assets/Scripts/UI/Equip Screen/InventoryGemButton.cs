using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class InventoryGemButton : MonoBehaviour 
{
	public Gem gem = null;

	public Text valueCounter = null;
	public Image gemImage = null;

	// Update is called once per frame
	void Update () 
	{
		if(gem == null)
		{
			valueCounter.text = "N";
			gemImage.enabled = false;
			return;
		}//if

		valueCounter.text = "" + gem.value;
		gemImage.sprite = gem.getSprite();
	}//Update
}//InventoryGemButton

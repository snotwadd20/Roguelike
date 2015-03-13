using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class GemFilteredContainer : MonoBehaviour 
{
	public GemPickable gemPick = null;

	public Container inventory = null;

	public Button buttonPrefab = null;
	public Transform contentPanel = null;
	public ScrollRect scrollRect = null;
	
	void Start () 
	{
		foreach(KeyValuePair<string, Pickable> item in inventory.contents)
		{
			Pickable pick = item.Value;
			gemPick = pick.GetComponent<GemPickable>();

			if(gemPick != null)
			{
				// do something with entry.Value or entry.Key
				GameObject button = ((GameObject)Instantiate(buttonPrefab.gameObject));
				InventoryGemButton igb = button.GetComponent<InventoryGemButton>();
				igb.gem = gemPick.gem;
				igb.gameObject.SetActive(true);
				igb.transform.SetParent(contentPanel);
				
				igb.transform.localScale = Vector3.one;
			}//if
		}//foreach

		Canvas.ForceUpdateCanvases();
		scrollRect.verticalNormalizedPosition = 0;
		Canvas.ForceUpdateCanvases();
	}//OnEnable
}

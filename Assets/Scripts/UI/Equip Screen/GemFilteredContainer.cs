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

	private InventoryGemButton igb = null;

	private List<GameObject> allContentObjects = null;
	void OnEnable () 
	{
		if(allContentObjects == null || allContentObjects.Count <= 0)
		{
			allContentObjects = new List<GameObject>();
		}//if
		List<string> nullKeys = new List<string>();
		foreach(KeyValuePair<string, Pickable> item in inventory.contents)
		{
			Pickable pick = item.Value;
			if(pick == null)
			{
				nullKeys.Add(item.Key);
			}//if

			gemPick = pick.GetComponent<GemPickable>();

			if(gemPick != null)
			{
				GameObject button = ((GameObject)Instantiate(buttonPrefab.gameObject));
				igb = button.GetComponent<InventoryGemButton>();
				igb.gem = gemPick.gem;
				igb.count = pick.count;
				igb.gameObject.SetActive(true);
				igb.transform.SetParent(contentPanel);
				
				igb.transform.localScale = Vector3.one;
				allContentObjects.Add(igb.gameObject);
			}//if
		}//foreach

		foreach(string nullKey in nullKeys)
		{
			inventory.Remove(nullKey);
		}//foreach

		Canvas.ForceUpdateCanvases();
		scrollRect.verticalNormalizedPosition = 0;
		Canvas.ForceUpdateCanvases();
	}//OnEnable

	void Update()
	{
		if(gemPick)
			igb.count = gemPick.pickableScript.count;
	}//Update
	void OnDisable()
	{
		CleanUp();
	}//OnDisable

	public void CleanUp()
	{
		//Flush the inventory so it'l get re-done
		for(int i=0; i < allContentObjects.Count; i++)
		{
			Destroy(allContentObjects[i]);
		}//for
		allContentObjects = null;
	}//CleanUp
}

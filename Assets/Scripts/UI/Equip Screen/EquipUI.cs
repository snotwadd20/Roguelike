using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;

public class EquipUI : MonoBehaviour 
{
	public InventoryGemButton invButtonSelected = null;
	public EquippedGemButton equipButtonSelected = null;

	private static Transform mainCanvas = null;

	public static EquipUI self = null;

	// Use this for initialization
	void OnEnable () 
	{
		if(self == null)
		{
			self = this;
		}//if

		if(mainCanvas == null)
			mainCanvas = GameObject.FindGameObjectWithTag("MainCanvas").transform;

		Time.timeScale = 0;
		transform.SetParent(mainCanvas, true);
	}//OnEnable

	void OnDisable () 
	{
		Time.timeScale = 1;
	}//OnEnable
	// Update is called once per frame
	void Update () 
	{
		PointerEventData cursor = new PointerEventData(EventSystem.current);                            // This section prepares a list for all objects hit with the raycast
		cursor.position = Input.mousePosition;

		List<RaycastResult> objectsHit = new List<RaycastResult> ();

		EventSystem.current.RaycastAll(cursor, objectsHit);

		InventoryGemButton igb = null;
		EquippedGemButton egb = null;

		bool foundClickable = false;
		if(Input.GetMouseButtonUp(0))
		{
			if(objectsHit.Count > 0)
			{
				foreach(RaycastResult obj in objectsHit)
				{
					igb = obj.gameObject.GetComponent<InventoryGemButton>();
					egb = obj.gameObject.GetComponent<EquippedGemButton>();

					if(igb)
					{
						foundClickable = true;
						doButtonSelection(igb);
					}//if
					if(egb)
					{
						foundClickable = true;
						doButtonSelection(egb);
					}//if
				}//foreach

				if(!foundClickable)
					deSelectAll();
			}//if
			else
			{
				deSelectAll();
			}//else
		}//if

		if(Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.C))
			Close();

	}//Update

	public void Close()
	{
		//Time.timeScale = 1;
		gameObject.SetActive(false);
		//gameObject.SetActive(false);
	}//Close

	private void doButtonSelection(MonoBehaviour selected)
	{
		if(selected.GetType() == (typeof(InventoryGemButton)))
		{
			invButtonSelected = selected as InventoryGemButton;
			invButtonSelected.select();
		}//if

		if(selected.GetType() == (typeof(EquippedGemButton)))
		{
			if(equipButtonSelected == null)
			{
				equipButtonSelected = selected as EquippedGemButton;
				equipButtonSelected.select();
			}//if
			else if(equipButtonSelected == selected as EquippedGemButton)
			{
				deSelectAll();
			}//else if
			else
			{
				//equipButtonSelected = selected as EquippedGemButton;
				swapGems(selected as EquippedGemButton, equipButtonSelected);
				(selected as EquippedGemButton).deSelect();
				deSelectAll();
			}//else
		}//if

		if(invButtonSelected != null && equipButtonSelected != null)
		{
			swapGems(invButtonSelected, equipButtonSelected);
			deSelectAll();
		}//if
	}//doButtonSelection

	private void deSelectAll()
	{
		if(invButtonSelected)
			invButtonSelected.deSelect();

		if(equipButtonSelected)
			equipButtonSelected.deSelect();

		invButtonSelected = null;
		equipButtonSelected = null;
	}//deSelectAll

	private void swapGems(EquippedGemButton gem1, EquippedGemButton gem2)
	{
		Gem temp = gem1.gem;
		gem1.gem = gem2.gem;
		gem2.gem = temp;
	}//swapGems

	private void swapGems(InventoryGemButton gem1, EquippedGemButton gem2)
	{
		Gem temp = gem1.gem;
		gem1.gem = gem2.gem;
		gem2.gem = temp;
	}//swapGems
}//EquipUI

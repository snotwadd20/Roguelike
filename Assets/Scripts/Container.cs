using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Container : MonoBehaviour 
{
	public Dictionary<string, Pickable> contents = null;
	public new string name = "Container";

	public ContainerUI currentGUI = null;

	// Use this for initialization
	void Awake () 
	{
		if(contents == null)
			contents = new Dictionary<string,Pickable>();
	}//Start
	
	// Update is called once per frame
	void Update () 
	{
	}

	public void showUI()
	{
		currentGUI = ContainerUI.Create(this);
	}//showUI

	public void hideUI()
	{
		if(currentGUI != null)
		{
			currentGUI.Close();
			currentGUI = null;
		}//if
	}//hideUI

	public void Add(Pickable item)
	{
		if(!contents.ContainsKey(item.type))
			contents.Add(item.type, item);
		else
		{
			contents[item.type].count+= item.count;
		}//else
		item.gameObject.SetActive(false);
	}//Add

	public void Remove(string type)
	{
		//If there are none, removes it from the dictionary
		if(contents[type] == null)
			contents.Remove(type);

		//If there are more left, removes one
		contents[type].count--;

		if(contents[type].count <= 0)
		{
			Destroy(contents[type]);
			contents.Remove(type);
		}//if
	}//Remove


}//Container

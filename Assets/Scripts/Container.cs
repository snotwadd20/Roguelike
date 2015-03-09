using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Container : MonoBehaviour 
{
	public Dictionary<string, Pickable> contents = null;
	public new string name = "Container";
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
		ContainerUI.Create(this);
	}//showUI

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


}//Container

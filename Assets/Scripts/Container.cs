using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Container : MonoBehaviour 
{
	Dictionary<string, Pickable> contents = null;

	// Use this for initialization
	void Start () 
	{
		if(contents == null)
			contents = new Dictionary<string,Pickable>();
	}//Start
	
	// Update is called once per frame
	void Update () 
	{
	
	}

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

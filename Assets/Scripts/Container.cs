using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class Container : MonoBehaviour 
{
	public Dictionary<string, Pickable> contents = null;
	public new string name = "<Container>";

	public ContainerUI currentGUI = null;

	public bool isInteractable = true;
	public bool isRandomlyFilled = true;

	private bool keyPressed = false;

	private static RandomSeed r = null;
	// Use this for initialization
	void Start () 
	{
		if(r == null)
			r = new RandomSeed(R_Map.self.seed);

		if(contents == null)
			contents = new Dictionary<string,Pickable>();

		if(isInteractable)
		{
			Collider2D bc2d = gameObject.GetComponent<Collider2D>();
			if(gameObject.GetComponent<Collider2D>() == null)
				bc2d = gameObject.AddComponent<BoxCollider2D>();

			bc2d.isTrigger = true;
		}//if

		fillRandomly();
	}//Start

	void fillRandomly()
	{
		if(!isRandomlyFilled)
			return;

		for(int i=0; i < r.getIntInRange(2, 5); i++)
		{
			Pickable loot = TreasureManager.SpawnLoot(Vector3.zero, this);
			Add(loot);
		}//for
	}//fillRandomly

	void OnTriggerEnter2D(Collider2D coll)
	{
		if(isInteractable)
		{
			ActLog.print("Press G to loot the " + name);
		}//if
	}//OnTriggerEnter2D
	
	void OnTriggerStay2D(Collider2D coll)
	{
		if(isInteractable && keyPressed)
		{
			Container inventory = R_Player.self.GetComponent<Container>();
			inventory.AddContainer(this);
			Destroy(gameObject);
			TurnManager.NextTurn();
		}//if
	}//if

	// Update is called once per frame
	void Update () 
	{
		if(isInteractable)
			keyPressed = Input.GetKeyDown(KeyCode.G);
	}//Update

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

	public void AddContainer(Container container)
	{
		foreach(KeyValuePair<string, Pickable> thing in container.contents)
		{
			ActLog.print("You got a " + thing.Value.name + "!");
			Add(thing.Value);
		}//foreach
	}//AddContainer

	public void Remove(string type)
	{
/*		//If there are none, removes it from the dictionary
		if(contents[type] == null)
			contents.Remove(type);
*/
		//If there are more left, removes one
		contents[type].count--;

		if(contents[type].count <= 0)
		{
			Destroy(contents[type]);
			contents.Remove(type);
		}//if
	}//Remove
}//Container

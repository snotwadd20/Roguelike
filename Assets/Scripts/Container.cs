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

	private CharacterSheet stats = null;
	// Use this for initialization
	void Start () 
	{
		if(r == null)
			r = new RandomSeed(R_Map.self.seed);

		if(contents == null)
			contents = new Dictionary<string,Pickable>();

		if(stats == null)
			stats = R_Player.self.GetComponent<CharacterSheet>();

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

		bool hasSpawnedGem = !isInteractable;
		int baseTreasure = 2;
		int maxTreasure = baseTreasure + Mathf.Max(1, Mathf.RoundToInt(stats.Luck/5));
		for(int i=0; i < r.getIntInRange(baseTreasure, maxTreasure); i++)
		{
			Pickable loot = TreasureManager.SpawnLoot(Vector3.zero, this, !hasSpawnedGem);

			if(isInteractable && loot.GetComponent<GemPickable>())
				hasSpawnedGem = true;

			Add(loot);
		}//for
	}//fillRandomly

	void OnTriggerEnter2D(Collider2D coll)
	{
		if(isInteractable)
		{
			ActLog.print("Press <color=lime>[L]</color> to loot the " + name );
		}//if
	}//OnTriggerEnter2D
	
	void OnTriggerStay2D(Collider2D coll)
	{
		if(isInteractable && keyPressed && !RoguelikeControls.isPaused)
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
			keyPressed = Input.GetKeyDown(KeyCode.L);
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
		{
			contents.Add(item.type, item);
		}//if
		else if(contents[item.type] == null)
		{
			Remove(item.type);
			contents.Add(item.type, item);
		}//else if
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
			ActLog.print("You got a <color=cyan>" + thing.Value.name + "</color>!");
			Add(thing.Value);
		}//foreach
	}//AddContainer

	public void Remove(string type)
	{
		if(contents.ContainsKey(type))
		{
			//If there are more left, removes one
			contents[type].count--;

			if(contents[type].count <= 0)
			{
				Destroy(contents[type]);
				contents.Remove(type);
			}//if
		}//if
	}//Remove
}//Container

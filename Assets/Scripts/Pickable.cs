﻿using UnityEngine;
using System.Collections;

public class Pickable : MonoBehaviour 
{
	//Metadata
	public new string name = "Item";
	public string type = "Nada";
	public int count = 1;

	private InventoryClickCallback _callback = null;

	public Container holdingContainer = null;

	public InventoryClickCallback callback
	{
		get
		{
			return _callback;
		}//get
	}//callback

	// Use this for initialization
	void Start () 
	{
		CircleCollider2D cc2d = gameObject.AddComponent<CircleCollider2D>();
		cc2d.isTrigger = true;
		cc2d.radius *= 1.25f;
	}//Start
	
	public void OnTriggerEnter2D()
	{
		ActLog.print("press P to pick up");
	}//OnTriggerEnter2D

	public void pick(ref Container container)
	{
		ActLog.print ("You picked up the " + name + ".");
		container.Add(this);
		holdingContainer = container;
	}//pick

	public void use()
	{
		if(_callback != null)
			_callback(this);
	}//use

	public void RegisterCallback(InventoryClickCallback callback)
	{
		this._callback = callback;
	}//

	public delegate void InventoryClickCallback(Pickable pickable);
}//Pickable

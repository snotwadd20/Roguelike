﻿using UnityEngine;
using System.Collections;

public class HealingItem : MonoBehaviour 
{
	public new string name = "Healing item";
	public Pickable pickableScript = null;
	public int count  = 1;
	public float healingAmount = 100;
	public const string TYPE = "healingItem";

	// Use this for initialization
	void OnEnable () 
	{
		if(pickableScript == null)
			pickableScript = gameObject.GetComponent<Pickable>();

		if(pickableScript == null)
			pickableScript = gameObject.AddComponent<Pickable>();

		pickableScript.name = name;
		pickableScript.type = TYPE;
		pickableScript.count = count;
		pickableScript.RegisterCallback(healUser);
	}

	public void healUser(Pickable pickable)
	{
		Container holder = pickable.holdingContainer;
		PlayerHealth ph = holder.gameObject.GetComponent<PlayerHealth>();
		EnemyHealth eh = holder.gameObject.GetComponent<EnemyHealth>();

		if(ph)
		{
			ph.refillSomeHealth(healingAmount);
			ActLog.print(ph.gameObject.name + " used " + name + " for +" + healingAmount + "HP!");
			TurnManager.NextTurn();
		}//if

		if(eh)
		{
			eh.refillSomeHealth(healingAmount);
			ActLog.print(eh.gameObject.name + " used " + name + " for +" + healingAmount + "HP!");
		}//if

		holder.Remove(pickable.type);
	}//healUser
}//HealingItem

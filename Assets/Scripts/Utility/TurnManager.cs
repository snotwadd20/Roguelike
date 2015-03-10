using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TurnManager 
{
	//SINGLETON
	private static TurnManager _self = null;

	public delegate void TurnManagerCallback(int turnNumber);
	
	private List<Callback> callbacks = null;
	
	private int _turnNumber = 0;

	public int turnNumber
	{
		get
		{
			return _turnNumber;
		}//get
	}//turnNumber

	public static TurnManager self
	{
		get
		{
			if(_self == null)
				_self = new TurnManager();

			return _self;
		}//get
	}//self


	// Use this for initialization
	public TurnManager () 
	{
		if(callbacks == null)
		{
			callbacks = new List<Callback>();
		}//if
		
	}//constructor

	public void nextTurn()
	{
		_turnNumber++;
		doCallbacks();
	}//NextTurn

	public void registerCallback(GameObject owner, TurnManagerCallback callBack)
	{
		callbacks.Add(new Callback(owner, callBack));
	}//RegisterCallback


	private void doCallbacks()
	{
		for(int i=0; i < callbacks.Count;i++)
		{
			//Get rid of dead entries
			if(callbacks[i].owner == null)
			{
				callbacks.RemoveAt(i);
				i--;
				continue;
			}//if
			
			callbacks[i].callBack(turnNumber);
		}//for
	}//doCallbacks

	private class Callback
	{
		public GameObject owner = null;
		public TurnManagerCallback callBack = null;
		public Callback(GameObject owner, TurnManagerCallback function)
		{
			this.owner = owner;
			this.callBack = function;
		}//constructor
	}//Callback
	public static int TurnNumber
	{
		get
		{
			return self.turnNumber;
		}//get
	}//TurnNumber

	public static void NextTurn()
	{
		self.nextTurn();
	}//NextTurn

	public static void RegisterCallback(GameObject owner, TurnManagerCallback callBack)
	{
		self.registerCallback(owner, callBack);
	}//RegisterCallback
}//TurnManager
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//A class to log the action
public class ActLog 
{
	//SINGLETON
	private static ActLog _self = null;

	public static ActLog self
	{
		get
		{
			if(_self == null)
				_self = new ActLog();
			
			return _self;
		}//get
	}//self

	// Use this for initialization
	public ActLog () 
	{
	}//constructor

}//ActLogs
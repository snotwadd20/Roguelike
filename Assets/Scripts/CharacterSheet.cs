using UnityEngine;
using System.Collections;

public class CharacterSheet : MonoBehaviour 
{
	public const int NUM_GEM_ROWS = 3;

	public Gem[,] gemSlots = null;

	public float Attack
	{
		get
		{
			return gemColumnValue((int)Gem.Kind.Attack);
		}//get
	}//Attack

	public float Defense
	{
		get
		{
			return gemColumnValue((int)Gem.Kind.Defense);	
		}//get
	}//Defense

	public float Luck
	{
		get
		{
			return gemColumnValue((int)Gem.Kind.Luck);	
		}//get
	}//Luck

	// Use this for initialization
	void OnEnable () 
	{
		if(gemSlots == null)
			gemSlots = new Gem[Gem.NUM_KINDS, NUM_GEM_ROWS];

		for(int y =0; y < NUM_GEM_ROWS; y++)
		{
			for(int x =0; x < Gem.NUM_KINDS; x++)
			{
				gemSlots[x,y] = new Gem(1, Gem.Kind.None);
			}//for
		}//for
	}//OnEnable
	
	public int getSlotValue(int x, int y)
	{
		if(gemSlots[x,y] == null)
			return 0;

		return gemSlots[x,y].value;
	}//getSlotValue

	public void SpawnUI()
	{
		//Two containers, and can drag between.
		//Null is no gem
		//Otherwise there's a gem object there
	}//SpawnUI

	private float gemColumnValue(int column)
	{
		int total = 0;
		for(int i=0; i < NUM_GEM_ROWS; i++)
		{
			if(gemSlots[column, i] != null)
			{
				int gemValue = gemSlots[column,i].value;
				
				//If the colors/types match, multiply by two
				if(gemSlots[column, i].typeI == column)
					gemValue *= 2;
				
				total += gemValue;
			}//if
		}//for
		return total;		
	}//
	
	public float getStat(int type)
	{
		return getStat((Gem.Kind)type);
	}//getStat
	
	public float getStat(Gem.Kind type)
	{
		if(type == Gem.Kind.Attack)
			return Attack;
		if(type == Gem.Kind.Defense)
			return Defense;
		if(type == Gem.Kind.Luck)
			return Luck;

		return -1;
	}//getStat
}//CharacterSheet

public class Gem
{
	public const int NUM_KINDS = (int)Kind._NUM_KINDS;

	public int value = 1;
	public Kind type = Kind.Attack;

	public enum Kind {Attack, Defense, Luck, _NUM_KINDS, None};

	public Gem(int value, Kind kind)
	{
		this.value = value;
		this.type = kind;
	}//constructor

	public Gem(int value, int kind)
	{
		this.value = value;
		this.type = (Kind)kind;
	}//constructor

	public int typeI
	{
		get
		{
			return (int)type;
		}//get
		set
		{
			type = (Kind)value;
		}//set
	}//typeI

	public Sprite getSprite()
	{
		//BASED ON VALUE
		return null;
	}//getSprite
}//Gem
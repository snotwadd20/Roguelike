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

		/*for(int y =0; y < NUM_GEM_ROWS; y++)
		{
			for(int x =0; x < Gem.NUM_KINDS; x++)
			{
				gemSlots[x,y] = new Gem((int)Random.Range(1,3), x%3);
			}//for
		}//for*/
	}//OnEnable
	
	public int getSlotValue(int x, int y)
	{
		if(gemSlots[x,y] == null)
			return 0;

		if(gemSlots[x, y].typeI == x)
			return gemSlots[x,y].value * 2;

		return gemSlots[x,y].value;
	}//getSlotValue

	public void SpawnUI()
	{
		//TODO
	}//SpawnUI

	private float gemColumnValue(int column)
	{
		int total = 0;
		for(int i=0; i < NUM_GEM_ROWS; i++)
		{
			if(gemSlots[column, i] != null)
			{
				int gemValue = getSlotValue(column,i);
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
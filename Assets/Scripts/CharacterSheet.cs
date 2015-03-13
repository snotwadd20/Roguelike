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
				gemSlots[x,y] = new Gem((int)Random.Range(1,3), (Gem.Kind)(int)Random.Range(1,3));
			}//for
		}//for
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

public class Gem
{
	public const int NUM_KINDS = (int)Kind._NUM_KINDS;

	public int value = 1;
	public Kind type = Kind.Attack;

	public enum Kind {Attack, Defense, Luck, _NUM_KINDS, None};

	private static Sprite attackSprite = null;
	private static Sprite defenseSprite = null;
	private static Sprite luckSprite = null;
	private static Sprite noneSprite = null;

	public Gem(int value, Kind kind)
	{
		this.value = value;
		this.type = kind;

		if(attackSprite == null)
			attackSprite = Resources.Load<Sprite>("UI/Gems/Attack");

		if(defenseSprite == null)
			defenseSprite = Resources.Load<Sprite>("UI/Gems/Defense");

		if(luckSprite == null)
			luckSprite = Resources.Load<Sprite>("UI/Gems/Luck");
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
		if(type == Kind.Attack)
			return attackSprite;

		if(type == Kind.Defense)
			return defenseSprite;

		if(type == Kind.Luck)
			return luckSprite;

		if(type == Kind.None)
			return noneSprite;

		return null;
	}//getSprite

	public Color getColor()
	{
		if(type == Kind.Attack)
			return Color.red;
		
		if(type == Kind.Defense)
			return Color.blue;
		
		if(type == Kind.Luck)
			return Color.green;

		if(type == Kind.None)
			return Color.gray;
		
		return new Color(1,1,0);
	}//getColor
}//Gem
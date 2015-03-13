using UnityEngine;
using System.Collections;

public class Gem
{
	public int value = 1;
	public Kind type = Kind.None;
	
	public enum Kind {Attack, Defense, Luck, _NUM_KINDS, None};

	public const int NUM_KINDS = (int)Kind._NUM_KINDS;

	private static Sprite attackSprite = null;
	private static Sprite defenseSprite = null;
	private static Sprite luckSprite = null;
	private static Sprite noneSprite = null;
	
	public Gem(int value, Kind kind)
	{
		this.value = value;
		this.type = kind;
		
		if(attackSprite == null)
			attackSprite = Resources.Load<Sprite>("Sprites/Gems/Attack");
		
		if(defenseSprite == null)
			defenseSprite = Resources.Load<Sprite>("Sprites/Gems/Defense");
		
		if(luckSprite == null)
			luckSprite = Resources.Load<Sprite>("Sprites/Gems/Luck");
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
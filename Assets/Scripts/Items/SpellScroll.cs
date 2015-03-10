using UnityEngine;
using System.Collections;

public delegate void SpellCallback(Collider2D[] colls, Vector3 mousePos, Transform caster);
public enum SpellType {MagicMissile, Fireball};

public class SpellScroll : MonoBehaviour 
{
	public new string name = "Healing item";
	public Pickable pickableScript = null;

	public SpellCallback spellCastFunction = null;

	public SpellType spellType = SpellType.MagicMissile;

	public int count  = 1;
	// Use this for initialization
	void Start () 
	{
		if(pickableScript == null)
			pickableScript = gameObject.GetComponent<Pickable>();
		
		if(pickableScript == null)
			pickableScript = gameObject.AddComponent<Pickable>();
		
		pickableScript.name = name;
		pickableScript.type = spellType + "";
		pickableScript.count = count;
		pickableScript.callback = castSpell;

		if(spellCastFunction == null)
		{
			if(spellType == SpellType.MagicMissile)
				spellCastFunction = Spells.MagicMissile;
			else if(spellType == SpellType.Fireball)
				spellCastFunction = Spells.Fireball;
		}//if
	}//Start
	
	public void castSpell(Pickable pickable)
	{
		ActLog.print("You use a " + name);
		Container holder = pickable.holdingContainer;
		holder.hideUI();

		Targeter.self.gameObject.SetActive(true);
		Targeter.self.callback = spellCastFunction;
		holder.Remove(pickable.type);
	}//castSpell



}//SpellScroll


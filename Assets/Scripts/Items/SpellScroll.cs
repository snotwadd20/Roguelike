using UnityEngine;
using System.Collections;

public delegate void SpellCallback(Collider2D[] colls, Vector3 mousePos, Transform caster);

public class SpellScroll : MonoBehaviour 
{
	public new string name = "Healing item";
	public Pickable pickableScript = null;


	public float damageAmount = 100;
	public const string TYPE = "spellScroll";

	public SpellCallback spellCastFunction = null;

	public int count  = 1;
	// Use this for initialization
	void Start () 
	{
		if(pickableScript == null)
			pickableScript = gameObject.GetComponent<Pickable>();
		
		if(pickableScript == null)
			pickableScript = gameObject.AddComponent<Pickable>();
		
		pickableScript.name = name;
		pickableScript.type = TYPE;
		pickableScript.count = count;
		pickableScript.callback = castSpell;

		if(spellCastFunction == null)
			spellCastFunction = Spells.MagicMissile;
	}//Start
	
	public void castSpell(Pickable pickable)
	{
		Container holder = pickable.holdingContainer;
		holder.hideUI();

		Targeter.self.gameObject.SetActive(true);
		Targeter.self.callback = spellCastFunction;
		holder.Remove(pickable.type);
	}//castSpell

}//SpellScroll


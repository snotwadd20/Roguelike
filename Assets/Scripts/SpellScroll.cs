using UnityEngine;
using System.Collections;

public delegate void SpellCallback(Collider2D[] colls, Vector3 mousePos);

public class SpellScroll : MonoBehaviour 
{
	public new string name = "Healing item";
	public Pickable pickableScript = null;


	public float damageAmount = 100;
	public const string TYPE = "spellScroll";

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
	}
	
	public void castSpell(Pickable pickable)
	{
		Container holder = pickable.holdingContainer;
		holder.hideUI();

		Targeter.self.enabled = true;
		Targeter.self.callback = magicMissile;
		holder.Remove(pickable.type);
	}//castSpell

	private void magicMissile(Collider2D[] colls, Vector3 mousePos)
	{
		print ("MAGIC MISSILE!!! " + mousePos + ((colls != null && colls.Length > 0) ? " TARGET HIT: " + colls[0].name : ""));
	}//magicMissile

}//SpellScroll


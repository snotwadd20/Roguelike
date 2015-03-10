using UnityEngine;
using System.Collections;

public class Spells
{
	public static void MagicMissile(Collider2D[] colls, Vector3 mousePos, Transform caster)
	{
		Missile missile = Missile.Create(caster.transform, mousePos, 20.0f, null);
		missile.onExplode = (RaycastHit2D[] hits) => 
		{
			string nm = "HIT: ";
			if(hits != null && hits.Length > 0)
			{
				foreach(RaycastHit2D hit in hits)
					nm += hit.transform.name + ", ";
			}//if

			//Debug.Log("Missile: " + caster.name + " -> " + nm + " | " + missile.transform.position + "->" + mousePos);
			CheckForHits(hits, caster, 10);
			//missile.gameObject.SetActive(false);
		};//missile.OnExplode
		SpriteRenderer sr = missile.gameObject.AddComponent<SpriteRenderer>();
		sr.sprite = Resources.Load<Sprite>("Sprites/potion");
		sr.sortingOrder = 1;
	}//magicMissile

	public static void CheckForHits(RaycastHit2D[] hits, Transform caster, float damage)
	{
		foreach(RaycastHit2D hit in hits)
		{
			if(hit.transform.gameObject != caster.gameObject)
			{
				DoDamage(hit.transform.gameObject, caster.gameObject, damage);
			}//if
		}//foreach
	}//

	public static void DoDamage(GameObject target, GameObject attacker, float numDamage)
	{
		ActLog.print(attacker.name + " did " + numDamage + " damage to " + target.name);
		PlayerHealth ph = target.gameObject.GetComponent<PlayerHealth>();
		EnemyHealth eh = target.gameObject.GetComponent<EnemyHealth>();

		if(ph)
		{
			ph.dealDamage(numDamage, attacker.transform.position);
		}//if

		if(eh)
		{
			eh.dealDamage(numDamage);
		}//if

	}//DoDamage
}//Spells
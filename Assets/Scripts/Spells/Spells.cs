using UnityEngine;
using System.Collections;

public class Spells
{
	public static GameObject magicMisslePrefab = null;
	public static GameObject fireballPrefab = null;
	public static GameObject explosionPrefab = null;

	private static CharacterSheet stats = null;

	private static RandomSeed _randSeed = null;
	private static RandomSeed r
	{
		get
		{
			if(_randSeed == null)
				_randSeed = new RandomSeed(R_Map.self.seed);

			return _randSeed;
		}//get
	}//RandomSeed

	public static void MagicMissile(Collider2D[] colls, Vector3 mousePos, Transform caster)
	{
		float damage = decideDamage() * 2;
		if(magicMisslePrefab == null)
		{
			magicMisslePrefab = Resources.Load<GameObject>("Effects/magicMissile");
		}//if

		Missile missile = Missile.Create(caster.transform, mousePos, 10, null);
		missile.onExplode = (RaycastHit2D[] hits) => 
		{
			ActLog.print(caster.name + "'s magic missile explodes!");
			CheckForHits(hits, caster, damage);
			CameraShake.Shake(Camera.main, 0.1f, 0.15f, 1.73f, Vector2.zero);
		};//missile.OnExplode

		GameObject missileArt = (GameObject)GameObject.Instantiate(magicMisslePrefab);
		missileArt.transform.position = missile.transform.position;
		missileArt.transform.parent = missile.transform;
	}//magicMissile

	public static float decideDamage()
	{
		if(stats == null)
			stats = R_Player.self.GetComponent<CharacterSheet>();


		float mult = rollToAttack();
		float extraDamage = (mult * stats.Attack);
		float damage = XPManager.CurrentPlayerLevel + extraDamage;
		return damage;
	}//decideDamage


	
	public static float rollToAttack()
	{

		//Return damage multiplier
		float plusCritPerLuck = 2.0f;
		float baseCrit = 10.0f + plusCritPerLuck * stats.Luck;
		float baseHit = (100 - baseCrit) * (1.0f/3.0f);

		float roll = r.getIntInRange(1,100);
		if(roll <= baseCrit)
			return 4;
		else if(roll <= baseHit)
			return 2;

		return 1;
	}//rollToAttack


	public static void Fireball(Collider2D[] colls, Vector3 mousePos, Transform caster)
	{
		float damage = decideDamage() * 5;
		float radius = 3.0f;
		if(fireballPrefab == null)
		{
			fireballPrefab = Resources.Load<GameObject>("Effects/fireball");
		}//if

		if(explosionPrefab == null)
		{
			explosionPrefab = Resources.Load<GameObject>("Effects/unitExplosion");
		}//if
		
		Missile missile = Missile.Create(caster.transform, mousePos, 20.0f, null);
		missile.onExplode = (RaycastHit2D[] hits) => 
		{
			GameObject explosion = (GameObject)GameObject.Instantiate(explosionPrefab);
			explosion.transform.position = missile.transform.position;
			explosion.transform.localScale = Vector3.one * (radius*2);

			//Spawn an AOE
			AOEBurst.Create(caster, missile.transform.position, radius, (Collider2D[] colliders) =>
			{
				ActLog.print(caster.name + "'s fireball explodes!");
				foreach(Collider2D coll in colliders)
				{
					DoDamage(coll.gameObject, missile.caster, damage);
				}//foreach
				CameraShake.Shake(Camera.main, 0.45f, 0.5f, 1, Vector2.zero);

			});
		};//missile.OnExplode
		
		GameObject fireballArt = (GameObject)GameObject.Instantiate(fireballPrefab);
		fireballArt.transform.position = missile.transform.position;
		fireballArt.transform.parent = missile.transform;
	}//Fireball

	public static void CheckForHits(Collider2D[] hits, Transform caster, float damage)
	{
	}//CheckForHits

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

		if(ph || eh)
			ActLog.print(attacker.name + " did " + numDamage + " damage to " + target.name);
	}//DoDamage
}//Spells
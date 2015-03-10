using UnityEngine;
using System.Collections;

public class Missile : MonoBehaviour 
{
	public Vector3 target;
	//private Vector3 source;

	public float speed = 5.0f;

	public ExplodeCallback onExplode = null;

	private GameObject caster = null;
	public LayerMask mask = 0;
	void Start () 
	{
		//source = transform.position;

		if(onExplode == null)
			onExplode = (RaycastHit2D[] hits) => {print ("BOOM!");gameObject.SetActive(false);};

		gameObject.layer = LayerMask.NameToLayer("Projectiles");

	}//Start

	void FixedUpdate () 
	{
		Vector3 oldPos = transform.position;

		Vector3 moveVector = (target - oldPos).normalized * speed * Time.deltaTime;
		transform.position += moveVector;

		//Which things don't we want to collide with
		mask = 1 << gameObject.layer;

		if(caster)
		mask |= 1 << caster.layer;

		//Flip the bits
		mask = ~mask;

		RaycastHit2D[] hits = Physics2D.RaycastAll(oldPos, moveVector, moveVector.magnitude, mask);
		if((hits != null && hits.Length > 0) || Vector2.Distance(transform.position, target) <= 0.2f)
		{
			//transform.position = target;
			explode (hits);
		}//if

	}//Update

	void explode(RaycastHit2D[] hits)
	{
		if(onExplode != null)
			onExplode(hits);

		if(caster == R_Player.self.gameObject)
			TurnManager.NextTurn();

		Destroy(gameObject);
	}//Explode

	public static Missile Create(Transform caster, Vector3 target, float speed, ExplodeCallback onExplode)
	{
		Missile missile = new GameObject("Missile").AddComponent<Missile>();

		target.z = caster.position.z;

		//missile.source = source;
		missile.transform.position = caster.position;
		missile.target = target;
		missile.speed = speed;
		missile.onExplode = onExplode;
		missile.caster = caster.gameObject;

		return missile;
	}//Create

	public delegate void ExplodeCallback(RaycastHit2D[] hits);

}//Missile

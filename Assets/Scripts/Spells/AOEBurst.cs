using UnityEngine;
using System.Collections;

public class AOEBurst : MonoBehaviour 
{
	private GameObject caster = null;

	private float radius = 10.0f;
	public LayerMask mask = 0;

	public ExplodeCallback onExplode = null;

	private bool isInitialized = false;

	void OnEnable () 
	{
		if(onExplode == null)
			onExplode = (Collider2D[] hits) => {print ("BOOM!");gameObject.SetActive(false);};
	}//Start

	void Update()
	{
		if(!isInitialized)
		{
			gameObject.layer = LayerMask.NameToLayer("Projectiles");
			
			//Which things don't we want to collide with
			mask = 1 << gameObject.layer;
			
			if(caster)
				mask |= 1 << caster.layer;
			
			//Flip the bits
			mask = ~mask;
			
			Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, radius, mask);
			Debug.DrawRay(transform.position, Vector3.up * radius, Color.red, 10);
			Debug.DrawRay(transform.position, Vector3.up*-1 * radius, Color.red, 10);
			Debug.DrawRay(transform.position, Vector3.right*-1 * radius, Color.red, 10);
			Debug.DrawRay(transform.position, Vector3.right * radius, Color.red, 10);

			if((hits != null && hits.Length > 0))
			{
				explode (hits);
			}//if
			isInitialized = true;
		}//isInitialized
		else
		{
			Destroy(gameObject);
		}//else

	}//Update
	
	void explode(Collider2D[] hits)
	{
		if(onExplode != null)
			onExplode(hits);
		
		if(caster == R_Player.self.gameObject)
			TurnManager.NextTurn();
		
		CameraShake.Shake(Camera.main, 0.1f, 0.15f, 1.73f, Vector2.zero);
		
		Destroy(gameObject);
	}//Explode
	
	public static AOEBurst Create(Transform caster, Vector3 target, float radius, ExplodeCallback onExplode)
	{
		AOEBurst burst = new GameObject("AOEBurst").AddComponent<AOEBurst>();
		
		target.z = caster.position.z;
		
		burst.transform.position = target;
		burst.radius = radius;
		burst.onExplode = onExplode;
		burst.caster = caster.gameObject;
		
		return burst;
	}//Create
	
	public delegate void ExplodeCallback(Collider2D[] hits);
	
}//AOEBurst

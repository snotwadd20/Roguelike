using UnityEngine;
using System.Collections;

public class EnemyMove : MonoBehaviour 
{
	//Directions
	public const int UP    = 0x1;
	public const int RIGHT = 0x2;
	public const int DOWN  = 0x4;
	public const int LEFT  = 0x8;

	//State vars
	public const int WANDER = 0;
	public const int CHASE = 1;

	//Public
	public Transform targetTile = null;
	public float enemySeeDist = 6.0f;

	public bool moveIsAttack = true;

	public float damage = 10.0f;

	//Private
	private SerializedPoint destination = null;
	private bool wasOffScreen = true;

	private int state = 0;
	private int lastMoveDir = 0;

	//Consecutive turns before we go back to wander
	//private int turnsChasingBlind = 0;
	//private int maxBlindChaseTurns = 4; 

	private int ignoreRaycastLayer = 0;

	public bool isDead = false;
	// Use this for initialization
	void Start () 
	{
		destination = transform.position;
		TurnManager.RegisterCallback(gameObject, OnTurn);

		targetTile = new GameObject("Enemy Target Tile").transform;

		wasOffScreen = true;
		state = WANDER;
		//turnsChasingBlind = 0;
		ignoreRaycastLayer = LayerMask.NameToLayer("Ignore Raycast");
	}//Start

	// Update is called once per frame
	void LateUpdate () 
	{
		targetTile.position = R_Player.self.transform.position;

		//Move to the next spot
		transform.position = ((Vector3)destination) + Vector3.forward * -1;
	}//Update

	//Thing that happens ever turn (deciding to move, attack, etc)
	void OnTurn(int turnNumber)
	{
		if(isDead)
			return;

		if(state == WANDER)
			wander ();
		else if (state == CHASE)
			chase ();

		if(moveIsAttack)
		{
			//Check the destination and see if the player is there
			RaycastHit2D hit = raycastTo(destination - transform.position, 1.0f, "Player");
			if(hit.collider != null)
			{
				//If he is, cancel the move and do damage instead
				destination = transform.position;
				doAttack();
			}//if
		}//if

		//IF the monster came on screen this frame, notify the player
		if(!isOnScreen())
		{
			wasOffScreen = true;
		}//if
		else
		{
			if(wasOffScreen && Vector2.Distance(transform.position, R_Player.self.transform.position ) <= FOVSquare.seeDist - 2.5f)
			{
				ActLog.print("<color=red>A " + gameObject.name + " begins to chase you!</color>");

				wasOffScreen = false;
				if(state == WANDER)
					state = CHASE;
			}//if
		}//else
	}//OnTurn

	public void doAttack()
	{
		CameraShake.Shake(Camera.main, 0.25f, 0.4f, 1.0f, Vector2.zero) ;
		ActLog.print("<color=red>Monster attacked player for " + damage + " damage!</color>");
		PlayerHealth ph = R_Player.self.GetComponent<PlayerHealth>();
		ph.dealDamage(damage, transform.position);
	}//doAttack

	public bool isOnScreen(Camera cam = null)
	{
		if(cam == null)
			cam = Camera.main;
		
		Vector3 pos = cam.WorldToViewportPoint(transform.position);
		return (pos.x >= 0.0f && pos.x <=1.0f && pos.y >= 0.0f && pos.y <=1.0f) ;
	}//isOnScreen

	private void chase() //Has a target
	{
		//Check available directions
		int availableDirs = R_Map.self.corners[(int)transform.position.x, (int)transform.position.y];
		//Remove the one you came from

		if(lastMoveDir != 0)
			availableDirs &= ~R_Map.self.oppositeDir(lastMoveDir);

		int dirToGo = 0;

		if(R_Map.self.numLinks(availableDirs) > 1)
		{
			//If there are more than one, choose the one closest to "target"
			float bestDist = float.MaxValue;
			SerializedPoint neighbor;

			while(availableDirs > 0)
			{
				int dir = R_Map.self.randomDirFromAvailable(ref availableDirs);

				neighbor = R_Map.self.findNeighbor(transform.position, dir);

				float dist = Vector2.Distance(targetTile.position, neighbor);
				if(dist < bestDist)
				{
					bestDist = dist;
					dirToGo = dir;
				}//if
			}//while

		}//if
		else
		{
			//Only one way to go
			dirToGo = availableDirs;

		}//else

		RaycastHit2D hit = raycastTo(destination, 1, "Enemy");
		if(hit)
			dirToGo = 0;

		if(dirToGo != 0)
		{
			lastMoveDir = dirToGo;
			destination += R_Map.self.findNeighbor(0,0, dirToGo);
		}//if



	}//chase

	private void wander() //No target
	{
		int dirs = UP | DOWN | LEFT | RIGHT;
		int chosenDir = 0;
		
		while(dirs > 0)
		{
			int dir = R_Map.self.randomDirFromAvailable(ref dirs);

			if((lastMoveDir != 0 && dir == R_Map.self.oppositeDir(lastMoveDir)) || (R_Map.self.corners[(int)transform.position.x, (int)transform.position.y] & dir) == 0)
				continue;
			
			chosenDir = dir;
			lastMoveDir = chosenDir;
			break;
		}//while
		
		if(chosenDir != 0)
		{
			destination += R_Map.self.findNeighbor(0,0, chosenDir);
		}//if
	}//wander

	private bool canSeePlayer
	{
		get
		{
			if(isOnScreen() && Vector2.Distance(transform.position, R_Player.self.transform.position ) <= enemySeeDist)
			{
				RaycastHit2D hit = raycastTo ((R_Player.self.transform.position - transform.position), FOVSquare.seeDist + 1, "Player", "Default");
				return (hit.collider != null && hit.transform.tag == "Player");
			}//if
			else
				return false;
		}//get
	}//canSeePlayer

	//This is the slower version (since it has to mess with strings)
	private RaycastHit2D raycastTo(Vector3 direction, float length, params string[] layerNames)
	{
		int layerFlags = 0;//1 << LayerMask.NameToLayer("Player");
		if(layerNames.Length == 0)
			layerFlags = 1 << LayerMask.NameToLayer("Default");
		else
		{
			foreach(string lName in layerNames)
			{
				layerFlags  |= 1 << LayerMask.NameToLayer(lName);
			}//foreach
		}//else
		
		
		return raycastTo(direction, length, layerFlags);
	}//ray

	//Faster  version
	private RaycastHit2D raycastTo(Vector3 direction, float length, int layerFlags)
	{
		int oldLayer = gameObject.layer;
		gameObject.layer = ignoreRaycastLayer;
		
		RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, length, layerFlags);

		if(hit.collider != null)
			Debug.DrawRay(transform.position, direction.normalized*length, Color.red, 2.0f);
		else
			Debug.DrawRay(transform.position, direction.normalized*length, Color.white, 2.0f);

		gameObject.layer = oldLayer;
		return hit;
	}//ray

}//EnemyMove
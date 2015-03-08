using UnityEngine;
using System.Collections;

public class EnemyMove : MonoBehaviour 
{
	public const int UP    = 0x1;
	public const int RIGHT = 0x2;
	public const int DOWN  = 0x4;
	public const int LEFT  = 0x8;

	public Transform targetTile = null;

	private SerializedPoint destination = null;
	private int lastMoveDir = 0;

	// Use this for initialization
	void Start () 
	{
		destination = transform.position;
		TurnManager.RegisterCallback(gameObject, OnTurn);

		targetTile = new GameObject("Enemy Target Tile").transform;
	}//Start
	
	// Update is called once per frame
	void Update () 
	{
		targetTile.position = R_Player.self.transform.position;

		//Move to the next spot
		transform.position = destination;
		if(Input.GetKeyDown(KeyCode.Space))
		{
			TurnManager.NextTurn();
		}//if
	}//Update

	//Thing that happens ever turn (deciding to move, attack, etc)
	void OnTurn(int turnNumber)
	{
		chase();
	}//OnTurn

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
			print (availableDirs);
			SerializedPoint neighbor;

			while(availableDirs > 0)
			{
				int dir = R_Map.self.randomDirFromAvailable(ref availableDirs);
				Debug.DrawRay(transform.position, R_Map.self.findNeighbor(0,0, dir), Color.green, 2.0f);

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
			Debug.DrawRay(transform.position, R_Map.self.findNeighbor(0,0, dirToGo), Color.cyan, 2.0f);

		}//else

		if(dirToGo != 0)
		{
			lastMoveDir = dirToGo;
			destination += R_Map.self.findNeighbor(0,0, dirToGo);
			Debug.DrawRay(transform.position, R_Map.self.findNeighbor(0,0, dirToGo), Color.red, 3.0f);
		}//if


	}//chase

	private void wander() //No target
	{
		int dirs = UP | DOWN | LEFT | RIGHT;
		int chosenDir = 0;
		
		while(dirs > 0)
		{
			int dir = R_Map.self.randomDirFromAvailable(ref dirs);
			Debug.DrawRay(transform.position, R_Map.self.findNeighbor(0,0, dir), Color.green, 2.0f);
			
			if(dir == R_Map.self.oppositeDir(lastMoveDir) || (R_Map.self.corners[(int)transform.position.x, (int)transform.position.y] & dir) == 0)
				continue;
			
			chosenDir = dir;
			lastMoveDir = chosenDir;
			break;
		}//while
		
		if(chosenDir != 0)
		{
			destination += R_Map.self.findNeighbor(0,0, chosenDir);
			Debug.DrawRay(transform.position, R_Map.self.findNeighbor(0,0, chosenDir), Color.red, 3.0f);
		}//if
	}//wander

}//EnemyMove
using UnityEngine;
using System.Collections;

public class EnemyMove : MonoBehaviour 
{
	SerializedPoint destination = null;

	// Use this for initialization
	void Start () 
	{
		destination = transform.position;
		TurnManager.RegisterCallback(gameObject, OnTurn);
	}//Start
	
	// Update is called once per frame
	void Update () 
	{
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
		print(destination + " -> " + (destination + Vector3.up) + " TN#: " + TurnManager.TurnNumber);
		destination += Vector3.up;
	}//OnTurn

}//EnemyMove
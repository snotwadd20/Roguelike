using UnityEngine;
using System.Collections;

public class Picker : MonoBehaviour 
{
	public Container inventory = null;

	private bool pickup = false;
	// Use this for initialization
	void Start () 
	{
		if(inventory == null)
		{
			inventory = gameObject.GetComponent<Container>();
		}//if
	}//Start

	void OnTriggerStay2D(Collider2D coll)
	{
		if(pickup && !RoguelikeControls.isPaused)
		{
			Pickable obj = coll.gameObject.GetComponent<Pickable>();
			if(obj != null && inventory != null)
			{
				obj.pick(ref inventory);
			}//if
			pickup = false;
		}//if
		else if(RoguelikeControls.isPaused)
		{
			pickup = false;
		}//else if
	}//if
	
	// Update is called once per frame
	void Update () 
	{
		if(pickup)
		{
			ActLog.print("There is nothing on the ground for you to [G]et.");
			pickup = false;
		}//if

		pickup = Input.GetKeyUp(KeyCode.G);
	}//Update
}//Picker

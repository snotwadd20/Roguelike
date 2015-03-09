using UnityEngine;
using System.Collections;

public class Picker : MonoBehaviour 
{
	public Container inventory = null;

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
		if(Input.GetKeyDown(KeyCode.P))
		{
			Pickable obj = coll.gameObject.GetComponent<Pickable>();
			if(obj != null && inventory != null)
			{
				obj.pick(ref inventory);
			}//if
		}//if
	}//if
	
	// Update is called once per frame
	void Update () 
	{
	
	}
}

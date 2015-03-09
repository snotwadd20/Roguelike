using UnityEngine;
using System.Collections;

public class Pickable : MonoBehaviour 
{
	//Metadata
	public string name = "Item";
	public string type = "Nada";
	public int count = 1;

	InventoryClickCallback callback = null;
	// Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}

	public void RegisterCallback(InventoryClickCallback callback)
	{
		this.callback = callback;
	}//

	public delegate void InventoryClickCallback();
}//Pickable

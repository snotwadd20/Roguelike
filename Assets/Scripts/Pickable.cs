using UnityEngine;
using System.Collections;

public class Pickable : MonoBehaviour 
{
	//Metadata
	public new string name = "Item";
	public string type = "Nada";
	public int count = 1;

	private InventoryClickCallback _callback = null;

	public Container holdingContainer = null;

	public InventoryClickCallback callback
	{
		get
		{
			return _callback;
		}//get
	}//callback

	// Use this for initialization
	void Start () 
	{
		BoxCollider2D bc2d = gameObject.AddComponent<BoxCollider2D>();
		bc2d.isTrigger = true;

	}//Start
	
	public void OnTriggerEnter2D()
	{
		ActLog.print("Press G to get the " + name);
	}//OnTriggerEnter2D

	public void pick(ref Container container)
	{
		ActLog.print ("You picked up the " + name + ".");
		container.Add(this);
		holdingContainer = container;
	}//pick

	public void use()
	{
		if(_callback != null)
			_callback(this);
	}//use

	public void RegisterCallback(InventoryClickCallback callback)
	{
		this._callback = callback;
	}//

	public delegate void InventoryClickCallback(Pickable pickable);
}//Pickable

using UnityEngine;
using System.Collections;

public class Pickable : MonoBehaviour 
{
	//Metadata
	public new string name = "Item";
	public string type = "Nada";
	public int count = 1;

	public InventoryClickCallback callback = null;

	// Use this for initialization
	void Start () 
	{
		CircleCollider2D cc2d = gameObject.AddComponent<CircleCollider2D>();
		cc2d.isTrigger = true;
		cc2d.radius *= 1.25f;
	}//Start
	
	public void OnTriggerEnter2D()
	{
		ActLog.print("press P to pick up");
	}//OnTriggerEnter2D

	public void pick(ref Container container)
	{
		ActLog.print ("You picked up the " + name + ".");
		container.Add(this);
	}//pick

	public void use()
	{
		callback();
	}//use

	public void RegisterCallback(InventoryClickCallback callback)
	{
		this.callback = callback;
	}//

	public delegate void InventoryClickCallback();
}//Pickable

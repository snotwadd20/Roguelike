using UnityEngine;
using System.Collections;

public class TreasureManager : MonoBehaviour 
{
	public static GameObject chestPrefab = null;

	// Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}

	public static GameObject Spawn(Vector3 pos)
	{
		if(chestPrefab == null)
			chestPrefab = Resources.Load<GameObject>("Objects/chest");

		GameObject go = Instantiate<GameObject>(chestPrefab);
		go.transform.position = pos + Vector3.forward * -1;
		go.SetActive(true);
		return go;
	}//Spawn
}//TreasureManager
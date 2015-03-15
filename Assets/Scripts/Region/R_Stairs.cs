using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class R_Stairs : MonoBehaviour 
{
	public bool stairsGoDown = true;

	private bool stairsKeyPressed = false;

	private static GameObject downPrefab = null;
	private static GameObject upPrefab = null;

	private static List<GameObject> allStairs = null;

	void Start()
	{
		if(allStairs == null)
			allStairs = new List<GameObject>();

		Collider2D coll = GetComponent<Collider2D>();
		if(!coll)
			coll = gameObject.AddComponent<BoxCollider2D>();

		coll.isTrigger = true;

		if((R_Map.Level == 0 && !stairsGoDown) || (R_Map.Level == R_Map.MAX_LEVELS-1 && stairsGoDown))
			gameObject.SetActive(false);

		gameObject.layer = LayerMask.NameToLayer("Stairs");
	}//OnEnable
	void OnTriggerEnter2D(Collider2D coll)
	{
		ActLog.print("Press S to use Stairs " + (stairsGoDown ? "Down." : "Up."));
	}//OnTriggerEnter2D

	void OnTriggerStay2D(Collider2D coll)
	{
		if(stairsKeyPressed)
		{
			if(stairsGoDown)
				R_Map.loadNextLevel();
			else if(!stairsGoDown)
				R_Map.loadPrevLevel();
		}//if
	}//if

	// Update is called once per frame
	void Update () 
	{
		if((R_Map.Level == 0 && !stairsGoDown) || (R_Map.Level == R_Map.MAX_LEVELS-1 && stairsGoDown))
			gameObject.SetActive(false);
		else
			stairsKeyPressed = (Input.GetKeyDown(KeyCode.S) && !RoguelikeControls.isPaused);
	}//Update

	public static void CleanUp()
	{
		for(int i=0; i < allStairs.Count; i++)
		{
			Destroy(allStairs[i]);
		}//for
		allStairs = null;
	}//CleanUp

	public static R_Stairs Create(Vector3 position, bool goesDown)
	{
		if(upPrefab == null || downPrefab == null)
		{
			upPrefab = Resources.Load<GameObject>("Objects/stairsUp");
			downPrefab = Resources.Load<GameObject>("Objects/stairsDown");
		}//if

		if(allStairs == null)
			allStairs = new List<GameObject>();

		R_Stairs stairs = ((GameObject)Instantiate(goesDown ? downPrefab : upPrefab)).GetComponent<R_Stairs>();

		stairs.transform.position = position + Vector3.forward * -1;
		stairs.stairsGoDown = goesDown;
		stairs.transform.SetParent(R_Map.self.transform);
		stairs.gameObject.SetActive(true);

		allStairs.Add(stairs.gameObject);

		return stairs;
	}//Create
}//R_Stairs

using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class StatTotalDisplay : MonoBehaviour 
{
	public int column = 0;
	public CharacterSheet stats = null;

	private Text textObj = null;
	// Use this for initialization
	void Start () 
	{
		if(textObj == null)
			textObj = GetComponent<Text>();

		if(textObj == null)
		{
			Debug.LogError("StatTotalDisplay.cs: Script must be put on a Text object");
			Destroy(this);
		}//if

	}//Start
	
	// Update is called once per frame
	void Update () 
	{
		if(stats == null)
			return;

		textObj.text = "" + stats.getStat(column);
	}//Update
}//StatTotalDisplay
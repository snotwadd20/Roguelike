using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UpdateLivesUI : MonoBehaviour 
{
	public Text textObj = null;
	public PlayerHealth ph = null;

	
	// Update is called once per frame
	void Update () 
	{
		if(textObj == null)
			textObj = gameObject.GetComponent<Text>();
		
		if(ph == null)
			ph = R_Player.self.GetComponent<PlayerHealth>();

		if(textObj != null)
		{
			textObj.text = "Lives: " + ph.lives;
		}//if
	}
}

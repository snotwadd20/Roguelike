using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LevelUpUI : MonoBehaviour 
{
	public Text header = null;
	public Text youGot = null;

	public static LevelUpUI self = null;

	public void Close()
	{
		gameObject.SetActive(false);
	}//Close

	private static bool oneanddone = false;

	void OnEnable()
	{
		if(self == null)
			self = this;

		if(!oneanddone)
		{
			gameObject.SetActive(false);
			oneanddone = true;
		}//if
		else
		{
			RoguelikeControls.Pause(false);
		}//else

	}//OnEnable

	void OnDisable()
	{
		RoguelikeControls.UnPause();
	}//OnEnable
}

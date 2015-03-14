using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public delegate void YesNoFunction();

public class YesNoUI : MonoBehaviour 
{
	public Text header = null;

	public Button yesButton = null;
	public Text yesButtonText = null;
	public Button noButton = null;
	public Text noButtonText = null;

	public YesNoFunction yesCallback = null;
	public YesNoFunction noCallback = null;

	public static YesNoUI self = null;

	public void DoYes()
	{
		if(yesCallback != null)
			yesCallback();

		Close();
	}//DoCallback

	public void DoNo()
	{
		if(noCallback != null)
			noCallback();

		Close();
	}//DoCallback

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
			Time.timeScale = 0;
		}//else
	}//OnEnable

	void OnDisable()
	{
		Time.timeScale = 1;
	}//OnEnable

	public void RegisterCallbacks(YesNoFunction yes, YesNoFunction no)
	{
		yesCallback = yes;
		noCallback = no;
	}//RegisterCallbacks
}

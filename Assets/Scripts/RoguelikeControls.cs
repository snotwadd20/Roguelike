using UnityEngine;
using System.Collections;

public class RoguelikeControls : MonoBehaviour 
{
	private string alphabet = "abcdefhijklmnopqrtuvwxyz1234567890"; //minus p (pick) and (s) stairs

	public EquipUI equipMenu = null;
	public PauseMap pauseMap = null;

	public static bool isPaused = false;
	private static float storedTimeScale = 1.0f;

	public static RoguelikeControls self = null;
	void Awake()
	{
		if(self == null)
			self = this;
	}//Awake
	void executeControl(string key)
	{
		switch(key)
		{
			case "i":
				Container inventory = gameObject.GetComponent<Container>();
				inventory.showUI();
				break;
			case "c":
				equipMenu.gameObject.SetActive(true);
				break;
		}//switch
	}//executeControl
	
	// Update is called once per frame
	void Update () 
	{
		/*if(Input.GetKeyUp(KeyCode.Tab) || (isPaused && Input.GetKeyUp(KeyCode.Escape)))
		{
			isPaused = !isPaused;
			
			if(isPaused)
			{
				Pause();
				
			}//if
			else
			{
				UnPause();
			}//else
			
			pauseMap.gameObject.SetActive(isPaused);
		}//if*/


		if(isPaused)
			return;

		if(Input.GetKeyUp(KeyCode.Escape))
		{
			SceneLoader.self.Load("QuitGame");
		}//if

		foreach(char letter in alphabet)
		{
			if(Input.GetKeyDown(letter+""))
			{
				executeControl(letter + "");
			}//if
		}//foreach


	}//Update

	public static void Pause(bool setTimeScale = true)
	{
		storedTimeScale = Time.timeScale;

		if(setTimeScale)
			Time.timeScale = 0.0f;
	}//doPause
	
	public static void UnPause()
	{
		Time.timeScale = storedTimeScale;
	}//doUnPause
}//RoguelikeControls
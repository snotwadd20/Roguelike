using UnityEngine;
using System.Collections;

public class RoguelikeControls : MonoBehaviour 
{
	private string alphabet = "abcdefhijklmnopqrtuvwxyz1234567890"; //minus p (pick) and (s) stairs

	public EquipUI equipMenu = null;
	public PauseMap pauseMap = null;

	public static bool isPaused = false;
	private float storedTimeScale = 1.0f;

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
		if(Input.GetKeyUp(KeyCode.Tab) || (isPaused && Input.GetKeyUp(KeyCode.Escape)))
		{
			isPaused = !isPaused;
			
			if(isPaused)
			{
				doPause();
				
			}//if
			else
			{
				doUnPause();
			}//else
			
			pauseMap.gameObject.SetActive(isPaused);
		}//if


		if(Time.timeScale == 0)
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

	private void doPause()
	{
		storedTimeScale = Time.timeScale;
		Time.timeScale = 0.0f;
	}//doPause
	
	private void doUnPause()
	{
		Time.timeScale = storedTimeScale;
	}//doUnPause
}//RoguelikeControls
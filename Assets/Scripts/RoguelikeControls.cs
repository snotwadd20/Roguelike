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
				if(inventory.currentGUI != null && inventory.currentGUI.isActiveAndEnabled)
				{
					inventory.hideUI();
				}
				else
				{
					inventory.showUI();
				}//else
				break;
			case "c":
				if(equipMenu.gameObject.activeSelf)	
					equipMenu.Close();
				else
					equipMenu.gameObject.SetActive(true);

				break;
		}//switch
	}//executeControl
	
	// Update is called once per frame
	void Update () 
	{
		if(!isPaused && Input.GetKeyDown(KeyCode.Escape))
		{
			SceneLoader.self.Load("QuitGame");
		}//if

		foreach(char letter in alphabet)
		{
			if(Input.GetKeyUp(letter+""))
			{
				executeControl(letter + "");
			}//if
		}//foreach


	}//Update
	public static int NumPauses = 0;
	public static void Pause(bool setTimeScale = true)
	{
		storedTimeScale = Time.timeScale;

		if(setTimeScale)
			Time.timeScale = 0.0f;

		isPaused = true;
		NumPauses++;
	}//doPause
	
	public static void UnPause()
	{
		NumPauses--;
		if(NumPauses <= 0)
		{
			Time.timeScale = storedTimeScale;
			isPaused = false;
		}//if

	}//doUnPause
}//RoguelikeControls
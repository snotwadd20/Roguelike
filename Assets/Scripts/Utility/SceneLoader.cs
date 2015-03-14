using UnityEngine;
using System;
using System.IO;
using System.Collections;
using System.Runtime.Serialization.Formatters.Binary;

public class SceneLoader : MonoBehaviour 
{
    //public UnityEngine.Object sceneToLoad = null;
    public string sceneNameToLoad = "";

    public LoadType loadCondition = LoadType.manual;
    public GameObject lynchpin;

	private static SceneLoader _self = null;
	public static SceneLoader self
	{
		get
		{
			if(_self == null)
			{
				_self = new GameObject("SceneLoader").AddComponent<SceneLoader>();
			}//if

			return _self;
		}//get
	}//self


	// Use this for initialization
	void Start () 
    {
	    if(loadCondition != LoadType.onStart)
            return;

        Load();
	}
	
	// Update is called once per frame
	void Awake () 
    {
        if(loadCondition != LoadType.onAwake)
            return;
        Load();
	}//Awake

    void OnEnable () 
    {
        if(loadCondition != LoadType.onEnable)
            return;

        Load();
    }//OnEnable

    void OnTriggerEnter2D(Collider2D coll)
    {
        if(loadCondition != LoadType.onStart)
            return;

        if(coll.gameObject == lynchpin)
            Load();
    }//OnTriggerEnter2D

    public void Load()
    {
		Load(sceneNameToLoad);
    }//Load

	public void Quit()
	{
		YesNoUI.self.gameObject.SetActive(false);
		Application.Quit();
		print ("QUIT");
	}//Quit

	public void Cancel()
	{
		YesNoUI.self.gameObject.SetActive(false);
	}//cancel

	public void Load(string toLoad)
	{
		if(toLoad == "QuitGame")
		{
			YesNoUI.self.header.text = "QUIT GAME?";
			YesNoUI.self.yesButtonText.text = "Yes!";
			YesNoUI.self.noButtonText.text = "HELL NO!";
			YesNoUI.self.RegisterCallbacks(Quit, Cancel);

			YesNoUI.self.gameObject.SetActive(true);
		}
		else
		{
			Application.LoadLevel (toLoad);
		}//else
	}//Load
    
    public enum LoadType {onEnable, onAwake, onStart, onTriggerVolume, manual};
}

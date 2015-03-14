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

	private SceneLoader _self = null;
	public SceneLoader self
	{
		get
		{
			if(_self == null)
			{
				_self = new GameObject("SceneLoader").AddComponent<SceneLoader>();
				DontDestroyOnLoad(_self);
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
        Application.LoadLevel (sceneNameToLoad);
    }//Load

    public enum LoadType {onEnable, onAwake, onStart, onTriggerVolume, manual};
}

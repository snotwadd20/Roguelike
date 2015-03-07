using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HiveMind : MonoBehaviour 
{
    public delegate void HiveMindCallback(int beatNum);

    public float beatsPerSecond = 2;
    public int beatsInMeasure = 4;

    private float timeBetweenBeats = 0;

    private float timer = 1.0f;

    private List<Callback> callbacks = null;

    private int _beatNumber = 0;

    public void sendBeat()
    {
        for(int i=0; i < callbacks.Count;i++)
        {
            //Get rid of dead entries
            if(callbacks[i].obj == null)
            {
                callbacks.RemoveAt(i);
                i--;
                continue;
            }//if

            callbacks[i].callBack(beatNumber);
        }//for
    }//sendBeat

    public void registerCallback(GameObject obj, HiveMindCallback callBack)
    {
        callbacks.Add(new Callback(obj, callBack));
    }//RegisterCallback

    public int beatNumber
    {
        get
        {
            return _beatNumber;
        }//get
    }//BeatNumber

    public float beatTime
    {
        get
        {
            return timeBetweenBeats;
        }//get
    }//beatTime

	// Use this for initialization
	void OnEnable () 
    {
        if(callbacks == null)
        {
            callbacks = new List<Callback>();
        }//if

        ChangeBeat(beatsPerSecond, beatsInMeasure);
	}//Start

    public void ChangeBeat(float bps, int bim)
    {
        beatsPerSecond = bps;
        beatsInMeasure = bim;

        timeBetweenBeats = 1/bps;
        timer = timeBetweenBeats;

        _beatNumber = 0;
    }//ChangeBeat
	
	// Update is called once per frame
	void Update () 
    {
        timer -= Time.deltaTime;
        if(timer <= 0)
        {
            float slop = timer;
            timer = timeBetweenBeats + slop;
            sendBeat();
            _beatNumber = (_beatNumber + 1) % beatsInMeasure;
            timer = timeBetweenBeats;
        }//if
	}



    private class Callback
    {
        public GameObject obj = null;
        public HiveMindCallback callBack = null;
        public Callback(GameObject obj, HiveMindCallback function)
        {
            this.obj = obj;
            this.callBack = function;
        }//constructor
    }//Callback
}
//HiveMind
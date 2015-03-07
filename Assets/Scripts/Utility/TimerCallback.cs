using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System;

public class TimerCallback : MonoBehaviour 
{
    public delegate void TimeUpFunction();

    private bool started = false;
    public float startingTime = 0.0f;
    private float myTimer = 0.0f;

    private TimeUpFunction callback;
    public static GameObject allTimers = null;
	private GameObject parent;
    private bool standalone = false;

    public bool updateWhilePaused = false;

    public float percentComplete
    {
        get 
        {
            return 1.0f - (myTimer / startingTime);
        }//get
    }//percentComplete

    public float maxTime
    {
        get
        {
            return startingTime;
        }//get
    }//maxTime

    public float timeLeft
    {
        get 
        {
            return myTimer;
        }//get
    }//timeLeft

	// Update is called once per frame
    DateTime finishTime;

	void Update () 
    {

        if(started)
        {
            if(myTimer > 0)
            {
                myTimer -= Time.deltaTime;
            }//if

            if(myTimer <= 0 || (updateWhilePaused && DateTime.Now.CompareTo(finishTime) > -1))
            {
                done();
            }//if
        } //if
	}//Update

    //Time is counted in seconds
    public void startTimer(float time, TimeUpFunction timeUpFunction )
    {
		myTimer = time;
        finishTime = DateTime.Now.AddSeconds(time);
        startingTime = time;
        callback = timeUpFunction;
        started = true;
    }//startTimer

    void done()
    {
        callback();

        if(standalone)
        {
            timerPool.Add(this);
            gameObject.SetActive(false);
        }//if
        else
        {
            Destroy(this);
        }//else
    }//done

    public static List<TimerCallback> timerPool = null;

    void OnDestroy()
    {
        if(timerPool == null)
            return;

        foreach(TimerCallback timer in timerPool)
        {
            Destroy(timer.gameObject);
        }//foreach
        timerPool = null;
    }//OnDestroy

    public static TimerCallback createTimer(float time, TimeUpFunction timeUpFunction, string name = "", bool updateWhilePaused = false)
    {
        if(allTimers == null)
            allTimers = new GameObject("Timers");

        if(name == "")
            name = "Timer[" + time + "]";

        if(timerPool == null)
        {
            timerPool = new List<TimerCallback>();
        }//if

        TimerCallback tc = null;

        if(timerPool.Count > 0)
        {
            tc = timerPool[0];
            timerPool.RemoveAt(0);
            tc.gameObject.SetActive(true);
        }//if

        GameObject timer = null;
        if(tc == null)
        {
            timer = new GameObject(name);
            timer.transform.parent = allTimers.transform;
            tc = timer.AddComponent<TimerCallback>();
        }//if
        else
        {
            timer = tc.gameObject;
            tc.gameObject.name = name;
        }//else

        tc.startTimer(time, timeUpFunction);
        tc.standalone = true;
        tc.updateWhilePaused = updateWhilePaused;

        return tc;
    }//createTimer
}//TimerCallback

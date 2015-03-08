using UnityEngine;
using System;
using System.Collections;

public class Scaler : MonoBehaviour 
{
    public string id = "";
    public float speed = 1.0f;
    public Vector3 scaleFactor = Vector3.one*2;
    public float waitTime = 0.5f;
    public ScaleType type = ScaleType.PingPong;
    
    public MonoBehaviour[] triggerWhenDone = null;
    public bool triggerMessage = true;
    
    private bool isWaiting = false;
    
    private float timer = 0;
    private Vector3 orgScale = Vector3.one * int.MinValue;
    private Vector3 destScale;
    
    private bool isReturning = false;
    private bool doPop = false;

    void OnEnable()
    {
        orgScale = transform.localScale;

        destScale = orgScale + scaleFactor;
        
        isWaiting = false;
        isReturning = false;
        doPop = false;
        timer = 0;
    }//OnEnable
    
    // Update is called once per frame
    void Update () 
    {
        timer += Time.deltaTime;
        
        if(isWaiting)
        {
            if(timer >= waitTime)
            {
                isWaiting = false;
                
                timer = 0;
            }//if
        }
        else
        {
            if(timer < speed && !doPop)
            {
                float delta = Mathf.Lerp(0,1,timer/speed);
                
                Vector3 moveVec = destScale - orgScale;
                transform.localScale = orgScale +  moveVec * delta;
            }//if
            else
            {
                transform.localScale = destScale;
                isWaiting = true;
                doTriggers();
                
                handleLooping();
                timer = 0;
            }//else
        }//if
    }//update
    
    public void handleLooping()
    {
        Vector3 temp;
        switch(type)
        {
            case ScaleType.Stay:
                enabled = false;
                break;
            case ScaleType.PingPong:
                temp = destScale;
                destScale = orgScale;
                orgScale = temp;
                break;
            case ScaleType.Pop:
                if(doPop == true)
                {
                    transform.localScale = orgScale;
                    enabled = false;
                    type = ScaleType.Stay;
                }//if
                else
                    doPop = true; 
                
                break;
            case ScaleType.Return:
                if(!isReturning)
                {
                    isReturning = true;
                    temp = destScale;
                    destScale = orgScale;
                    orgScale = temp;
                    isReturning = !isReturning;
                    type = ScaleType.Stay;
                    isWaiting = true;
                }//if
                break;
        }//switch
    }//handleLooping
    
    public void doTriggers()
    {
        if(triggerWhenDone != null)
        {
            for(int i=0; i < triggerWhenDone.Length; i++)
            {
                if(triggerWhenDone[i])
                    triggerWhenDone[i].enabled = triggerMessage;
            }//for
        }//if
    }//doTriggers
    
    public enum ScaleType {Stay, Return, Pop, PingPong};
}//Scaler

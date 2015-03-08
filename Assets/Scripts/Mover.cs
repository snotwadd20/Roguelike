using UnityEngine;
using System;
using System.Collections;

public class Mover : MonoBehaviour 
{
    public string id = "";
    public float speed = 1.0f;
    public Transform moveToPos = null;
    public Vector3 dirToMove = Vector3.one;
    public float waitTime = 0.5f;
    public MoveType type = MoveType.Stay;

    public MonoBehaviour[] triggerWhenDone = null;
    public bool triggerMessage = true;

    public bool doLateUpdate = false;

    private bool isWaiting = false;

    private float timer = 0;
    private Vector3 oldPos = Vector3.one * int.MinValue;
    private Vector3 destPos;
    private bool doPop = false;

    private bool isReturning = false;
   
    private Vector3 firstPos;
    void OnEnable()
    {
        firstPos = transform.position;
        oldPos = transform.position;

        if(moveToPos != null)
            destPos = moveToPos.position;
        else
            destPos = oldPos + dirToMove;

        isWaiting = false;
        isReturning = false;
        doPop = false;
        timer = 0;
    }//OnEnable

    void OnDisable()
    {
        transform.position = firstPos;
    }//OnDisable

    void Movement()
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
                
                Vector3 moveVec = destPos - oldPos;
                transform.position = oldPos +  moveVec * delta;
                /*if(Vector2.Distance(transform.position, destPos) <= 0.05)
                    timer = speed;*/
            }//if
            else
            {
                transform.position = destPos;
                isWaiting = true;
                doTriggers();
                
                handleLooping();
                timer = 0;
            }//else
        }//if
    }//movement

    // Update is called once per frame
    void Update () 
    {
        if(!doLateUpdate)
            Movement();
    }//update

    void LateUpdate () 
    {
        if(doLateUpdate)
            Movement();
    }//update

    public void handleLooping()
    {
        Vector3 temp;
        switch(type)
        {
            case MoveType.Stay:
                enabled = false;
                break;
            case MoveType.PingPong:
                temp = destPos;
                destPos = oldPos;
                oldPos = temp;
                //isReturning = !isReturning;
                break;
            case MoveType.Pop:
                if(doPop == true)
                {
                    transform.position = oldPos;
                    enabled = false;
                    type = MoveType.Stay;
                }//if
                else
                    doPop = true; 

                break;
            case MoveType.Return:
                if(!isReturning)
                {
                    isReturning = true;
                    temp = destPos;
                    destPos = oldPos;
                    oldPos = temp;
                    isReturning = !isReturning;
                    type = MoveType.Stay;
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

    public enum MoveType {Stay, Return, Pop, PingPong};
}//Mover

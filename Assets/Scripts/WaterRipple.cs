using UnityEngine;
using System.Collections;

public class WaterRipple : MonoBehaviour 
{
    public static GameObject rippleEffect = null;

    public Color color = Color.black; //Black == not set

    private ParticleSystem[] effects = null;

    private int currentEffect = 0;
    private float timer = 0;
	// Use this for initialization
	void Start () 
    {
        if(rippleEffect == null)
            rippleEffect = (GameObject)Resources.Load("Effects/Ripples");

        if(color != Color.black)
        {
            ParticleSystem ps = rippleEffect.GetComponent<ParticleSystem>();
            ps.startColor = color;
        }//if
        else
            color = Color.white;

	    effects = new ParticleSystem[4];
        for(int i=0; i < effects.Length; i++)
        {
            effects[i] = ((GameObject)GameObject.Instantiate(rippleEffect)).GetComponent<ParticleSystem>();
            effects[i].Stop();
            effects[i].transform.parent = transform;
        }//for
        gameObject.layer = LayerMask.NameToLayer("Player Triggers");
	}//Start
	
    void Update()
    {
        timer += Time.deltaTime;
    }//Update

	// Update is called once per frame
	void OnTriggerStay2D (Collider2D coll) 
    {
        if(timer > 0.25f)
        {
            effects[currentEffect].transform.position = coll.gameObject.transform.position;
            effects[currentEffect].Play();
            currentEffect = (currentEffect + 1) % effects.Length;
            timer = 0;
        }//if
    }//OnTriggerEnter2D
}//WaterRipple
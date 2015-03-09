using UnityEngine;
using System.Collections;

public class EnemyHealth : MonoBehaviour 
{
    public float startingHealth = 100;
    public float currentHealth;

    private bool isDead = false;
    private bool takingDamage = false;
    private int flickerTracker = 0;

    private SpriteRenderer sr = null;
    private MeshRenderer mr = null;

    private Color startingMeshColor;
    private Color startingSpriteColor;

    public Color damageColor = Color.red;

    public bool neverKilled = true;

    private BoxCollider2D hitbox = null;

    // DamagePlayerOnTouch dpot = null;

    private AudioClip deathSound = null;

    public int level = 1;

    void OnEnable () 
    {
        if(deathSound == null)
            deathSound = Resources.Load<AudioClip>("Audio/Enemies/KillEnemy");

        //print (deathSound);
        currentHealth = startingHealth;
        isDead = false;
        takingDamage = false;

        sr = gameObject.GetComponent<SpriteRenderer>();
        mr = gameObject.GetComponent<MeshRenderer>();

        if(sr)
            startingSpriteColor = sr.color;

        if(mr)
            startingMeshColor = mr.material.color;

        if(hitbox == null)
        {
            hitbox = gameObject.GetComponent<BoxCollider2D>();

            if(!hitbox)
                hitbox = gameObject.AddComponent<BoxCollider2D>();
        }//if

        hitbox.isTrigger = false;

    }//OnEnable

    public void setStartingHealth(float startHealth)
    {
        startingHealth = startHealth;
        currentHealth = startHealth;
    }//setStartingHealth

    public float health
    {
        get
        {
            return currentHealth;
        }//get
    }//health

    public void dealDamage(float damage)
    {
        if(isDead)
            return;

        currentHealth -= damage;
        doHitColor();
		PopupText.Create("-" + damage +  " HP" , transform.position + Vector3.up * 0.75f, Color.blue);

        if(currentHealth <= 0)
        {
            die();
        }//if

    }//dealDamage

    private void doHitColor()
    {
        if(sr)
        {
            sr.color = damageColor;
            sr.material.color = damageColor;
        }//if

        if(mr)
        {
            mr.material.color = damageColor;
        }//if

        takingDamage = true;
        TimerCallback.createTimer(0.1f, unColor, "Sprite colorizer timer");
    }//hitColor

    private void unColor()
    {
        if(sr)
        {
            sr.color = startingSpriteColor;
            sr.material.color = startingSpriteColor;
        }
        if(mr)
        {
            mr.material.color = startingMeshColor;
        }
        takingDamage = false;
    }//unColor

    private void die()
    {
        isDead = true;
        neverKilled = false;
        //Turn the collisions into triggers so bullets won't collide after this
        //but if we want we can still check for things going through this area
        foreach(Collider2D c in GetComponents<Collider2D> ()) 
        {
            c.isTrigger = true;
        }//foreach

        Destroy(this.gameObject, 1.0f); //Destroy the enemy after 2 seconds
    }//die

    private void flicker(bool doFlicker = true, int interval = 10)
    {
        if(doFlicker)
        {
            if(sr)
            {
                sr.enabled = flickerTracker % interval < interval/2;
            }//if
            if(mr)
            {
                mr.enabled = flickerTracker % interval < interval/2;
            }//if
            flickerTracker++;
        }
        else
        {
            if(sr)
            {
                sr.enabled = true;
            }//if
            if(mr)
            {
                mr.enabled = true;
            }//if
        }//else
    }//flicker



	//Update is called once per frame
	void Update () 
    {
        if(isDead)
        {
            flicker(true);
        }//if
        else
        {
            flicker(takingDamage, 6);
        }//else
	
    }//Update
}//EnemyHealth

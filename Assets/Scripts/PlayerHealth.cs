﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerHealth : MonoBehaviour 
{
    public float max_Health = 100;
    public float currentHealth;
    
    private bool isDead = false;
    private bool takingDamage = false;
    private int flickerTracker = 0;
    
    private SpriteRenderer sr = null;
    private MeshRenderer mr = null;
    
    private Color startingMeshColor;
    private Color startingSpriteColor;
    
    public Color damageColor = Color.red;

    public Slider healthBar = null;
    public Text hpText = null;
    // Use this for initialization

    private bool isInvincible = false;

	public int lives = 1;

	public CharacterSheet stats = null;

    //private PlayerMovement pm = null;
    void Awake () 
    {
		currentHealth = max_Health;
		//currentHealth = GameManager.self.maxPlayerHealth;

		if(sr)
			startingSpriteColor = sr.color;
	
		if(mr)
			startingMeshColor = mr.material.color;

		if(stats == null)
			stats = GetComponent<CharacterSheet>();
	}//Awake

    public void makeInvincible(bool doIt = false)
    {
        isInvincible = doIt;
    }//makeInvincible

    public float maxHealth
    {
        get
        {
            return max_Health;
        }//get
    }//maxHealth
    
    public float health
    {
        get
        {
            return currentHealth;
        }//get
    }//health

    public void refillAllHealth()
    {
        currentHealth = maxHealth;
    }//refillAllHealth

	public void refillSomeHealth(float healingAmount)
	{
		currentHealth = Mathf.Min(maxHealth, currentHealth + healingAmount);
	}//refillAllHealth
    
    public void dealDamage(float damage, Vector2 damageLocation)
    {
        if(isDead || isInvincible)
            return;

        currentHealth -= adjustDamage(damage);
		PopupText.Create("-" + damage +  " HP" , transform.position + Vector3.up * 0.75f, Color.red);

        knockBack((Vector2)transform.position - damageLocation, 2000);
        doHitColor();

        if(currentHealth <= 0)
        {
            lives--;
            if(lives <= 0)
                die ();
            else
            {
                useExtraLife();
                makeInvincible(true);
                TimerCallback.createTimer(3.0f,makeHittable , "Player invincible timer");
            }//else
        }//if
    }//dealDamage

	private float adjustDamage(float baseDamage)
	{
		if(stats.Defense > 0)
		{
			float DR = stats.Defense*3;
			PopupText.Create(DR + " DR. [Def " + stats.Defense + "]", transform.position + Vector3.up * 0.45f, Color.green);
			ActLog.print("<color=lightblue> [Def " + stats.Defense + "]</color>: Prevented " + DR + " damage!");
		}//if

		return Mathf.Max(Mathf.CeilToInt(baseDamage - stats.Defense*3), 1*R_Map.self.mapLevel);
	}//

    private void makeHittable()
    {
        makeInvincible(false);
    }//makeHittable

    public void useExtraLife()
    {
        //Spawn a "1up" message over player
		PopupText.Create("-1 Life!" , transform.position + Vector3.forward * -3 + Vector3.up * GetComponent<Collider2D>().bounds.size.y/3, Color.black);

		TimerCallback.createTimer(0.1f, refillAllHealth, "Refill health timer", true);
    }//useExtraLife

    public void addToMaxHealth(float extraHealth)
    {
		max_Health += extraHealth;
        //GameManager.self.maxPlayerHealth += extraHealth;
        refillAllHealth();
    }//addToMaxHealth

    public void knockBack(Vector2 direction, float force)
    {
        gameObject.GetComponent<Rigidbody2D>().AddForce(direction.normalized * force);
    }//knockBack
    
    private void doHitColor()
    {
        if(sr)
        {
            sr.color = damageColor;
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
        healthBar.gameObject.SetActive(false);
        //Turn the collisions into triggers so bullets won't collide after this
        //but if we want we can still check for things going through this area
        foreach(Collider2D c in GetComponents<Collider2D> ()) 
        {
            c.isTrigger = true;
        }//foreach

        Invoke("removePlayer",1.0f);


        //Destroy(this.gameObject, 1.0f); //Destroy the enemy after 2 seconds
    }//die

    private void removePlayer()
    {
        gameObject.SetActive(false);
		SceneLoader.self.Load("GameOver");
    }//removePlayer
    
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
            flicker(takingDamage || isInvincible, 6);
        }//else

        if(healthBar)
        {
            healthBar.value = currentHealth / maxHealth;
        }//if

        if(hpText)
        {
            hpText.text = currentHealth + "/" + maxHealth + " HP";
        }//if
        
    }//Update
}//ShipHealth

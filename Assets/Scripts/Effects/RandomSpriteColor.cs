using System;
using UnityEngine;
using System.Collections;

public class RandomSpriteColor : MonoBehaviour 
{
	// Use this for initialization
	void Start () 
	{
		SpriteRenderer renderer = GetComponent<SpriteRenderer>();
		if(renderer == null)
			return;

		RandomSeed r = new RandomSeed((int)(DateTime.Now.Ticks % 1234567890));
		renderer.color = UColor.RandomColor(r);
	}//Start

}//RandomSpriteColor

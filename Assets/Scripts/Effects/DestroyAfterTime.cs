using UnityEngine;
using System.Collections;

public class DestroyAfterTime : MonoBehaviour 
{
	public float time = 1.0f;
	// Use this for initialization
	void Start () 
	{
		GameObject.Destroy(gameObject, time);
	}//Start
}//DestroyAfterTime

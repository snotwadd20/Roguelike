using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FadeScrollTextUI : MonoBehaviour 
{
	public float timeToZero = 30.0f;// How long till it fades completely out and dies
	public Text textObject = null;

	public float timer = 0;
	// Use this for initialization
	void Start () 
	{
		if(textObject == null)
			textObject = GetComponent<Text>();

		timer = 0;
	}//Start
	
	// Update is called once per frame
	void Update () 
	{
		timer += Time.deltaTime;
		Color color = textObject.color;
		color.a = Mathf.Lerp(1,0, timer / timeToZero);
		textObject.color = color;

		if(color.a == 0)
		{
			Canvas.ForceUpdateCanvases();
			Destroy(gameObject);
			Canvas.ForceUpdateCanvases();
		}//if
	}//Update
}//FadeScrollTextUI

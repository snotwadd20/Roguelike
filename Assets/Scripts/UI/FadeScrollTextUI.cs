using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FadeScrollTextUI : MonoBehaviour 
{
	public float timeToZero = 30.0f;// How long till it fades completely out and dies
	public Text textObject = null;

	public float timer = 0;
	// Use this for initialization
	void OnEnable () 
	{
		if(textObject == null)
			textObject = GetComponent<Text>();

		timer = 0;
	}//Start

	private int skipFrames = 10;
	private int frameCounter = 0;
	// Update is called once per frame
	void Update () 
	{
		timer += Time.deltaTime;

		if(frameCounter < skipFrames)
			frameCounter++;
		else
		{
			frameCounter = 0;
			Color color = textObject.color;
			color.a = Mathf.Lerp(1,0, timer / timeToZero);
			textObject.color = color;

			if(color.a == 0)
			{
				/*Canvas.ForceUpdateCanvases();
				LogUI.RePool(textObject);
				Canvas.ForceUpdateCanvases();*/
			}//if
		}//else
	}//Update
}//FadeScrollTextUI

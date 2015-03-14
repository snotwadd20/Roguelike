using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class LogUI : MonoBehaviour 
{
	public Text prefab = null;
	public ScrollRect scrollRect = null;
	public Transform contentPanel = null;

	public static LogUI self = null;
	// Use this for initialization

	public static List<Text> pooledTexts = null;
	void Start () 
	{
		if(self == null)
			self = this;

		if(pooledTexts == null)
			pooledTexts = new List<Text>();
	}//Start

	public static void AddMessage(string message)
	{

		Text txt = TextFromPool(message);

		txt.transform.SetParent(null);
		txt.transform.SetParent(self.contentPanel, true);
		txt.gameObject.SetActive(true);

		Canvas.ForceUpdateCanvases();
		self.scrollRect.verticalNormalizedPosition = 0;
		Canvas.ForceUpdateCanvases();
    }//addMessage

	public static Text CreateText(string message)
	{
		
		Text txt = GameObject.Instantiate(self.prefab).GetComponent<Text>();
		txt.text = message;
		txt.gameObject.AddComponent<FadeScrollTextUI>();
		pooledTexts.Add(txt);
		return txt;
	}//CreateText

	public static void RePool(Text text)
	{
		text.gameObject.SetActive(false);
		FadeScrollTextUI fstUI= text.GetComponent<FadeScrollTextUI>();
		fstUI.timer = 0;
	}//rePool
	
	public static List<Text> pool = null;
	
	public static Text TextFromPool(string message)
	{
		if(pooledTexts == null)
		{
			pooledTexts = new List<Text>();
		}//if

		Text theText = null;
		
		if(pooledTexts.Count > 0)
		{
			for(int i=0; i < pooledTexts.Count; i++)
			{
				if(pooledTexts[i] == null)
				{
					pooledTexts.RemoveAt(i);
					i--;
				}
				else if(pooledTexts[i].gameObject.activeSelf == false)
				{
					theText = pooledTexts[i];
					break;
				}//if
			}//for
		}//if
		
		if(theText == null)
		{
			theText = CreateText("");
		}//if

		theText.text = message;

		return theText;
	}//FromPool
}//LogUI

using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class LogUI : MonoBehaviour 
{
	public int maxChars = 1000;
	public Text allText = null;

	public static LogUI self = null;
	// Use this for initialization

	void Start () 
	{
		if(self == null)
			self = this;

	}//Start

	public static void AddMessage(string message)
	{
		print(self.allText.text.Length);
		if(self.allText.text.Length > self.maxChars)
		{
			int index = self.allText.text.LastIndexOf("\n");
			self.allText.text = self.allText.text.Substring(0,index);
		}//if


		print(self.allText.text.Length);

		self.allText.text = "\n" + message + self.allText.text;


		/*Text txt = TextFromPool(message);

		txt.transform.SetParent(null);
		txt.transform.SetParent(self.contentPanel, true);
		txt.gameObject.SetActive(true);

		Canvas.ForceUpdateCanvases();
		self.scrollRect.verticalNormalizedPosition = 0;
		Canvas.ForceUpdateCanvases();*/
    }//addMessage

}//LogUI

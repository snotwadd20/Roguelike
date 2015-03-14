using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LogUI : MonoBehaviour 
{
	public Text prefab = null;
	public ScrollRect scrollRect = null;
	public Transform contentPanel = null;

	public static LogUI self = null;
	// Use this for initialization
	void Start () 
	{
		if(self == null)
			self = this;
	}

	public static void AddMessage(string message)
	{
		Text txt = GameObject.Instantiate(self.prefab).GetComponent<Text>();
		txt.text = message;
		txt.transform.SetParent(self.contentPanel, true);
		txt.gameObject.SetActive(true);
		txt.gameObject.AddComponent<FadeScrollTextUI>();
		Canvas.ForceUpdateCanvases();
		self.scrollRect.verticalNormalizedPosition = 0;
		Canvas.ForceUpdateCanvases();
    }//addMessage
}//LogUI

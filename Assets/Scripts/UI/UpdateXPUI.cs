using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UpdateXPUI : MonoBehaviour 
{
	public Text xpText = null;
	public Slider xpBar = null;
	public Text levelNum = null;

	// Update is called once per frame
	void Update () 
	{
		xpText.text = XPManager.CurrentXP + "/" + XPManager.XPToNextLevel;
		xpBar.value = (XPManager.CurrentXP / XPManager.XPToNextLevel);
		levelNum.text = "Level: " + XPManager.CurrentPlayerLevel;
	}//Update
}//UpdateXPUI

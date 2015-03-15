using UnityEngine;
using System.Collections;

public class XPManager : MonoBehaviour
{
	public static int CurrentPlayerLevel = 1;
	private static float baseXPToLevel = 5.0f;

	private static XPManager _self = null;
	public static XPManager self
	{
		get
		{
			if(_self == null)
				_self = new GameObject("<XP Manager>").AddComponent<XPManager>();

			return _self;
		}//get
	}//self

	public static float XPToNextLevel
	{
		get
		{
			return Mathf.CeilToInt(baseXPToLevel * (Mathf.Max(1.2f*(CurrentPlayerLevel-1), 1)));
		}//get
	}//
	public static float CurrentXP
	{
		get
		{
			return self._currentXP;
		}//get
	}//currentXP

	private float _currentXP = 0;

	public static float AddAdjustedXP(int currentLevel)
	{
		return AddXP(1 + (currentLevel * 1.2f));
	}//AddAdjustedXP

	public static float AddXP(float xpToAdd)
	{
		ActLog.print("You got <color=lightblue>" + xpToAdd + " XP</color>!");
		self._currentXP += xpToAdd;

		TimerCallback.createTimer(0.5f, checkForLevelUp, "LEVEL UP SCREEN TIMER", true);
		//checkForLevelUp();

		return self._currentXP;
	}//AddXP

	public static void checkForLevelUp()
	{
		if(CurrentXP >= XPToNextLevel)
		{
			CurrentPlayerLevel++;

			float extraHealth = CurrentPlayerLevel * 1.2f * 10;

			//LevelUpUI.self.header;
			LevelUpUI.self.youGot.text = "Level: " + XPManager.CurrentPlayerLevel + 
				"\n+" + extraHealth + " HP" + 
				"\nXP to Level " + (XPManager.CurrentPlayerLevel+1) + ": " + XPManager.XPToNextLevel;

			LevelUpUI.self.gameObject.SetActive(true);


			ActLog.print("<color=yellow>You levelled up!</color>");
			ActLog.print("<color=yellow>You're now level " + CurrentPlayerLevel + "!</color>");
			ActLog.print("<color=lightblue>XP to next level: " + XPToNextLevel + "</color>");
			self._currentXP = 0;

			PlayerHealth ph = R_Player.self.gameObject.GetComponent<PlayerHealth>();
			ph.addToMaxHealth(extraHealth);
			ActLog.print("<color=orange>You got +" + extraHealth + " MaxHP!</color>");
		}//if
	}//

}//XPManager

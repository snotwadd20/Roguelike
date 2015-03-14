using UnityEngine;
using System.Collections;

public class XPManager : MonoBehaviour
{
	public static int CurrentPlayerLevel = 1;
	private static float baseXPToLevel = 1.0f;

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
			return Mathf.CeilToInt(baseXPToLevel * (1.2f*CurrentPlayerLevel));
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

		checkForLevelUp();

		return self._currentXP;
	}//AddXP

	public static void checkForLevelUp()
	{
		if(self._currentXP >= (baseXPToLevel * (1.2f*CurrentPlayerLevel)))
		{
			CurrentPlayerLevel++;
			ActLog.print("<color=yellow>You levelled up!</color>");
			ActLog.print("<color=yellow>You're now level " + CurrentPlayerLevel + "!</color>");
			ActLog.print("<color=lightblue>XP to next level: " + Mathf.CeilToInt(baseXPToLevel * (1.2f*CurrentPlayerLevel)) + "</color>");
			self._currentXP = 0;

			PlayerHealth ph = R_Player.self.gameObject.GetComponent<PlayerHealth>();
			ph.addToMaxHealth(CurrentPlayerLevel * 1.2f * 10);
			ActLog.print("<color=orange>You got +" + (CurrentPlayerLevel * 1.2f * 10) + " MaxHP!</color>");
		}//if
	}//

}//XPManager

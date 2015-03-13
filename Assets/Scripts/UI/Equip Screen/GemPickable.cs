using UnityEngine;
using System.Collections;

public class GemPickable : MonoBehaviour
{
	private Gem _gem = null;
	public int value = 1;
	public Gem.Kind type = Gem.Kind.None;

	public Gem gem
	{
		get
		{
			return _gem;
		}//get
	}//gem

	void OnEnable()
	{
		if(_gem == null)
		{
			_gem = new Gem(value, type);
		}//if
	}//OnEnable
}//GemPickable
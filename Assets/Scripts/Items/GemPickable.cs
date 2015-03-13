using UnityEngine;
using System.Collections;

public class GemPickable : MonoBehaviour
{
	private Gem _gem = null;

	public new string name = "Gem";
	public int value = 1;
	public Gem.Kind type = Gem.Kind.None;
	public int count  = 1;

	public Pickable pickableScript = null;

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
			_gem = new Gem(value, type, pickableScript);
		}//if
	
		if(pickableScript == null)
			pickableScript = gameObject.GetComponent<Pickable>();
		
		if(pickableScript == null)
			pickableScript = gameObject.AddComponent<Pickable>();
		
		pickableScript.name = (type == Gem.Kind.None ? "" : type + " ") + "Gem";
		pickableScript.type = type + "Gem" + value;

		colorGem();

		pickableScript.callback = (Pickable) => {ActLog.print("Press C to equip gems.");};
	}//OnEnable

	private void colorGem()
	{
		SpriteRenderer sr = GetComponent<SpriteRenderer>();
		sr.color = _gem.getColor();
	}//colorGem

}//GemPickable
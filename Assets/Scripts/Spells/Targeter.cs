using UnityEngine;
using System.Collections;

public class Targeter : MonoBehaviour 
{
	public SpellCallback callback = null;

	private static Targeter _self = null;

	public static Targeter self 
	{
		get
		{
			if(_self == null)
				_self = new GameObject("Targeter").AddComponent<Targeter>();

			return _self;
		}//get
	}//self
	void OnEnable () 
	{
		if(_self == null)
			_self = this;

		Cursor.SetCursor(Resources.Load<Texture2D>("targetCursor"), new Vector2(9,0), CursorMode.Auto);
		Time.timeScale = 0;
	}//OnEnable

	void OnDisable()
	{
		Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
		Time.timeScale = 1;
	}//OnDisable
	
	void Update () 
	{
		if(Input.GetMouseButtonDown(0))
		{
			Vector3 clickPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			clickPos.z = -2;

			Collider2D[] colls = Physics2D.OverlapCircleAll(clickPos, 1.0f, 1 << LayerMask.NameToLayer("Enemies"));

			fireSpellAt(colls, clickPos, R_Player.self.transform);

			gameObject.SetActive(false);

		}//if
	}//Update

	private void fireSpellAt(Collider2D[] targets, Vector3 mousePos, Transform caster)
	{
		if(callback != null)
		{
			callback(targets, mousePos, caster);
		}//fireSpellAt
	}//fireSpellAt
}//Targeter
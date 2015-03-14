using UnityEngine;
using System.Collections;

public class FOVSquare : MonoBehaviour 
{
	public R_Player player = null;
	public static float seeDist = 5.5f;

	public float state = NEVER_SEEN;

	//For State
	public const int NEVER_SEEN = 0;
	public const int HAS_SEEN   = 1;

	private Renderer _renderer = null;

	private Vector3 cachedPlayerSpot;

	// Use this for initialization
	void Start () 
	{
		if(_renderer == null)
			_renderer = GetComponent<Renderer>();

		cachedPlayerSpot = Vector3.one * -1;
	}//Start
	
	// Figure out FOV stuff after the frame is over
	void LateUpdate () 
	{
		Color transBlack = Color.clear;
		if(cachedPlayerSpot != player.transform.position && !R_Player.self.isMoving)
		{
			cachedPlayerSpot = player.transform.position;
			//_renderer.enabled = !(Vector2.Distance(player.transform.position, transform.position) < seeDist);

			if(_renderer.enabled)
			{
				SerializedPoint tileLoc = new SerializedPoint((int)transform.position.x, (int)transform.position.y);
				if(state == NEVER_SEEN || tileLoc.x >= R_Map.self.width || tileLoc.y >= R_Map.self.height)
				{
					//Off the map is solid black
					_renderer.enabled = true;

					_renderer.material.color = Color.Lerp(transBlack, Color.black, Vector2.Distance(transform.position, R_Player.self.transform.position) / seeDist);
				}//if
				else if(state == HAS_SEEN)
				{
					_renderer.enabled = true;
					_renderer.material.color = Color.Lerp(transBlack, Color.black, Vector2.Distance(transform.position, R_Player.self.transform.position) / seeDist);
				}//if
			}//if
			else
			{
				//These are the ones you can currently "SEE"
				state = HAS_SEEN;
			}//else
		}//if
	}//LateUpdate
}//FOVSquare

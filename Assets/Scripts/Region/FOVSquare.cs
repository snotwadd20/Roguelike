using UnityEngine;
using System.Collections;

public class FOVSquare : MonoBehaviour 
{
	public R_Player player = null;
	public float seeDist = 4.5f;

	public float state = NEVER_SEEN;

	//For State
	public const int NEVER_SEEN = 0;
	public const int HAS_SEEN   = 1;

	private new Renderer renderer = null;

	private Vector3 cachedPlayerSpot;

	// Use this for initialization
	void Start () 
	{
		if(renderer == null)
			renderer = GetComponent<Renderer>();

		cachedPlayerSpot = Vector3.one * -1;
	}//Start
	
	// Figure out FOV stuff after the frame is over
	void LateUpdate () 
	{
		if(cachedPlayerSpot != player.transform.position)
		{
			cachedPlayerSpot = player.transform.position;
			renderer.enabled = !(Vector2.Distance(player.transform.position, transform.position) < seeDist);

			if(renderer.enabled)
			{
				SerializedPoint tileLoc = new SerializedPoint((int)transform.position.x, (int)transform.position.y);
				if(state == NEVER_SEEN || tileLoc.x >= R_Map.self.width || tileLoc.y >= R_Map.self.height)
				{
					//Off the map is solid black
					renderer.enabled = true;
					renderer.material.color = Color.black;
				}//if
				else if(state == HAS_SEEN)
				{
					renderer.enabled = true;
					renderer.material.color = new Color(1,1,0,0.75f);
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

using UnityEngine;
using System.Collections;

public class R_FOV : MonoBehaviour 
{
	public static float fogZDepth = -8;

	private int mapWidth = 0;
	private int mapHeight = 0;

	private static R_FOV _self;

	private R_Map map = null;
	private R_Player player = null;

	private GameObject[,] fogSprites = null;

	private static Texturizer t = null;

	public void INIT(R_Map mapInstance, R_Player playerInstance)
	{
		if(t == null)
			t = new Texturizer(Color.black);

		t.setupTile(0);

		mapWidth = mapInstance.width;
		mapHeight = mapInstance.height;

		map = mapInstance;
		player = playerInstance;
	
		if(fogSprites == null)
		{
			fogSprites = new GameObject[mapWidth+1, mapHeight+1];
			
			for(int y=0; y < mapHeight+1; y++)
			{
				for(int x=0; x < mapWidth+1; x++)
				{
					fogSprites[x,y] = t.makeObject();
					fogSprites[x,y].name = x + "," + y;
					fogSprites[x,y].transform.position = map.transform.position + new Vector3(x,y);
					fogSprites[x,y].transform.parent = transform;
					fogSprites[x,y].AddComponent<FOVSquare>().player = player;
				}//for
			}//for
		}//if

		//transform.localScale = new Vector3(1, 1, 1);
		transform.position = map.transform.position + new Vector3(-0.5f, 0, fogZDepth);
		transform.parent = map.transform;
	}//INIT

	public static R_FOV self
	{
		get
		{
			if(_self == null)
				_self = new GameObject("FOV Manager").AddComponent<R_FOV>();
			
			return _self;
		}//get
	}//self

	public class TilesMetadata
	{
		int FOVState = 0; //0 is unvisited, 1 is visible, 2 is visited but not visible
		public TilesMetadata(int startingState)
		{
			FOVState = startingState;
		}//constructor
	}//TilesMetadata
}
//R_FOV
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PauseMap : MonoBehaviour 
{
	public R_Map map;
	public GameObject player;
	
	public SerializedPoint currentRoom;
	
	public Image image = null;
	
	//private SerializedPoint lastScreenIndex;
	private Texture2D newTex;
	
	public GameObject playerIcon = null;
	private Vector3[] fourCorners = null;
	
	public static int cellPixels = 30;
	
	private GameObject playerObj = null;
	private Container inventory = null;

	private static PauseMap _self = null;
	public bool[,,] mapRevealed = null;
	public static PauseMap self
	{
		get
		{
			return _self;
		}//get
	}//self

	void Start () 
	{
		if(_self == null)
			_self = this;

		if(mapRevealed == null)
		{
			mapRevealed = new bool[R_Map.MAX_LEVELS,R_Map.self.width, R_Map.self.height];

			for(int i=0; i < R_Map.MAX_LEVELS; i++)
			{
				for(int y=0; y < R_Map.self.height; y++)
				{
					for(int x=0; x < R_Map.self.width; x++)
					{
						mapRevealed[i,x,y]= false;
					}//for
				}//for
			}//for
		}//if

		fourCorners = new Vector3[4];
		
		if(!map)
		{
			GameObject mapObject = GameObject.FindGameObjectWithTag("Map");
			if(!mapObject)
			{
				print ("LKCamera: No R_Map object found in scene.");
				return;
			}//if
			
			map = mapObject.GetComponent<R_Map>();
		}//if
		
		if(playerObj == null)
			playerObj = GameObject.FindGameObjectWithTag("Player");
		
		if(playerObj != null)
		{
			inventory = playerObj.GetComponent<Container>();
		}//if
		else
		{
			print("PauseMap.cs - Can't find player object with tag 'Player'");
		}//else
		
		newTex = new Texture2D(R_Map.self.width * cellPixels, R_Map.self.height * cellPixels);
		newTex.filterMode = FilterMode.Point;
		newTex.wrapMode = TextureWrapMode.Clamp;
		
		makeMap();
		
		if(image == null)
			image = gameObject.GetComponent<Image>();
		
		Sprite theSprite = Sprite.Create(newTex, new Rect(0,0,R_Map.self.width * cellPixels,R_Map.self.height * cellPixels), Vector2.zero);
		//theSprite.pixelsPerUnit = 1.0f;
		image.sprite = theSprite;
		
	}//Start
	
	void OnEnable()
	{
		makeMap();
		
		playerIcon.SetActive(true);
	}//OnEnabled
	
	void OnDisable()
	{
		playerIcon.SetActive(false);
	}//OnEnabled
	
	void Update()
	{
		if(!map || !inventory || !playerObj)
			return;
		
		image.rectTransform.GetWorldCorners(fourCorners);
		//Get the difference in width
		float widthDiff = fourCorners[3].x - fourCorners[0].x;
		//Get the difference in height
		float heightDiff = fourCorners[1].y - fourCorners[0].y;
		
		//Scale it by the map width and height
		float widthRatio = widthDiff/R_Map.self.width;
		float heightRatio = heightDiff/R_Map.self.height;
		
		//Store the lower left pos (for offsets)
		Vector3 lowerLeftPos = fourCorners[0];
		
		SerializedPoint currentScreenPos = worldPosToScreenIndex(R_Player.self.transform.position);
		
		Vector3 playerDotPos = new Vector3();
		playerDotPos.x = lowerLeftPos.x + (widthRatio * currentScreenPos.x) + (widthRatio/2);
		playerDotPos.y = lowerLeftPos.y + (heightRatio * currentScreenPos.y) + heightRatio/2;
		
		playerIcon.transform.position = playerDotPos;//image.transform.position + new Vector3(0,0,image.transform.position.z - 10);
		playerIcon.transform.localScale = new Vector3(widthRatio/3, heightRatio/3, 1.0f);
		
		makeMap();
		
	}//Updates

	public SerializedPoint worldPosToScreenIndex(Vector2 worldPos)
	{
		int x = Mathf.FloorToInt((worldPos.x - transform.position.x));
		int y = Mathf.FloorToInt((worldPos.y - transform.position.y));
		
		return new SerializedPoint(x,y);
	}//worldPosToScreenNumber
	
	// Update is called once per frame
	void makeMap () 
	{
		if(!map || !inventory)
			return;
		
		for(int y =0; y < R_Map.self.height; y++)
		{
			for(int x =0; x < R_Map.self.width; x++)
			{
				Color toPlace = Color.black;
				if(R_Map.self.tiles[x, y] != -1)
				{
					if(mapRevealed[R_Map.self.mapLevel,x,y])
						toPlace = Color.white;//map.getLevelColor(map.tierNumbers[x,y]+2);
				}//if
				
				for(int yy=0; yy < cellPixels; yy++)
				{
					for(int xx=0; xx < cellPixels; xx++)
					{
						if(xx == 0 || xx == cellPixels - 1 || yy == 0 || yy == cellPixels-1)
						{
							Color edgeColor = Color.black;
							
							//int pixelOffset = cellPixels / 3;
							if(R_Map.self.tiles[x, y] != -1 && (mapRevealed[R_Map.self.mapLevel,x,y]) &&
							   ((xx > cellPixels/3 && xx < cellPixels - cellPixels/3) ||
							 (yy > cellPixels/3 && yy < cellPixels - cellPixels/3)) )
							{
								if(map.isLinkedInDir(new SerializedPoint(x,y), R_Map.UP) 
								   && yy == cellPixels -1)
								{
									edgeColor = toPlace;
								}//if
								
								if(map.isLinkedInDir(new SerializedPoint(x,y), R_Map.DOWN) 
								   && yy == 0)
								{
									edgeColor = toPlace;
								}//if
								
								if(map.isLinkedInDir(new SerializedPoint(x,y), R_Map.LEFT) 
								   && xx == 0)
								{
									edgeColor = toPlace;
								}//if
								
								if(map.isLinkedInDir(new SerializedPoint(x,y), R_Map.RIGHT) 
								   && xx == cellPixels-1)
								{
									edgeColor = toPlace;
								}//if
								
							}//if
							newTex.SetPixel(x * cellPixels + xx, y * cellPixels + yy, edgeColor);
						}//if
						else
						{
							newTex.SetPixel(x * cellPixels + xx, y * cellPixels + yy, toPlace);
						}//else
					}//for
				}//for
			}//for
		}//for
		newTex.Apply();
		
	}//makeMap
	
	
}//PauseMap

using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class R_Map : MonoBehaviour 
{
    //********************************
    //CONSTANTS AND STATICS
    //********************************
    //For maze generation
    public const int IN_FRONTIERS = -2;
    public const int UNVISITED = -1;
    public const int VISITED = 0; //Must be 0
    public const int UP    = 0x1;
    public const int RIGHT = 0x2;
    public const int DOWN  = 0x4;
    public const int LEFT  = 0x8;


    //For tilemap placement
    public const int GROUND_TILE = 1;
    public const int WALL_TILE = 0;
    public const int NO_TILE = -1;

    //For flood fills
    public const int NOT_FLOODED = -1;

	public static R_Map self = null;

    //********************************
    //PUBLIC VARIABLES
    //********************************
    //Width and height of the links[] array
    public Vector2 size = Vector2.one;
    public bool useSeed = false;
    public int seed = -1;
    public int wallIn = 3;
    public bool hideClutter = false;
    public int clutterChance = 50;
    public bool isZooming = false;

    public SerializedPoint endPos;
    public SerializedPoint startPos;

    //-------------------------------
    //PRIVATE VARIABLES
    //-------------------------------
    //Width and Height of the tiles[] array
    public int width = -1;
    public int height = -1;

    //For value flooding
    private int highFloodValue = -1;

    //Random number generator
    private RandomSeed r = null;

    //********************************
    //ARRAYS AND LISTS
    //********************************
    //Used for links[] sized arrays
    public int[,] links = null;
	public int[,] floodVals = null;
    //Used for tiles[] sized arrays
    public int[,] tiles = null;
    public int[,] corners = null;

	public int mapLevel = 0;
	private int[] mapSeeds = null;
	public const int MAX_LEVELS = 20;

	public List<GameObject> allCreatedObjects = null;

    public void initializeLists()
    {
		//-------------------------------
        //LISTS<T>
        //-------------------------------
		allCreatedObjects = new List<GameObject>();

        //-------------------------------
        //LINKS[] - SIZED ARRAYS
        //-------------------------------
        if(links == null)
            links = new int[linksWidth,linksHeight];

		if(floodVals == null)
			floodVals = new int[linksWidth,linksHeight];

        for(int y=0; y < linksHeight; y++)
        {
            for(int x=0; x < linksWidth; x++)
            {
                links[x, y] = UNVISITED;
				floodVals[x,y] = NOT_FLOODED;
            }//for
        }//for

        //-------------------------------
        //TILES[] - SIZED ARRAYS
        //-------------------------------

        width = linksWidth * wallIn+1;
        height = linksHeight * wallIn+1;

        if(corners == null)
            corners = new int[width,height];

        if(tiles == null)
            tiles = new int[width,height]; //Adds in spacers for "wall" tiles in maze

		for(int y=0; y < height; y++)
        {
            for(int x=0; x < width; x++)
            {
                tiles[x, y] = NO_TILE;
                corners[x,y] = 0;
            }//for
        }//for

		//-------------------------------
		//MAP SEEDS
		//-------------------------------

		if(mapSeeds == null || mapSeeds.Length == 0)
		{
			mapSeeds = new int[MAX_LEVELS];

			for(int i=0; i < mapSeeds.Length; i++)
			{
				mapSeeds[i] = r.getIntInRange(0,1234567890);
			}//for
		}//for
    }//initializeLists

    //********************************
    // MONOBEHAVIOR FUNCTIONS
    //********************************
    void OnEnable () 
    {
		if(self == null)
			self = this;

		if(!useSeed)
		{
			seed = (int) DateTime.Now.Ticks % 1234567890;
		}//if

		//Set up the initial seed that will make all the mapSeeds
		r = new RandomSeed(seed);
		initializeLists();

		//Now set up a new generator using the correct mapLevel seed
		r.setSeed(mapSeeds[mapLevel]);

        generateMaze();
        startPos = new SerializedPoint(r.getIntInRange(0, linksWidth-1),r.getIntInRange(0, linksHeight-1));
        floodMaze(-1, startPos);

        setUpTilesArray();

		removeSmallWalls();
        //Output the region as tiles
        ToTiles();

        //DEBUG
        print(ToString());
    }//Awake

	void OnDisable()
	{
		links = null;
		floodVals = null;
		tiles = null;
		corners = null;

		for(int i=0; i < allCreatedObjects.Count; i++)
		{
			Destroy(allCreatedObjects[i]);
		}//for
		GC.Collect();
	}//OnDisable

    void Update()
    {
    }//Update

    void Start()
    {
    }//Start

    //********************************
    // MAZE GENERATION FUNCTIONS
    //********************************
	//-------------------------------
	//Removes any 1-unit walls from the tiles array
	public void removeSmallWalls()
	{
		for(int y=0; y < height; y++)
		{
			for(int x=0; x < width; x++)
			{
				if(tiles[x,y] == WALL_TILE)
				{
					int connections = 0;

					int dirs = UP | DOWN | LEFT | RIGHT;
					SerializedPoint neighbor;

					while(dirs > 0)
					{
						int dir = randomDirFromAvailable(ref dirs);
						neighbor = findNeighbor(x,y, dir);
						if(inTileBounds(neighbor))
						{
							if(tiles[x,y] == tiles[neighbor.ix, neighbor.iy])
							{
								connections = connections | dir;
							}//if
						}//if
					}//while

					if(connections == 0)
					{
						tiles[x,y] = GROUND_TILE;
					}//if
				}//if
			}//for
		}//for
	}//removeSmallWalls

	//-------------------------------
    //Autotiles the tiles[] array
    public void autoTileWalls()
    {
        for(int y=0; y < height; y++)
        {
            for(int x=0; x < width; x++)
            {
                int cornerNum = 0;

                //Set up the autotile corners info
                int tileType = tiles[x,y];

                //Above
                if(y+1 < height && tiles[x,y+1] == tileType)
                    cornerNum += 1;
                
                //Right
                if(x+1 < width && tiles[x+1,y] == tileType)
                    cornerNum += 2;
                
                //Below
                if(y-1 >= 0 && tiles[x,y-1] == tileType)
                    cornerNum += 4;
                
                //Left
                if(x-1 >= 0 && tiles[x-1,y] == tileType)
                    cornerNum += 8;

                corners[x,y] = cornerNum;
            }//for
        }//for


    }//autoTile

    public void chooseEndPos()
    {
        List<SerializedPoint> possibleCells = new List<SerializedPoint>();

        int bestCellValue = -1;

        for(int y =0; y < linksHeight; y++)
        {
            for(int x =0; x < linksWidth; x++)
            {
                if(y == 0 || x == 0 || y == height-1 || x == width-1)
                {
                    if(floodVals[x,y] > bestCellValue)
                    {
                        bestCellValue = floodVals[x,y];
                    }//if
                }//if
            }//for
        }//for

        for(int y =0; y < linksHeight; y++)
        {
            for(int x =0; x < linksWidth; x++)
            {
                if(y == 0 || x == 0 || y == height-1 || x == width-1)
                {
                    if(floodVals[x,y] == bestCellValue)
                    {
                        possibleCells.Add(new SerializedPoint(x,y));
                    }//if
                }//if
            }//for
        }//for

        //Put the end at the end
        endPos = randomPointFromList(ref possibleCells);
        //endPos = endPos * 3 + new SerializedPoint(1,1);
    }//chooseEndPos
        

    //-------------------------------
    //Floods a newly created map from a point and fills out floodValues[] with the result
    public void floodMaze(int parentValue, SerializedPoint loc)
    {
        if(floodVals[loc.ix,loc.iy] == NOT_FLOODED) // if it has not ben set yet
        {
            int value = parentValue + 1;
            if(value > highFloodValue)
                highFloodValue = value;

            floodVals[loc.ix,loc.iy] = value;

            for(int i=1; i <=LEFT; i*=2)
            {
                SerializedPoint neighbor = findNeighbor(loc, i);
                if(inLinkBounds(neighbor) && isLinkedInDir(loc, i))
                {
                    floodMaze(value, neighbor);
                }//if
            }//for
        }//if
    }//floodMazeValues

    //-------------------------------
    //Generates an R_MAP Maze using Primm's algorithm
    public void generateMaze()
    {
        List<SerializedPoint> frontiers = new List<SerializedPoint>();
        //Choose a starting point for maze gen
        SerializedPoint startingNode = new SerializedPoint(r.getIntInRange(0, linksWidth-1), r.getIntInRange(0,linksHeight-1));
        links[startingNode.ix, startingNode.iy] = VISITED; //Visited

        addUnvisitedToList(startingNode, ref frontiers);
        foreach(SerializedPoint frontier in frontiers)
        {
            links[frontier.ix, frontier.iy] = IN_FRONTIERS;
        }//foreach

        while(frontiers.Count > 0)
        {
            //Choose a frontier at random   
            SerializedPoint frontier = randomPointFromList(ref frontiers);
            links[frontier.ix, frontier.iy] = VISITED; //Visited

            //Choose a neighbor (that is not a frontier) at random
            List<SerializedPoint> nonFrontiersTemp = new List<SerializedPoint>();

            //Find the non-frontier neighbors of this frontier
            addAllVisitedToList(frontier, ref nonFrontiersTemp);

            //Get one at random
            SerializedPoint nonFrontier = nonFrontiersTemp[r.getIntInRange(0, nonFrontiersTemp.Count-1)];

            //Connect it to the frontier
            linkNeighbors(frontier, nonFrontier);

            //Add all unvisited neighbors of this frontier to the frontiers list
            List<SerializedPoint> unvisitedNeighbors = new List<SerializedPoint>();
            addUnvisitedToList(frontier, ref unvisitedNeighbors);

            for(int i=0; i < unvisitedNeighbors.Count; i++)
            {
                frontiers.Add(unvisitedNeighbors[i]);
                links[unvisitedNeighbors[i].ix, unvisitedNeighbors[i].iy] = IN_FRONTIERS;
            }//for
        }//while

		removeDeadEnds();
    }//generateMaze

	public void removeDeadEnds()
	{
		for(int y = 0; y < linksHeight; y++)
		{
			for(int x = 0; x < linksWidth; x++)
			{
				//If it's a leaf
				if(countLinks(x,y) == 1)
				{
					int dirs = UP | DOWN | LEFT | RIGHT;
					SerializedPoint neighbor = null;

					while(dirs > 0 && neighbor == null)
					{
						int dir = randomDirFromAvailable(ref dirs);
						SerializedPoint tempNbr = findNeighbor(x,y, dir);
						if(inLinkBounds(tempNbr) && !isLinkedInDir(x,y,dir))
						{
							neighbor = tempNbr;
						}//if
					}//while

					if(neighbor != null)
					{
						linkNeighbors(new SerializedPoint(x,y), neighbor);
					}//if
				}//if
			}//for
		}//for
	}//removeDeadEnds

    //-------------------------------
    //Finishes setting up the tiles[] list based on the information we get by flooding the maze
    public void setUpTilesArray()
    {
        //Texturizer t = new Texturizer(Color.white);

        //Put the start at the start
        startPos = (startPos * wallIn) + new SerializedPoint(1,1);

		chooseEndPos();

        //Set up the tiles[] tilemap based on the information in links[] and floodValues
        for(int y =0; y < height; y++)
        {
            for(int x =0; x < width; x++)
            {
                int xx = (x-1)%wallIn;
                int yy = (y-1)%wallIn;
                int linksX = ((x-1)/wallIn);
                int linksY = ((y-1)/wallIn);
                //int cellValue = floodVals[linksX, linksY];
                //bool isEven = ((cellValue % 2) == 0);
                SerializedPoint pos = new SerializedPoint(x,y);

                tiles[x,y] = GROUND_TILE;

                if(x == 0 || y == 0 || y == height-1 || x == width-1)
                {
                    tiles[x,y] = WALL_TILE;
                }//if
				else
                {
                    if( xx == wallIn-1 && yy == wallIn-1 && !pos.Equals(startPos) && !pos.Equals(endPos))
                    {
                        tiles[x,y] = WALL_TILE;
                    }//if
                    else if(!pos.Equals(startPos) && !pos.Equals(endPos))
                    {
                        if(xx == wallIn-1) //RIGHT SIDE WALL
                        {
                            if(isLinkedInDir(linksX, linksY, RIGHT))
                                tiles[x,y] = GROUND_TILE;
                            else
                                tiles[x,y] = WALL_TILE;
                        }//if
                        else if(yy == wallIn-1) //UP SIDE WALL
                        {
                            if(isLinkedInDir(linksX, linksY, UP))
                                tiles[x,y] = GROUND_TILE;
                            else
                                tiles[x,y] = WALL_TILE;
                        }//if
                        else
                        {

                            //A THING
                        }//else
                    }//else
                }//else
            }//for
        }//for

    }//setUpTilesArray

    //********************************
    // FUNCTIONS DEALING WITH LINKS
    //********************************
    //-------------------------------
    // Add all surrounding unvisited link cells to the supplied list
    public int addUnvisitedToList(SerializedPoint center, ref List<SerializedPoint> list)
    {
        int frontiersAdded = 0;

        for(int i=1; i <= 8; i*=2)
        {
            SerializedPoint pt = findNeighbor(center, i);
            if(inLinkBounds(pt) && links[pt.ix, pt.iy] == UNVISITED) // UP
            {
                list.Add(pt);
                frontiersAdded++;
            }//if
        }//for

        return frontiersAdded;
    }//addUnvisitedToList

    // Add all surrounding visited link cells to the supplied list
    public int addAllVisitedToList(SerializedPoint center, ref List<SerializedPoint> list)
    {
        int nonFrontiersAdded = 0;

        for(int i=1; i <= 8; i*=2)
        {
            SerializedPoint pt = findNeighbor(center, i);
            if(inLinkBounds(pt) && links[pt.ix, pt.iy] >= VISITED) // UP
            {
                list.Add(pt);
                nonFrontiersAdded++;
            }//if
        }//for

        return nonFrontiersAdded;
    }//addFrontiers

    //-------------------------------
    //Count the number of links in the bitmask stored in links[]
    public int countLinks(int x, int y)
    {
        return numLinks(links[x,y]);
    }//countLinks
    public int countLinks(SerializedPoint screen)
    {
        return countLinks(screen.ix, screen.iy);
    }//countLinks

    //-------------------------------
    //Find a "neighbor" cell in a direction and return coordinates
    public SerializedPoint findNeighbor(SerializedPoint loc, int dir)
    {
        return findNeighbor(loc.ix, loc.iy, dir);
    }//findNeighbor
    public SerializedPoint findNeighbor(int x, int y, int dir)
    {
        switch(dir)
        {
            case UP:
                y++;
                break;
            case DOWN:
                y--;
                break;
            case LEFT:
                x--;
                break;
            case RIGHT:
                x++;
                break;
            default:
                print ("THIS IS NOT A DIRECTION: " + dir + " ("+ x + ","+y+")");
                return new SerializedPoint(-1,-1);
        }//switch
        
        return new SerializedPoint(x,y);
    }//findNeighbor

    //-------------------------------
    ///Check bitmap to see if a room has a link in that direction
    public bool isLinkedInDir(SerializedPoint loc, int dir)
    {
        return isLinkedInDir(loc.ix, loc.iy, dir);
    }//isLinkedInDir
    public bool isLinkedInDir(int x, int y, int dir)
    {
        return (links[x,y] & dir) > 0;
    }//isLinkedInDir

    //-------------------------------
    //Check if a room in links[] is within bounds
    public bool inLinkBounds(SerializedPoint loc)
    {
        return inLinkBounds(loc.ix, loc.iy);
    }//isInBounds
    public bool inLinkBounds(int x, int y)
    {
        return (x >= 0 && x < linksWidth && 
                y >= 0 && y < linksHeight);
    }//inLinkBounds

    //-------------------------------
    //Check if a room in tiles[] is within bounds
    public bool inTileBounds(SerializedPoint loc)
    {
        return inTileBounds(loc.ix, loc.iy);
    }//isInBounds
    public bool inTileBounds(int x, int y)
    {
        return (x >= 0 && x < width && 
                y >= 0 && y < height);
    }//inTileBounds

    //-------------------------------
    //Links both "doors" between adjacent "neighbor" cells
    public void linkNeighbors(SerializedPoint cellA, SerializedPoint cellB)
    {
        //Figure out what direction cellB is from cellA
        //Does not check if cells are actually adjacent
        if(Mathf.Abs(cellA.ix - cellB.ix) != 1 && 
           Mathf.Abs(cellA.iy - cellB.iy) != 1)
        {
            print("CELLS ARE NOT ADJACENT! : (" + cellA.ix + "," + cellA.iy + " -> " + cellB.ix + "," + cellB.iy + ")");
            return;
        }//if
        
        if(cellB.iy > cellA.iy) 
        {
            //Cell B is above Cell A
            links[cellA.ix,cellA.iy] |= UP;
            links[cellB.ix,cellB.iy] |= DOWN;
        }//if
        else if(cellB.iy < cellA.iy)
        {
            //Cell B is below Cell A
            links[cellA.ix,cellA.iy] |= DOWN;
            links[cellB.ix,cellB.iy] |= UP;
        }//else if
        else if(cellB.ix > cellA.ix)
        {
            //Cell B is to the right of Cell A
            links[cellA.ix,cellA.iy] |= RIGHT;
            links[cellB.ix,cellB.iy] |= LEFT;
        }//else if
        else if(cellB.ix < cellA.ix)
        {
            //Cell B is to the left of Cell A
            links[cellA.ix,cellA.iy] |= LEFT;
            links[cellB.ix,cellB.iy] |= RIGHT;
        }//else if
    }//linkNeighbors

    //-------------------------------
    //Return the number of direction links encoded into a bitmask
    public int numLinks(int bitmask)
    {
        int linksCount = 0;
        for(int i=0; i < 4; i++)
        {
            if((bitmask & (1 << i)) != 0)
            {
                linksCount++;
            }//if
        }//for
        return linksCount;
    }//numLinks

    //-------------------------------
    //Given a direction bitmask, return the bitmask for the opposite wall
    public int oppositeDir(int dir)
    {
        switch(dir)
        {
            case UP:
                return DOWN;
            case DOWN:
                return UP;
            case LEFT:
                return RIGHT;
            case RIGHT:
                return LEFT;
            default:
                print ("THIS IS NOT A DIRECTION: " + dir);
                break;
        }//switch
        return -1;
    }//wal

    //-------------------------------
    //Given a list, pops a random point and returns it
    public SerializedPoint randomPointFromList(ref List<SerializedPoint> list)
    {
        if(list.Count <= 0)
        {
            return null;
        }//if
        
        int index = r.getIntInRange(0, list.Count-1);
        
        SerializedPoint popped = list[index];
        list.RemoveAt(index);
        return popped;
        
    }//randomPointFromList

    //-------------------------------
    //Given a bitmask of directions, choose one and return it. Also update the bitmask
    public int randomDirFromAvailable(ref int availableDirs)
    {
        int adTemp = availableDirs;
        int numDirs = numLinks(adTemp);
        
        if(numDirs == 0)
        {
            return -1;
        }//if
        
        int[] dirList = new int[numDirs];
        
        int nextSpot = 0;
        if((availableDirs & UP) != 0)
        {
            dirList[nextSpot] = UP;
            nextSpot++;
        }//if
        if((availableDirs & DOWN)  != 0)
        {
            dirList[nextSpot] = DOWN;
            nextSpot++;
        }//if
        if((availableDirs & LEFT)  != 0)
        {
            dirList[nextSpot] = LEFT;
            nextSpot++;
        }//if
        if((availableDirs & RIGHT)  != 0)
        {
            dirList[nextSpot] = RIGHT;
            nextSpot++;
        }//if
        int theDir = dirList[r.getIntInRange(0, numDirs-1)];
        availableDirs &= ~theDir;
        return theDir;
    }//randomDirFromAvailable
	
    //********************************
    // CREATION AND PLACING OF OBJECTS
    //********************************
	public void ToTiles()
    {
        Texturizer t = new Texturizer(Color.blue,64,64);
        string mountains = "TileSets/Autotiles/mountain";
        string ground = "TileSets/Autotiles/grass";
        t.loadAutoTile(mountains);
        t.loadAutoTile(ground);
        autoTileWalls();
        Color groundColor = UColor.ChangeBrightness(Color.green, 0.7f);
        groundColor = UColor.ChangeSaturation(groundColor, 0.6f);

        for(int y=height-1; y >= 0; y--)
        {
            for(int x=0; x < width; x++)
            {
                GameObject spt = null;
				bool isCollidable = false;

                switch(tiles[x,y])
                {
                    case GROUND_TILE:
                        t.color = groundColor;
                        t.setupTile(ground, corners[x,y]);
                        break;
                    case WALL_TILE:
						if(corners[x,y] == 15) //Wall singles into rooms
						{
							tiles[x,y] = GROUND_TILE;
							t.color = groundColor;
							t.setupTile(ground, corners[x,y]);
						}//if
						else
						{
							t.color = Color.gray;
		                    t.setupTile(mountains, corners[x,y]);
							isCollidable = true;
						}//else
                        break;
                    case NO_TILE:
                    default:
                        print ("WTF?");
                        break;
                }//switch

                spt = t.makeObject();
                spt.name = ""+ tiles[x,y];

                if(spt != null)
                {
                    spt.transform.position = transform.position + new Vector3(x,y,0);
                    spt.transform.parent = transform;
                    spt.name += " ["+x+","+y+"]";

					if(isCollidable)
						spt.AddComponent<BoxCollider2D>();

					allCreatedObjects.Add(spt);
                }//if
            }//for
        }//for

        //Create a large backdrop quad for the whole region. This keeps the seams in the level invisible-ish
        t.setupFromTexture((Texture2D)Resources.Load("TileSets/Floors/gray"));
        t.color = groundColor;
        GameObject backdrop = t.makeObject(width, height, Vector3.zero, Vector2.one, TextureWrapMode.Repeat);
        backdrop.transform.position = transform.position + Vector3.forward + new Vector3(width/2, height/2);
        backdrop.transform.parent = transform;

		allCreatedObjects.Add(backdrop);
    }//toTiles

    //********************************
    // DEBUG OUTPUT
    //********************************
    public override string ToString ()
    {
        string str = "---[start:"+startPos+"]------[end:" + endPos + "]------[seed:" + seed + "] [mapSeed:" + mapSeeds[mapLevel] +" => L:"+ mapLevel + "]------\n";
        for(int y=height-1; y >= 0; y--)
        {
            for(int x=0; x < width; x++)
            {
                str += corners[x,y] + ", ";
            }
            str += "\n";
        }//for
        str += "\n\n";
        for(int y=height-1; y >= 0; y--)
        {
            for(int x=0; x < width; x++)
            {
                str += tiles[x,y] + ", ";
            }
            str += "\n";
        }//for
        return str;
            
    }//ToString

    //********************************
    //ACCESSORS
    //********************************
    public int linksWidth
    {
        get
        {
            return (int)size.x;
        }//get
    }//width

    public int linksHeight
    {
        get
        {
            return (int)size.y;
        }//get
    }//height
}//R_Map

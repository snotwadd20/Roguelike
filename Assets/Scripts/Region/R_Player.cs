using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class R_Player : MonoBehaviour 
{
    public static Texturizer t = null;

    public bool isUsingGodFinger = false;

    public float moveSpeed = 1f;
    private float gridSize = 1f;
    public enum Orientation {
        Horizontal,
        Vertical
    };

    public const int UP    = 0x1;
    public const int RIGHT = 0x2;
    public const int DOWN  = 0x4;
    public const int LEFT  = 0x8;

    public Orientation gridOrientation = Orientation.Vertical;

    public Vector2 input;
    public bool isMoving = false;

    private float time;
    private float factor;
    
    private R_Map map = null;
    
    public SerializedPoint currentMapPos;
    
    public static R_Player self;
    public bool autoWalkOn = true;

    public float playerLayer = 3;
    public void Start()
    {
        if(self == null)
            self = this;

        if(t == null)
        {
            t = new Texturizer(Color.white, 32,32);
            t.setupTile(0);
        }//if

        if(map == null)
        {
            GameObject mapObj = GameObject.FindGameObjectWithTag("Map");
            if(mapObj != null)
            {
                map = mapObj.GetComponent<R_Map>();
                if(map == null)
                {
                    print ("Can't find OWMap Object in level tagged 'Map'");
                }//if
            }
            else
            {
                print ("Can't find OWMap Object in level tagged 'Map'");
            }//else
        }//map
        
		R_FOV.self.INIT(map, this);

        if(!GetComponent<Collider2D>())
            gameObject.AddComponent<CircleCollider2D>();


        GetComponent<Collider2D>().transform.localScale = new Vector3(0.75f,0.75f,0.75f);

        if(!GetComponent<Rigidbody2D>())
            gameObject.AddComponent<Rigidbody2D>();

        GetComponent<Rigidbody2D>().isKinematic = true;
        GetComponent<Rigidbody2D>().gravityScale = 0;

		SerializedPoint spot = map.startPos;
        transform.position = spot;

    }//Awake

    void OnCollisionEnter2D(Collision2D coll)
    {
        print("PLAYER");
    }//

    public void movePlayerToPos(SerializedPoint point)
    {
        movePlayerToPos((float)point.x, (float)point.y);
    }//if

    public void movePlayerToPos(int x, int y)
    {
        movePlayerToPos((float)x, (float)y);
    }//movePlayerToPos
    public void movePlayerToPos(float x, float y)
    {
        transform.position = new Vector3(x,y,R_Map.self.transform.position.z - playerLayer) + R_Map.self.transform.position;   
    }//movePlayerToPos
    
    public void OnDestroy()
    {

    }//OnDestroy
    
    public bool canWalkInDir(int dir)
    {
        SerializedPoint _NULL = new SerializedPoint(-1,-1);

        //First handle the case of a wall being in the direction, since that's really cheap
        SerializedPoint neighbor = ptInDir(currentMapPos, dir);
        if(neighbor.Equals(_NULL))
            return false;
		if(R_Map.self.tiles[neighbor.ix, neighbor.iy] == R_Map.WALL_TILE)
			return false;
        //Do a ray cast in the direction to see if you hit a piece of clutter

        int oldLayer = gameObject.layer;
        gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");
        Vector2 rayPos = ptInDir(0,0,dir).normalized;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, rayPos, 1);
        Debug.DrawRay(transform.position,rayPos, Color.red, 0.1f);
        gameObject.layer = oldLayer;

        if(hit) //Collide
        {
            return false;
        }//if

        return true;
    }//canWalkInDir
    
    public void Update() 
    {
        if(isUsingGodFinger)
            return;

        if(map == null)
            return;
        
        if (!isMoving) 
        {

            currentMapPos = new SerializedPoint(transform.position.x, transform.position.y);

            input = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
            
            if (Mathf.Abs(input.x) > Mathf.Abs(input.y)) 
            {
                input.y = 0;
            }//if 
            else 
            {
                input.x = 0;
            }//else
            
            if (input != Vector2.zero) 
            {
                int dir = 0;
                
                if(input.x > 0)
                    dir = dir | R_Map.RIGHT;
                else if(input.x < 0)
                    dir = dir | R_Map.LEFT;
                else if(input.y < 0)
                    dir = dir | R_Map.DOWN;
                else if(input.y > 0)
                    dir = dir | R_Map.UP;
                
                if(canWalkInDir(dir))
                {
					TurnManager.NextTurn();
					StartCoroutine(move(transform));
                }//if
                else
                    Bump(dir);
                
            }//if
        }//if
    }//Update

    //Find a "neighbor" cell in a direction and return coordinates
    public Vector2 ptInDir(Vector2 loc, int dir)
    {
        return ptInDir(loc.x, loc.y, dir);
    }//ptInDir
    public Vector2 ptInDir(SerializedPoint loc, int dir)
    {
        return ptInDir((float)loc.x, (float)loc.y, dir);
    }//ptInDir
    public Vector2 ptInDir(float x, float y, int dir)
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
                return new Vector2(-1,-1);
        }//switch
        
        return new Vector2(x,y);
    }//ptInDir

    public void Bump(int dir)
    {
        if(Camera.main.GetComponent<CameraShake>() == null)
        {
            Vector2 direction = ptInDir(0,0,dir).normalized;
            CameraShake.Shake(Camera.main, 0.1f, 0.1f, 1, direction);
        }//if
    }//destination

    public void AutoWalk(Transform destination)
    {
        if(autoWalkOn && !isMoving)
            StartCoroutine(move(transform, destination.position));
    }//AutoWalk

    private Vector3 startPosition;
    private Vector3 endPosition;

    public IEnumerator move(Transform transform) 
    {
        isMoving = true;
        startPosition = transform.position;
        time = 0;
        
        if(gridOrientation == Orientation.Horizontal) 
        {
            endPosition = new Vector3(startPosition.x + System.Math.Sign(input.x) * gridSize,
                                      startPosition.y, startPosition.z + System.Math.Sign(input.y) * gridSize);
        } //if
        else 
        {
            endPosition = new Vector3(startPosition.x + System.Math.Sign(input.x) * gridSize,
                                      startPosition.y + System.Math.Sign(input.y) * gridSize, startPosition.z);
        }//else

        factor = 1f;
        
        while (time < 1f) 
        {
            time += Time.deltaTime * (moveSpeed/gridSize) * factor;
            transform.position = Vector3.Lerp(startPosition, endPosition, time);
            yield return null;
        }//while
        
        isMoving = false;
        yield return 0;
    }//move

    public IEnumerator move(Transform transform, Vector3 endPos) 
    {
        isMoving = true;
        startPosition = transform.position;
        time = 0;
        
        endPosition = endPos;

        factor = 1f;
        
        while (time < 1f) 
        {
            time += Time.deltaTime * (moveSpeed/gridSize) * factor;
            transform.position = Vector3.Lerp(startPosition, endPosition, time);
            yield return null;
        }//while
        
        isMoving = false;
        yield return 0;
    }//move
}//GridMove
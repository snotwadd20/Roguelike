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

	private bool justMoved = false;
	private bool justAttacked = false;
	private int ignoreRaycastLayer = 0;

	public CharacterSheet stats = null;

	private bool facingLeft = true;

	private Animator myAnimator = null;

	private static RandomSeed r = null;

    public void Start()
    {
        if(self == null)
            self = this;

		if(stats == null)
			stats = GetComponent<CharacterSheet>();

		if(myAnimator == null)
			myAnimator = GetComponent<Animator>();

        if(t == null)
        {
            t = new Texturizer(Color.white, 32,32);
            t.setupTile(0);
        }//if

		if(r == null)
			r = new RandomSeed(R_Map.self.seed);

        if(map == null)
        {
            GameObject mapObj = GameObject.FindGameObjectWithTag("Map");
            if(mapObj != null)
            {
                map = mapObj.GetComponent<R_Map>();
                if(map == null)
                {
                    print ("Can't find R_Map Object in level tagged 'Map'");
                }//if
            }
            else
            {
                print ("Can't find R_Map Object in level tagged 'Map'");
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

		SerializedPoint spot;

		if(R_Map.LastLevelVisited < R_Map.Level)
			spot = map.startPos;
		else
			spot = map.endPos;

        transform.position = spot;

		justMoved = false;

		ignoreRaycastLayer = LayerMask.NameToLayer("Ignore Raycast");

		ActLog.print("<color=lime>[I] Open Inventory.</color>");
		ActLog.print("<color=lime>[C] Open Gem Equipper.</color>");
		ActLog.print("<color=lime>[G] Get item from floor.</color>");
		ActLog.print("<color=lime>[S] Use the stairs.</color>");
		ActLog.print("<color=lime>[UDRL] or [NUMPAD] to move.</color>");
		ActLog.print("<color=cyan>Bump into enemies to attack them!</color>");
		ActLog.print("<color=cyan>Use scrolls for ranged attacks!</color>");
		ActLog.print("<color=cyan>Potions will heal you!</color>");

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
        
        return true;
    }//canWalkInDir
    
	private void clearAttackFlag()
	{
		justAttacked = false;
	}

    public void Update() 
    {
        if(isUsingGodFinger)
            return;

        if(map == null)
            return;
        
		if(justMoved && !isMoving) //Did you just finish moving?
		{
			justMoved = false;
			TurnManager.NextTurn();
		}//if

        if (!justAttacked && !isMoving) 
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
				{
                    dir = dir | R_Map.RIGHT;
					facingLeft = false;
				}
                else if(input.x < 0)
				{
                    dir = dir | R_Map.LEFT;
					facingLeft = true;
				}
                else if(input.y < 0)
                    dir = dir | R_Map.DOWN;
                else if(input.y > 0)
                    dir = dir | R_Map.UP;
                
                if(canWalkInDir(dir))
                {
					justAttacked = true;

					TimerCallback.createTimer(0.35f, clearAttackFlag, "Stop Moving timer", false);
					//If going in that direction would hit an enemy, do an attack instead
					Vector3 rayPos = transform.position + (Vector3)ptInDir(0,0,dir).normalized;
					RaycastHit2D hit = raycastTo(rayPos - transform.position,1,"Enemies");
					
					if(hit) //Collide
					{
						EnemyHealth eh = hit.collider.gameObject.GetComponent<EnemyHealth>();
						if(eh != null)
						{
							float mult = rollToAttack();
							float extraDamage = (mult * stats.Attack);
							float damage = XPManager.CurrentPlayerLevel + extraDamage;

							ActLog.print("<color=orange>[Attack: " + stats.Attack + "]</color> Extra damage: " + extraDamage);
							ActLog.print("Player did <color=red>" + damage + "</color> damage to " + eh.name);
							eh.dealDamage(damage);
							CameraShake.Shake(Camera.main, 0.25f, 0.4f, 1.0f, Vector2.zero) ;
						}//if
						TurnManager.NextTurn();
					}//if
					else
					{
						justMoved = true;
						StartCoroutine(move(transform));
					}//else
                }//if
                else
                    Bump(dir);
            }//if
        }//if


		myAnimator.SetFloat("horzSpeed", (isMoving || justAttacked)? 1 : 0);

		if(facingLeft)
			transform.localScale = new Vector3(1,1,1);
		else
			transform.localScale = new Vector3(-1,1,1);
    }//Update

	public float rollToAttack()
	{
		//Return damage multiplier
		float plusCritPerLuck = 2.0f;
		float baseCrit = 10.0f + plusCritPerLuck * stats.Luck;
		float baseHit = (100 - baseCrit) * (1.0f/3.0f);
		//float baseMiss = 100 - baseCrit - baseHit;

		if(stats.Luck > 0)
		{
			ActLog.print("<color=lime>[Luck: " + stats.Luck + "]</color>: +" + (plusCritPerLuck*stats.Luck) + " Damage Multiplier Chance.");
		}//if

		float roll = r.getIntInRange(1,100);
		if(roll <= baseCrit)
		{
			ActLog.print("<color=lime>Critical Hit!!</color> 4x Damage!!");
			return 4;
		}//if
		else if(roll <= baseHit)
		{
			ActLog.print("<color=lime>Nice Hit!</color> 2x Damage!");
			return 2;
		}//else if
		else
		{
			ActLog.print("<color=lime>Ok Hit.</color> 1x Damage.");
		}//else

		return 1;
	}//rollToAttack

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

	//This is the slower version (since it has to mess with strings)
	private RaycastHit2D raycastTo(Vector3 direction, float length, params string[] layerNames)
	{
		int layerFlags = 0;//1 << LayerMask.NameToLayer("Player");
		if(layerNames.Length == 0)
			layerFlags = 1 << LayerMask.NameToLayer("Default");
		else
		{
			foreach(string lName in layerNames)
			{
				layerFlags  |= 1 << LayerMask.NameToLayer(lName);
			}//foreach
		}//else
		
		
		return raycastTo(direction, length, layerFlags);
	}//ray
	
	//Faster  version
	private RaycastHit2D raycastTo(Vector3 direction, float length, int layerFlags)
	{
		int oldLayer = gameObject.layer;
		gameObject.layer = ignoreRaycastLayer;
		
		RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, length, layerFlags);
		
		if(hit.collider != null)
			Debug.DrawRay(transform.position, direction.normalized*length, Color.red, 2.0f);
		else
			Debug.DrawRay(transform.position, direction.normalized*length, Color.white, 2.0f);
		
		gameObject.layer = oldLayer;
		return hit;
	}//ray
}//GridMove
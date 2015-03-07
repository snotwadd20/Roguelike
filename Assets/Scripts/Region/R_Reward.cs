using UnityEngine;
using System.Collections;

public class R_Reward : MonoBehaviour 
{
    public SerializedPoint mapPos = null;
    public int value = -1;

    public static Texturizer t = null;

    // Use this for initialization
    void Awake () 
    {
        if(GetComponent<Collider2D>() == null)
            gameObject.AddComponent<CircleCollider2D>();

        GetComponent<Collider2D>().isTrigger = true;
    }//Start
    
    void OnTriggerEnter2D(Collider2D coll)
    {
        if(coll.gameObject == R_Player.self.gameObject)
        {
            spawnReward();
        }//if
    }//

    public void spawnReward()
    {
        print("REWARD -> " + transform.position);
        Destroy(gameObject);
    }//spawnReward
    // Update is called once per frame
    public static R_Reward Create (SerializedPoint mapPos, Transform parent, int value)
    {
        if(t == null)
        {
            t = new Texturizer(Color.yellow, 256,256);
            t.setupCircle();
        }//if
        
        GameObject obj = t.makeObject();
        obj.name = "Region Reward: [" + mapPos.x + "," + mapPos.y + "] -> " + value;
        obj.transform.position = parent.transform.position + new Vector3(mapPos.x,mapPos.y,-2);
        obj.transform.parent = parent;
        obj.transform.localScale = obj.transform.localScale * 0.75f ;
        
        R_Reward scpt = obj.AddComponent<R_Reward>();
        scpt.mapPos = mapPos;
        scpt.value = value;
        
        return scpt;
    }//Create
}

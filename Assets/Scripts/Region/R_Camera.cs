using UnityEngine;
using System.Collections;

public class R_Camera : MonoBehaviour
{
    public R_Player player = null;

    private GameObject follow = null;

    public float xOffset = 0.0f;
    public float yOffset = 0.0f;
    public float height = 10.0f;
    // Use this for initialization
    void Start()
    {
        if(player == null)
            player = R_Player.self;
        
		if(follow == null)
			follow = player.gameObject;
    }//Awake
    
    void Update()
    {
        if(R_Player.self == null || player == null)
            return;

        float playerY = follow.transform.position.y + yOffset;
        
        
        
        transform.position = new Vector3(follow.transform.position.x + xOffset,
                                         playerY,
                                         follow.transform.position.z - height);
    }//Update
    
    void LateUpdate()
    {

	}//LateUpdate
}//R_Camera

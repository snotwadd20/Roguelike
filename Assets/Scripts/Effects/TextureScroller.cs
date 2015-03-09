using UnityEngine;
using System.Collections;

public class TextureScroller : MonoBehaviour {

    public Vector2 initialOffset = Vector2.zero;
    public Vector2 scrollDirection = Vector2.up;
    public float scrollSpeed = 1.0f;

    private float scroller = 0;
	// Use this for initialization
	void Start () 
    {
        GetComponent<Renderer>().material.SetTextureOffset("_MainTex",initialOffset );

        if(GetComponent<Renderer>().material.HasProperty("_BumpMap"))
            GetComponent<Renderer>().material.SetTextureOffset("_BumpMap",initialOffset);
	}//Start
	
	// Update is called once per frame
	void Update () 
    {
        scroller += Time.deltaTime * scrollSpeed;

        GetComponent<Renderer>().material.SetTextureOffset("_MainTex",scrollDirection * scroller );

        if(GetComponent<Renderer>().material.HasProperty("_BumpMap"))
            GetComponent<Renderer>().material.SetTextureOffset("_BumpMap",scrollDirection * scroller );
	}//Update
}//TextureScroller

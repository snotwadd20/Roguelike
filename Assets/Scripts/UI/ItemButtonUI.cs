using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ItemButtonUI : MonoBehaviour 
{
	public Button button = null;
	public Image image = null;
	public Text textObj = null;
	public Text itemCount = null;
	public Pickable item = null;

	void Update()
	{
		if(item == null)
		{
			Destroy(gameObject);
			return;
		}//if

		textObj.text = item.name;
		itemCount.text = item.count + "";
	}//Update
}//ItemButtonUI
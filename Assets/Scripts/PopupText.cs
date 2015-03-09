using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PopupText : MonoBehaviour 
{
	public Text textObject = null;

	private const float lifeSpan = 1.0f;

	private static GameObject prefab = null;
	void OnEnable () 
	{
		Destroy(gameObject, lifeSpan);
	}//OnEnable


	public static PopupText Create(string text, Vector3 position, Color color)
	{
		if(prefab == null)
		{
			prefab = Resources.Load<GameObject>("UI/textpopup");
		}//prefab

		PopupText pText = ((GameObject)GameObject.Instantiate(prefab)).GetComponent<PopupText>();
		pText.textObject.text = text;
		pText.textObject.color = color;
		pText.transform.position = position;

		pText.gameObject.SetActive(true);

		return pText;
	}//Create
}//PopupText

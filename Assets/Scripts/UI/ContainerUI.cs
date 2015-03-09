using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class ContainerUI : MonoBehaviour 
{
	public Text headerText = null;
	public Button buttonPrefab = null;
	public Transform contentPanel = null;
	public Container container = null;
	public ScrollRect scrollRect = null;

	private static GameObject prefab = null;
	private static Transform mainCanvas = null;

	void Start () 
	{
		if(mainCanvas == null)
			mainCanvas = GameObject.FindGameObjectWithTag("MainCanvas").transform;

		headerText.text = container.name;

		foreach(KeyValuePair<string, Pickable> item in container.contents)
		{

			// do something with entry.Value or entry.Key
			ItemButtonUI itemButton = ((GameObject)Instantiate(buttonPrefab.gameObject)).GetComponent<ItemButtonUI>();

			SpriteRenderer sr = item.Value.gameObject.GetComponent<SpriteRenderer>();

			if(sr != null)
			{
				Sprite sprite = sr.sprite;
				itemButton.image.sprite = sprite;
				itemButton.image.color = sr.color;
			}//if

			itemButton.textObj.text = item.Value.name;
			itemButton.itemCount.text = item.Value.count + "";
			itemButton.gameObject.SetActive(true);
			itemButton.transform.SetParent(contentPanel);
			itemButton.transform.localScale = Vector3.one;


		}//foreach
		Canvas.ForceUpdateCanvases();
		scrollRect.verticalNormalizedPosition = 0;
		Canvas.ForceUpdateCanvases();
		//Canvas.ForceUpdateCanvases();

		Time.timeScale = 0;

		transform.SetParent(mainCanvas, true);
	}//OnEnable

	void Update()
	{
	}//Update

	public void Close()
	{
		Time.timeScale = 1;
		Destroy(gameObject);
		//gameObject.SetActive(false);
	}//Close
	
	public static ContainerUI Create(Container container)
	{
		if(prefab == null)
		{
			prefab = Resources.Load<GameObject>("UI/containerUI");
		}//prefab
		
		ContainerUI containerWindow = ((GameObject)GameObject.Instantiate(prefab)).GetComponent<ContainerUI>();
		containerWindow.container = container;
		containerWindow.gameObject.SetActive(true);
		
		return containerWindow;
	}//Create
}//ContainerUI

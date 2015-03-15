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
		List<string> removeNulls = new List<string>();
		foreach(KeyValuePair<string, Pickable> item in container.contents)
		{
			Pickable pick = item.Value;
			if(pick == null)
			{
				removeNulls.Add(item.Key);
				continue;
			}//if
			// do something with entry.Value or entry.Key
			ItemButtonUI itemButton = ((GameObject)Instantiate(buttonPrefab.gameObject)).GetComponent<ItemButtonUI>();

			SpriteRenderer sr = pick.gameObject.GetComponent<SpriteRenderer>();

			if(sr != null)
			{
				Sprite sprite = sr.sprite;
				itemButton.image.sprite = sprite;
				itemButton.image.color = sr.color;
			}//if

			itemButton.item = pick;

			itemButton.gameObject.SetActive(true);
			itemButton.transform.SetParent(contentPanel);
			itemButton.transform.SetAsLastSibling();

			itemButton.transform.localScale = Vector3.one;


			itemButton.button.onClick.AddListener(() => {use(pick);} );

		}//foreach
		foreach(string type in removeNulls)
		{
			container.Remove(type);
			print (type);
		}//type

		Canvas.ForceUpdateCanvases();
		scrollRect.verticalNormalizedPosition = 0;
		Canvas.ForceUpdateCanvases();
		//Canvas.ForceUpdateCanvases();

		//Time.timeScale = 0;
		RoguelikeControls.Pause(false);
		transform.SetParent(mainCanvas, true);
	}//OnEnable

	void Update()
	{
		if(Input.GetKeyUp(KeyCode.Escape) || Input.GetKeyUp(KeyCode.I))
			Close();
	}//Update

	private void clickButton()
	{
	}//clickButton

	public void use(Pickable pickable)
	{
		pickable.use();
	}//use

	public void Close()
	{
		//Time.timeScale = 1;
		Destroy(gameObject);
		RoguelikeControls.UnPause();
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

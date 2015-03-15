using UnityEngine;
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;

public class MonsterMaker
{

	private static Deck<Sprite> sprites = null;
	private static Deck<Color> colors = null;

	private static RandomSeed r = null;

	private static List<GameObject> monsterPrefabs = null;

	public static void CleanUp()
	{
		/*for(int i=0; i < monsterPrefabs.Count; i++)
		{
			GameObject.Destroy(monsterPrefabs[i]);
			monsterPrefabs[i] = null;
		}//for
		monsterPrefabs = null;

		for(int i=0; i < sprites.Count; i++)
		{
			GameObject.Destroy(sprites.cards[i]);
			sprites.cards[i] = null;
		}//for
		sprites = null;

		colors = null;*/
	}//CleanUp
	private static int LastMapSeed = -1;
	public static void Initialize()
	{
		if(r == null)
			r = new RandomSeed(0);

		if(R_Map.mapSeed!= LastMapSeed)
		{
			LastMapSeed = R_Map.mapSeed;
			r.setSeed(R_Map.mapSeed);
		}//if

		SetupSprites();
		SetupColors();

		if(monsterPrefabs == null)
		{
			monsterPrefabs = new List<GameObject>();

			EnemyMove em;
			EnemyHealth eh;

			for(int i=0; i < 7; i++)
			{
				GameObject monsterPrefab = MakeMonsterArt();
				monsterPrefab.name += " [PREFAB]";
				//monsterPrefab.SetActive(false);
				monsterPrefab.transform.position = new Vector3(0,i-10, -2);

				em = monsterPrefab.AddComponent<EnemyMove>();
				em.damage = Mathf.Max(1, r.getIntInRange(R_Map.self.mapLevel+1, (R_Map.self.mapLevel+1)*3)*10);

				eh = monsterPrefab.AddComponent<EnemyHealth>();
				eh.startingHealth = (R_Map.self.mapLevel * 1.35f) * r.getIntInRange(1,5);

				monsterPrefabs.Add(monsterPrefab);
			}//for
		}//for

	}//MonsterMaker

	private static void SetupSprites()
	{
		if(sprites == null)
		{
			sprites = new Deck<Sprite>("Sprite Deck", r);

			Sprite[] spts = Resources.LoadAll<Sprite>("Sprites/MonsterShapes");
			foreach(Sprite sprite in spts)
			{
				sprites.Add(sprite);
			}//foreach
		}//sprites
	}//setupSprites

	private static void SetupColors()
	{
		if(colors == null)
		{
			colors = new Deck<Color>("Colors Deck", r);

			Color[] colArray = new Color[8];
			colArray[0] = new Color(1,0,0);
			colArray[1] = new Color(1,0.5f,0);
			colArray[2] = new Color(1,1,0);
			colArray[3] = new Color(0,1,0);
			colArray[4] = new Color(0,0,1);
			colArray[5] = new Color(1,0,1);
			colArray[6] = new Color(1,0.25f,0.5f);
			colArray[7] = new Color(0.5f,0.7f,0.75f);

			foreach(Color color in colArray)
			{
				Color realColor = UColor.ChangeBrightness(color, 0.8f);
				realColor = UColor.ChangeRelativeSaturation(realColor, 0.7f);
				colors.Add(realColor);
			}//foreach
		}//if
	}//setupTierColors

	private static GameObject MakeMonsterArt()
	{
		SpriteRenderer monsterArt = new GameObject("Monster Art").AddComponent<SpriteRenderer>();
		monsterArt.sprite = sprites.Draw();
		monsterArt.color = colors.Draw();
		return monsterArt.gameObject;
	}//MakeMonsterArt

	public static GameObject Spawn(Vector3 position)
	{
		Initialize();

		GameObject monster = (GameObject)GameObject.Instantiate(monsterPrefabs[r.getIntInRange(0,monsterPrefabs.Count-1)]);
		monster.name = "Monster";
		monster.transform.position = position + Vector3.forward * -1;
		monster.layer = LayerMask.NameToLayer("Enemies");
		monster.AddComponent<BoxCollider2D>();
		monster.SetActive(true);
		return monster;
	}//Create
}//MonsterMaker
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MonsterMaker
{

	private static Deck<Sprite> sprites = null;
	private static Deck<Color> colors = null;

	private static RandomSeed r = null;

	private static List<GameObject> monsters = null;

	public static void Initialize()
	{
		r = new RandomSeed(R_Map.self.seed);
		SetupSprites();
		SetupColors();

		if(monsters == null)
		{
			monsters = new List<GameObject>();

			EnemyMove em;
			EnemyHealth eh;

			for(int i=0; i < 7; i++)
			{
				GameObject monsterPrefab = MakeMonsterArt();
				monsterPrefab.name += " [PREFAB]";
				monsterPrefab.SetActive(false);
				monsterPrefab.transform.position = new Vector3(0,i-10, -2);

				em = monsterPrefab.AddComponent<EnemyMove>();
				em.damage = r.getIntInRange(3,10) * 2;

				eh = monsterPrefab.AddComponent<EnemyHealth>();
				eh.startingHealth = r.getIntInRange(1,5) * 10;

				monsters.Add(monsterPrefab);
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

			Color[] colArray = new Color[7];
			colArray[0] = new Color(1,0,0);
			colArray[1] = new Color(1,0.5f,0);
			colArray[2] = new Color(1,1,0);
			colArray[3] = new Color(0,1,0);
			colArray[4] = new Color(0,0,1);
			colArray[5] = new Color(1,0,1);
			colArray[6] = new Color(1,0.25f,0.5f);

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

		GameObject monster = (GameObject)GameObject.Instantiate(monsters[r.getIntInRange(0,monsters.Count-1)]);
		monster.name = "Monster!";
		monster.transform.position = position + Vector3.forward * -1;
		monster.layer = LayerMask.NameToLayer("Enemies");
		monster.AddComponent<BoxCollider2D>();
		monster.SetActive(true);
		return monster;
	}//Create
}//MonsterMaker
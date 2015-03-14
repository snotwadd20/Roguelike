using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class TreasureManager : MonoBehaviour 
{
	public static GameObject chestPrefab = null;

	private static List<Pickable> lootPrefabs = null;
	private static List<Pickable> lootPrefabsNoGems = null;
	public static RandomSeed r = null;

	public static GameObject SpawnChest(Vector3 pos)
	{
		if(chestPrefab == null)
			chestPrefab = Resources.Load<GameObject>("Objects/chest");

		GameObject go = Instantiate<GameObject>(chestPrefab);
		go.transform.position = pos + Vector3.forward * -1;
		go.SetActive(true);
		return go;
	}//SpawnChest

	public static Pickable SpawnLoot(Vector3 pos, Container container, bool allowGems = true)
	{
		if(lootPrefabs == null)
			setupPrefabs();
		if(r == null)
			r = new RandomSeed(R_Map.self.seed);

		Pickable prefab;
		if(allowGems)
			prefab = lootPrefabs[r.getIntInRange(0, lootPrefabs.Count-1)];
		else
			prefab = lootPrefabsNoGems[r.getIntInRange(0, lootPrefabsNoGems.Count-1)];

		Pickable instance = Instantiate<GameObject>(prefab.gameObject).GetComponent<Pickable>();
		instance.transform.position = pos;
		instance.gameObject.SetActive(true);
		instance.holdingContainer = container;
		instance.count = 1;

		//Do stuff for gem amounts here
		if(allowGems)
		{
			GemPickable gp = instance.GetComponent<GemPickable>();
			if(gp)
			{
				int roll = r.getIntInRange(1,100);
				if(roll == 100)
					gp.gem.value = 3;
				else if(roll > 90)
					gp.gem.value = 2;
				else
					gp.gem.value = 1;

				bool isStatAligned = r.percentageChance(10);
				if(isStatAligned)
				{
					gp.gem.type = (Gem.Kind)r.getIntInRange(0, Gem.NUM_KINDS-1);
				}//if
				else
				{
					gp.gem.type = Gem.Kind.None;
				}//else
			}//if
		}//if

		return instance;
	}//SpawnLoot

	private static void setupPrefabs()
	{
		lootPrefabs = new List<Pickable>();

		lootPrefabs.Add(Resources.Load<GameObject>("Objects/scrollFireball").GetComponent<Pickable>());
		lootPrefabs.Add(Resources.Load<GameObject>("Objects/scrollMagicMissile").GetComponent<Pickable>());
		lootPrefabs.Add(Resources.Load<GameObject>("Objects/healingPotion").GetComponent<Pickable>());

		//Store the loot before adding the gems
		lootPrefabsNoGems = new List<Pickable>();
		foreach(Pickable obj in lootPrefabs)
		{
			lootPrefabsNoGems.Add(obj);
		}//foreach

		lootPrefabs.Add(Resources.Load<GameObject>("Objects/gem").GetComponent<Pickable>());

		foreach(Pickable prefab in lootPrefabs)
		{
			prefab.gameObject.SetActive(false);
		}//prefab
	}//setupPrefabs

}//TreasureManager
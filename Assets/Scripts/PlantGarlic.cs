using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantGarlic : PlantBase
{
	public new SkinnedMeshRenderer renderer;

	// Use this for initialization
	void Start ()
	{
		renderer = GetComponentInChildren<SkinnedMeshRenderer> ();
		plantType = PlantType.potato;
	}
	
	// Update is called once per frame
	void Update ()
	{
		
	}

	void RemoveInfection ()
	{
		if (stance == PlantStance.friendly) {
			// get neighbours
			PlantBase[] neighbours = PlantManager.Get8NeighboursForPosition (gridPos);
			for (int i = 0; i < neighbours.Length; i++) {
				// can't have null plants
				if (neighbours [i] == null) {
					continue;
				}

				neighbours [i].ClearInfection ();
			}
		}
	}

	public override void OnPlantStanceChanged ()
	{
		RemoveInfection ();
		switch (GetStance ()) {
		case PlantStance.enemy:
			renderer.sharedMaterial = enemyMaterial;
			break;
		case PlantStance.friendly:
			renderer.sharedMaterial = friendlyMaterial;
			break;
		}
	}
}

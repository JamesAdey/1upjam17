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

	void ClearInfection ()
	{
		if (stance == PlantStance.friendly) {
			// get neighbours
			PlantBase[] neighbours = PlantManager.GetNeighboursForPosition (gridPos);
			// count how much corn is around us
			int cornCount = 0;
			for (int i = 0; i < neighbours.Length; i++) {
				// can't have null plants
				if (neighbours [i] == null) {
					continue;
				}

				if (neighbours [i].plantType == PlantType.corn) {
					cornCount++;
				}

			}
			// not enough corn?
			if (cornCount < 2) {
				return;
			}

			for (int i = 0; i < neighbours.Length; i++) {
				if (neighbours [i] != null) {
					neighbours [i].ClearInfection ();
				}
			}
		}
	}

	public override void OnPlantStanceChanged ()
	{
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

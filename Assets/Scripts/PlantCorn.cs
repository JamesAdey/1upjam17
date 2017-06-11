using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantCorn : PlantBase
{
	const int minInfectionTime = 2;
	const int maxInfectionTime = 7;
	float nextInfectionTime = 0;
	bool needInfection;
	PlantMaterialControl[] materialControllers;

	// Use this for initialization
	void Start ()
	{
		plantType = PlantType.corn;
		materialControllers = GetComponentsInChildren<PlantMaterialControl> ();
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (!GameManager.isPlaying) {
			return;
		}
		// early out if no infection required

		if (Time.time > nextInfectionTime) {
			nextInfectionTime = Time.time + Random.Range (minInfectionTime, maxInfectionTime);
			if (needInfection) {	
				SpreadInfection ();
				needInfection = false;
			}
		}
	}

	public override void OnPlantStanceChanged ()
	{
		if (GetStance () == PlantStance.friendly) {
			needInfection = true;
		}
		for (int i = 0; i < materialControllers.Length; i++) {
			materialControllers [i].SetStance (GetStance ());
		}
	}

	void SpreadInfection ()
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
					neighbours [i].InfectPlant ();
				}
			}
		}
	}

}

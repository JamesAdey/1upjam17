using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantCorn : PlantBase
{
	const int minInfectionTime = 2;
	const int maxInfectionTime = 11;
	float nextInfectionTime;

	// Use this for initialization
	void Start ()
	{
		plantType = PlantType.corn;
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (!GameManager.isPlaying) {
			return;
		}
		if (Time.time > nextInfectionTime) {
			nextInfectionTime += Random.Range (minInfectionTime, maxInfectionTime);
			SpreadInfection ();
		}
	}

	public override void OnPlantStanceChanged ()
	{

	}

	void SpreadInfection ()
	{
		if (stance == PlantStance.friendly) {
			// get neighbours
			PlantBase[] neighbours = PlantManager.GetNeighboursForPosition (gridPos);
			// count how much corn is around us
			int cornCount = 0;
			for (int i = 0; i < neighbours.Length; i++) {
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

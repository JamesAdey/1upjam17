using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlantStance
{
	friendly,
	neutral,
	enemy
}

public enum PlantType
{
	any,
	corn,
	potato,
	onion,
	garlic,
	shroom
}

public abstract class PlantBase : MonoBehaviour
{
	public PlantType plantType;
	private PlantStance _stance = PlantStance.neutral;
	public Vector2 gridPos;
	public float alarmDuration = 5;
	private float nextAlarmTime = 0;
	public Material enemyMaterial;
	public Material friendlyMaterial;

	public PlantStance stance {
		get {
			return _stance;
		}
		protected set {
			if (_stance != value) {
				_stance = value;
				OnPlantStanceChanged ();
			} else {
				_stance = value;
			}
		}
	}

	public PlantStance GetStance ()
	{
		return stance;
	}

	public void InfectPlant ()
	{
		stance = PlantStance.friendly;
	}

	public void ClearInfection ()
	{
		stance = PlantStance.enemy;
	}

	public void CreateAlarm ()
	{
		if (Time.time > nextAlarmTime) {
			AIManager.SpawnAlarmAtPosition (transform.position + Vector3.up, alarmDuration);
			// 1.25 seconds between alarms
			nextAlarmTime = Time.time + alarmDuration + 1.25f;
		}
	}

	public abstract void OnPlantStanceChanged ();
}

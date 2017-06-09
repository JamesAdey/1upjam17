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
	corn,
	potato
}

public abstract class PlantBase : MonoBehaviour
{
	public PlantType plantType;
	private PlantStance _stance;
	public Vector2 gridPos;

	public PlantStance stance {
		get {
			return _stance;
		}
		protected set {
			if (_stance != value) {
				OnPlantStanceChanged ();
			}
			_stance = value;
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

	public abstract void OnPlantStanceChanged ();
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct WorldTile
{
	public TileType tileType;
	public PlantBase plant;

	public bool hasPlant {
		get {
			return plant != null;
		}
	}

}

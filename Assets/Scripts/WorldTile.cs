using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class WorldTile
{
	private int _x;
	private int _y;
	public bool marked;
	public WorldTile previous;

	public int x {
		get {
			return _x;
		}
	}

	public int y {
		get {
			return _y;
		}
	}

	public WorldTile (TileType t, int nx, int ny)
	{
		tileType = t;
		plant = null;
		_x = nx;
		_y = ny;
	}

	public TileType tileType;
	public PlantBase plant;

	public bool hasPlant {
		get {
			return plant != null;
		}
	}

}

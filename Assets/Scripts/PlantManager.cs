using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantManager : MonoBehaviour
{
	public static PlantManager singleton;
	WorldTile[,] world;
	public int worldX = 64;
	public int worldY = 64;
	// Use this for initialization
	void Start ()
	{
		singleton = this;
		world = new WorldTile[worldX, worldY];
		for (int i = 0; i < worldX; i++) {
			for (int j = 0; j < worldY; j++) {

			}
		}
	}
	
	// Update is called once per frame
	void Update ()
	{
		
	}

	static Vector2[] directions = { Vector2.up, Vector2.down, Vector2.left, Vector2.right };


	public static PlantBase[] GetNeighboursForPosition (Vector2 pos)
	{
		PlantBase[] neighbours = new PlantBase[directions.Length];

		int x, y;
		Vector2 newPos;
		for (int i = 0; i < directions.Length; i++) {
			newPos = pos + directions [i];
			x = (int)newPos.x;
			y = (int)newPos.y;
			if (IsValidWorldPos (x, y) && singleton.world [x, y].hasPlant) {
				neighbours [i] = singleton.world [x, y].plant;
			} else {
				neighbours [i] = null;
			}
		}
		return neighbours;
	}

	public static bool IsValidWorldPos (int x, int y)
	{
		if (x >= 0 && y >= 0 && x < singleton.worldX && y < singleton.worldY) {
			return true;
		}
		return false;
	}
}

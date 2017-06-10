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

		FillInWorld ();
	}

	void FillInWorld ()
	{
		// hedge then path
		FillHedgeRow (0);
		FillPathRow (1);
		// now fill in the real "meat" of the crop farm
		// lol, crop farms don't have meat :P
		int rowEnd = worldX - 2;
		GenerateWorld (2, rowEnd, false);
		// path then hedge
		FillPathRow (worldX - 2);
		FillHedgeRow (worldX - 1);

		DistributePlants ();

	}


	void DistributePlants ()
	{
		// get a list of all the available plant coordinates
		List<Vector2> validCoords = GetValidCoordsForType (PlantType.corn);
	}

	List<Vector2> GetValidCoordsForType (PlantType typ)
	{
		List<Vector2> list = new List<Vector2> ();
		// now populate the world!
		for (int i = 0; i < worldX; i++) {
			for (int j = 0; j < worldY; j++) {
				// i want a functor and a filter here :s
				// welcome to the imperative version!
				switch (typ) {

				}
			}
		}
		return list;
	}

	
	void GenerateWorld (int rowNum, int rowEnd, bool needPath)
	{
		int rowsLeft = rowEnd - rowNum;
		if (needPath) {
			// create a path row
			FillPathRow (rowNum);
			GenerateWorld (rowNum + 1, rowEnd, false);
		}

		// special case for ending fields nicely
		if (rowsLeft <= 3) {
			FillCropRows (rowNum, rowsLeft);
			return;
		}
		int rowWidth = Random.Range (1, 3);

		FillCropRows (rowNum, rowWidth);
		GenerateWorld (rowNum + rowWidth, rowEnd, true);
	}

	void FillHedgeRow (int rowIndex)
	{
		for (int i = 0; i < worldY; i++) {
			world [rowIndex, i] = new WorldTile (TileType.hedge, rowIndex, i);
		}
	}

	void FillPathRow (int rowIndex)
	{
		// hedge then path
		world [rowIndex, 0] = new WorldTile (TileType.hedge, rowIndex, 0);
		world [rowIndex, 1] = new WorldTile (TileType.path, rowIndex, 1);

		// fill the path row...
		for (int i = 2; i < worldY - 2; i++) {
			world [rowIndex, i] = new WorldTile (TileType.path, rowIndex, i);
		}

		// path then hedge
		world [rowIndex, worldY - 2] = new WorldTile (TileType.path, rowIndex, worldY - 2);
		world [rowIndex, worldY - 1] = new WorldTile (TileType.hedge, rowIndex, worldY - 1);
	}

	void FillCropRows (int firstRowNum, int count)
	{
		for (int x = 0; x < count; x++) {
			int rowIndex = firstRowNum + x;
			// hedge then path
			world [rowIndex, 0] = new WorldTile (TileType.hedge, rowIndex, 0);
			world [rowIndex, 1] = new WorldTile (TileType.path, rowIndex, 1);

			// fill the path row...
			for (int i = 2; i < worldY - 2; i++) {
				world [rowIndex, i] = new WorldTile (TileType.plant, rowIndex, i);
			}

			// path then hedge
			world [rowIndex, worldY - 2] = new WorldTile (TileType.path, rowIndex, worldY - 2);
			world [rowIndex, worldY - 1] = new WorldTile (TileType.hedge, rowIndex, worldY - 1);
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

	public static WorldTile[] GetNeighbouringCropCells (Vector2 pos)
	{
		List<WorldTile> neighbours = new List<WorldTile> (directions.Length);
		int x, y;
		Vector2 newPos;
		for (int i = 0; i < directions.Length; i++) {
			newPos = pos + directions [i];
			x = (int)newPos.x;
			y = (int)newPos.y;
			if (IsValidWorldPos (x, y) && singleton.world [x, y].tileType == TileType.plant) {
				neighbours.Add (singleton.world [x, y]);
			}
		}
		return neighbours.ToArray ();
	}

	PlantType GetPlantInTile (int x, int y)
	{
		return world [x, y].plant.plantType;
	}

	public static bool IsValidWorldPos (int x, int y)
	{
		if (x >= 0 && y >= 0 && x < singleton.worldX && y < singleton.worldY) {
			return true;
		}
		return false;
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantManager : MonoBehaviour
{
	public static PlantManager singleton;
	WorldTile[,] world;
	public int worldX = 64;
	public int worldY = 64;

	public GameObject hedgePrefab;
	public GameObject pathPrefab;
	public GameObject cornPrefab;
	public GameObject potatoPrefab;
	public GameObject onionPrefab;
	public GameObject garlicPrefab;

	public int cropRowCount = 0;
	List<PathTile> pathTiles = new List<PathTile> ();

	// for t junction tiles, their forward is the "other" direction
	public GameObject tJuncPathPrefab;
	public GameObject linePathPrefab;


	// Use this for initialization
	void Start ()
	{
		cropRowCount = 0;
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


		// orient the paths
		OrientPaths ();

		StartCoroutine (DistributePlants ());

	}

	void OrientPaths ()
	{
		for (int i = 0; i < pathTiles.Count; i++) {
			OrientPathTile (pathTiles [i]);
		}
	}

	private bool isDistributing;

	IEnumerator DistributePlants ()
	{
		isDistributing = true;
		StartCoroutine (DistributeCorn ());
		while (isDistributing) {
			yield return new WaitForEndOfFrame ();
		}

		isDistributing = true;
		StartCoroutine (DistributePotato ());
		while (isDistributing) {
			yield return new WaitForEndOfFrame ();
		}

		isDistributing = true;
		StartCoroutine (DistributeOnion ());
		while (isDistributing) {
			yield return new WaitForEndOfFrame ();
		}

		isDistributing = true;
		StartCoroutine (DistributeGarlic ());
		while (isDistributing) {
			yield return new WaitForEndOfFrame ();
		}

		isDistributing = true;
		StartCoroutine (FillRemaining ());
		while (isDistributing) {
			yield return new WaitForEndOfFrame ();
		}
		Debug.Log ("Done Spawning");
		// BEGIN!
		GameManager.isPlaying = true;
	}

	IEnumerator FillRemaining ()
	{
		Debug.Log ("Filling left-overs...");
		isDistributing = true;
		// fill the rest with corn
		while (true) {
			// get a list of all the available plant coordinates
			List<Vector2> validCoords = GetValidCoordsForType (PlantType.any);
			// stop spawning if no coords
			if (validCoords.Count == 0) {
				break;
			}
			int patchSize = 10;
			// reduce corn to spawn
			// generate a random spawn place
			int randomPlace = Random.Range (0, validCoords.Count);
			FloodFillCorn (validCoords [randomPlace], patchSize, 1000);
			yield return new WaitForEndOfFrame ();
		}
		isDistributing = false;
	}

	IEnumerator DistributeCorn ()
	{
		Debug.Log ("Planting corn...");
		isDistributing = true;
		// how many corn seeds
		int numSeeds = cropRowCount + 3;

		while (numSeeds > 0) {
			// get a list of all the available plant coordinates
			List<Vector2> validCoords = GetValidCoordsForType (PlantType.corn);
			// stop spawning if no coords
			if (validCoords.Count == 0) {
				break;
			}
			int patchSize = Random.Range (7, 13);
			// reduce corn to spawn
			numSeeds--;
			// generate a random spawn place
			int randomPlace = Random.Range (0, validCoords.Count);
			FloodFillCorn (validCoords [randomPlace], patchSize, 1000);
			yield return new WaitForEndOfFrame ();
		}
		isDistributing = false;
	}

	IEnumerator DistributePotato ()
	{
		Debug.Log ("Planting potato...");
		isDistributing = true;
		// how many seeds
		int numSeeds = 3 * cropRowCount;
		while (numSeeds > 0) {
			// get a list of all the available plant coordinates
			List<Vector2> validCoords = GetValidCoordsForType (PlantType.potato);
			// stop spawning if no coords
			if (validCoords.Count == 0) {
				break;
			}
			// reduce corn to spawn
			numSeeds--;
			// generate a random spawn place
			int randomPlace = Random.Range (0, validCoords.Count);
			SpawnPotato (validCoords [randomPlace]);
			yield return new WaitForEndOfFrame ();
		}
		isDistributing = false;
	}

	IEnumerator DistributeOnion ()
	{
		Debug.Log ("Planting onions...");
		isDistributing = true;
		// how many seeds
		int numSeeds = cropRowCount;
		while (numSeeds > 0) {
			// get a list of all the available plant coordinates
			List<Vector2> validCoords = GetValidCoordsForType (PlantType.onion);
			// stop spawning if no coords
			if (validCoords.Count == 0) {
				break;
			}
			int patchSize = Random.Range (1, 4);
			// reduce corn to spawn
			numSeeds--;
			// generate a random spawn place
			int randomPlace = Random.Range (0, validCoords.Count);
			FloodFillOnion (validCoords [randomPlace], patchSize, 1000);
			yield return new WaitForEndOfFrame ();
		}
		isDistributing = false;
	}

	IEnumerator DistributeGarlic ()
	{
		Debug.Log ("Planting garlic...");
		isDistributing = true;
		// how many seeds
		int numSeeds = cropRowCount;
		while (numSeeds > 0) {
			// get a list of all the available plant coordinates
			List<Vector2> validCoords = GetValidCoordsForType (PlantType.garlic);
			// stop spawning if no coords
			if (validCoords.Count == 0) {
				break;
			}
			// reduce corn to spawn
			numSeeds--;
			// generate a random spawn place
			int randomPlace = Random.Range (0, validCoords.Count);
			SpawnGarlic (validCoords [randomPlace]);
			yield return new WaitForEndOfFrame ();
		}
		isDistributing = false;
	}

	void FloodFillCorn (Vector2 pos, int amount, float radius)
	{
		LinkedList<WorldTile> cells = GetEmptyCropCellsFloodFill ((int)pos.x, (int)pos.y, radius, amount);
		while (cells.Count > 0) {
			// get each cell
			WorldTile cell = cells.First.Value;
			cells.RemoveFirst ();
			// make corn
			GameObject obj = Instantiate (cornPrefab, new Vector3 (cell.x, 0, cell.y), Quaternion.identity);
			PlantBase plant = obj.GetComponent<PlantBase> ();
			plant.gridPos = new Vector2 (cell.x, cell.y);
			// set corn in world
			world [cell.x, cell.y].plant = plant;
		}
	}

	void SpawnPotato (Vector2 pos)
	{
		WorldTile cell = world [(int)pos.x, (int)pos.y];
		GameObject obj = Instantiate (potatoPrefab, new Vector3 (cell.x, 0, cell.y), Quaternion.identity);
		PlantBase plant = obj.GetComponent<PlantBase> ();
		plant.gridPos = new Vector2 (cell.x, cell.y);
		// set potato in world
		world [cell.x, cell.y].plant = plant;

	}

	void SpawnGarlic (Vector2 pos)
	{
		WorldTile cell = world [(int)pos.x, (int)pos.y];
		GameObject obj = Instantiate (garlicPrefab, new Vector3 (cell.x, 0, cell.y), Quaternion.identity);
		PlantBase plant = obj.GetComponent<PlantBase> ();
		plant.gridPos = new Vector2 (cell.x, cell.y);
		// set potato in world
		world [cell.x, cell.y].plant = plant;

	}

	void FloodFillOnion (Vector2 pos, int amount, float radius)
	{
		LinkedList<WorldTile> cells = GetEmptyCropCellsFloodFill ((int)pos.x, (int)pos.y, radius, amount);
		while (cells.Count > 0) {
			// get each cell
			WorldTile cell = cells.First.Value;
			cells.RemoveFirst ();
			// make onion
			GameObject obj = Instantiate (onionPrefab, new Vector3 (cell.x, 0, cell.y), Quaternion.identity);
			PlantBase plant = obj.GetComponent<PlantBase> ();
			plant.gridPos = new Vector2 (cell.x, cell.y);
			// set plant
			world [cell.x, cell.y].plant = plant;
		}
	}

	// returns a breadth first flood fill
	LinkedList<WorldTile> GetEmptyCropCellsFloodFill (int x, int y, float radius, int amount = int.MaxValue)
	{
		int count = 0;
		float radiusSqr = radius * radius;
		LinkedList<WorldTile> openList = new LinkedList<WorldTile> ();
		LinkedList<WorldTile> closedList = new LinkedList<WorldTile> ();
		// add to open list
		openList.AddLast (world [x, y]);
		// get neighbours
		while (openList.Count > 0) {
			// grab the first and mark it
			WorldTile currentTile = openList.First.Value;
			currentTile.marked = true;
			closedList.AddLast (currentTile);
			openList.RemoveFirst ();

			count++;
			if (count >= amount) {
				break;
			}

			// get the neighbours and add to open list
			WorldTile[] neighbours = GetNeighbouringCropCells (currentTile.x, currentTile.y);
			for (int i = 0; i < neighbours.Length; i++) {

				// skip if marked
				if (neighbours [i].marked) {
					continue;
				}

				// we've visited this tile
				neighbours [i].marked = true;

				// skip if plant
				if (neighbours [i].hasPlant) {
					continue;
				}
				// skip if too far
				int xDiff = neighbours [i].x - x;
				int yDiff = neighbours [i].y - y;

				float dist = xDiff * xDiff + yDiff * yDiff;
				if (dist > radiusSqr) {
					continue;
				}
				// only add if a valid crop
				openList.AddLast (neighbours [i]);
			}
		}

		ClearMarkedTiles ();
		return closedList;
	}

	LinkedList<WorldTile> GetEmptyCropCellsInRadius (int x, int y, float radius)
	{
		LinkedList<WorldTile> tiles = new LinkedList<WorldTile> ();
		int lowX = (int)Mathf.Max (0, x - radius);
		int lowY = (int)Mathf.Max (0, y - radius);
		int upX = (int)Mathf.Min (worldX, x + radius);
		int upY = (int)Mathf.Min (worldY, y + radius);
		float radiusSqr = radius * radius;

		for (int i = lowX; i < upX; i++) {
			for (int j = lowY; j < upY; j++) {
				// skip if marked
				if (world [i, j].marked) {
					continue;
				}

				// skip if not a plant
				if (world [i, j].tileType != TileType.plant) {
					continue;
				}

				// skip if already occupied
				if (world [i, j].hasPlant) {
					continue;
				}

				// sqr dist
				float sqrDist = (x - i) * (x - i) + (y - j) * (y - j);
				// check if within radius
				if (sqrDist < radiusSqr) {
					tiles.AddLast (world [i, j]);
				}
			}
		}

		return tiles;
	}

	void SpawnPlantAtTile (WorldTile t, PlantType typ)
	{
		GameObject prefab = cornPrefab;

		switch (typ) {
		case PlantType.corn:
			prefab = cornPrefab;
			break;
		case PlantType.potato:
			prefab = potatoPrefab;
			break;
		}
		GameObject newPlant = Instantiate (prefab, new Vector3 (t.x, 0, t.y), Quaternion.identity);
		PlantBase plantScript = newPlant.GetComponent<PlantBase> ();
		plantScript.gridPos = new Vector2 (t.x, t.y);
		t.plant = plantScript;
	}

	List<Vector2> GetValidCoordsForType (PlantType typ)
	{
		List<Vector2> list = new List<Vector2> ();
		// now populate the world!
		for (int i = 0; i < worldX; i++) {
			for (int j = 0; j < worldY; j++) {
				// i want a functor and a filter here :s
				bool isValid = false;

				// skip if not a plant
				if (world [i, j].tileType != TileType.plant) {
					continue;
				}

				// can't spawn a plant on top of another plant
				if (world [i, j].hasPlant) {
					continue;
				}

				// welcome to the imperative version!
				switch (typ) {
				case PlantType.corn:
					isValid = CheckCornValid (i, j);
					break;
				case PlantType.potato:
					isValid = CheckPotatoValid (i, j);
					break;
				case PlantType.onion:
					isValid = CheckOnionValid (i, j);
					break;
				case PlantType.garlic:
					isValid = CheckGarlicValid (i, j);
					break;
				case PlantType.shroom:
					isValid = CheckPotatoValid (i, j);
					break;
				case PlantType.any:
					isValid = true;
					break;
				}

				if (isValid) {
					list.Add (new Vector2 (i, j));
				}
			}
		}
		return list;
	}

	bool CheckCornValid (int x, int y)
	{
		WorldTile[] neighbours = GetNeighbouringCropCells (new Vector2 (x, y));
		for (int i = 0; i < neighbours.Length; i++) {
			// skip if there is a plant, this ensures corn has gaps in it for other plants
			if (neighbours [i].hasPlant) {
				continue;
			}
		}
		return true;
	}

	bool CheckPotatoValid (int x, int y)
	{
		WorldTile[] neighbours = GetNeighbouringCropCells (x, y);
		// skip if 4 neighbours, potatos are only found on row edges
		if (neighbours.Length == 4) {
			return false;
		}
		for (int i = 0; i < neighbours.Length; i++) {
			// skip if no plant
			if (!neighbours [i].hasPlant) {
				continue;
			}

			if (neighbours [i].plant.plantType == PlantType.potato) {
				return false;
			}
		}
		return true;
	}

	bool CheckOnionValid (int x, int y)
	{
		WorldTile[] neighbours = GetNeighbouringCropCells (x, y);
		// skip if 4 neighbours, onions are only found on row edges
		if (neighbours.Length == 4) {
			return false;
		}
		return true;
	}

	bool CheckGarlicValid (int x, int y)
	{
		WorldTile[] neighbours = GetNeighbouringCropCells (x, y);
		// skip if less than 3 neighbours, garlic only appears in big patches
		if (neighbours.Length < 3) {
			return false;
		}
		int cornCount = 0;
		for (int i = 0; i < neighbours.Length; i++) {
			// skip if no plant
			if (!neighbours [i].hasPlant) {
				continue;
			}
			// don't spawn near too much corn
			if (neighbours [i].plant.plantType == PlantType.corn) {
				cornCount++;
				continue;
			}
			// can't spawn next to other garlic
			if (neighbours [i].plant.plantType == PlantType.garlic) {
				return false;
			}
		}
		// if more than 2 corn neighbors? we can't cause enough carnage
		if (cornCount > 2) {
			return false;
		}

		return true;
	}

	void GenerateWorld (int rowNum, int rowEnd, bool needPath)
	{
		int rowsLeft = rowEnd - rowNum;
		if (needPath) {
			// create a path row
			FillPathRow (rowNum);
			GenerateWorld (rowNum + 1, rowEnd, false);
			return;
		}

		// special case for ending fields nicely
		if (rowsLeft <= 3) {
			FillCropRows (rowNum, rowsLeft);
			return;
		}
		int rowWidth = Random.Range (1, 4);

		FillCropRows (rowNum, rowWidth);
		GenerateWorld (rowNum + rowWidth, rowEnd, true);
	}

	void CreateHedge (int x, int y)
	{
		world [x, y] = new WorldTile (TileType.hedge, x, y);
		Instantiate (hedgePrefab, new Vector3 (x, 0, y), Quaternion.identity);
	}

	void CreatePath (int x, int y)
	{
		world [x, y] = new WorldTile (TileType.path, x, y);
		GameObject path = Instantiate (pathPrefab, new Vector3 (x, 0, y), Quaternion.identity);
		PathTile script = path.GetComponent<PathTile> ();
		script.SetPos (x, y);
		pathTiles.Add (script);
	}

	void CreatePlant (int x, int y)
	{
		world [x, y] = new WorldTile (TileType.plant, x, y);
	}

	void FillHedgeRow (int rowIndex)
	{
		for (int i = 0; i < worldY; i++) {
			CreateHedge (rowIndex, i);
		}
	}

	void FillPathRow (int rowIndex)
	{
		// hedge then path
		CreateHedge (rowIndex, 0);
		CreatePath (rowIndex, 1);

		// fill the path row...
		for (int i = 2; i < worldY - 2; i++) {
			CreatePath (rowIndex, i);
		}

		// path then hedge
		CreatePath (rowIndex, worldY - 2);
		CreateHedge (rowIndex, worldY - 1);
	}

	void FillCropRows (int firstRowNum, int count)
	{
		for (int x = 0; x < count; x++) {
			int rowIndex = firstRowNum + x;
			// hedge then path
			CreateHedge (rowIndex, 0);
			CreatePath (rowIndex, 1);

			// fill the plant row...
			for (int i = 2; i < worldY - 2; i++) {
				CreatePlant (rowIndex, i);
			}

			// path then hedge
			CreatePath (rowIndex, worldY - 2);
			CreateHedge (rowIndex, worldY - 1);
		}

		cropRowCount++;
	}

	// Update is called once per frame
	void Update ()
	{
		
	}

	void ClearMarkedTiles ()
	{
		for (int i = 0; i < worldX; i++) {
			for (int j = 0; j < worldY; j++) {
				world [i, j].marked = false;
			}
		}
	}

	static Vector2[] directions = { Vector2.up, Vector2.down, Vector2.left, Vector2.right };

	public void OrientPathTile (PathTile tile)
	{
		Vector2 pos = tile.GetPosition ();
		bool[] nextPaths = new bool[4];
		int validDirs = 0;
		for (int i = 0; i < directions.Length; i++) {
			int x = (int)(pos + directions [i]).x;
			int y = (int)(pos + directions [i]).y;
			if (IsValidWorldPos (x, y) && world [x, y].tileType == TileType.path) {
				validDirs++;
				nextPaths [i] = true;
			}
		}

		Vector3 finalDir = Vector3.zero;

		// 3 directions, therefore T junction
		if (validDirs == 3) {
			for (int i = 0; i < nextPaths.Length; i++) {
				if (nextPaths [i]) {
					finalDir += (Vector3)directions [i];
				}
			}
			tile.SetType (PathType.tJunc);
		}
		// 2 directions
		else if (validDirs == 2) {
			
			if (nextPaths [0] && nextPaths [1]) {
				finalDir = directions [0];
				tile.SetType (PathType.line);
			} else if (nextPaths [2] && nextPaths [3]) {
				finalDir = directions [2];
				tile.SetType (PathType.line);
			} else {
				for (int i = 0; i < nextPaths.Length; i++) {
					if (nextPaths [i]) {
						finalDir += (Vector3)directions [i];
					}
				}
				tile.SetType (PathType.corner);
			}

		}

		// move Y to Z
		finalDir.z = finalDir.y;
		finalDir.y = 0;
		tile.SetLookDirection (finalDir);

		// work out is linear?
	}

	/// <summary>
	/// Gets the neighbours for a position.
	/// </summary>
	/// <returns>A length 4 array of neighbours, they can be null</returns>
	/// <param name="pos">Position.</param>
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

	static Vector2[] directions8 = {
		Vector2.up,
		Vector2.down,
		Vector2.left,
		Vector2.right,
		Vector2.up + Vector2.right,
		Vector2.up + Vector2.left,
		Vector2.down + Vector2.right,
		Vector2.down + Vector2.left
	};

	/// <summary>
	/// Gets the neighbours for a position.
	/// </summary>
	/// <returns>A length 4 array of neighbours, they can be null</returns>
	/// <param name="pos">Position.</param>
	public static PlantBase[] Get8NeighboursForPosition (Vector2 pos)
	{
		PlantBase[] neighbours = new PlantBase[directions8.Length];

		int x, y;
		Vector2 newPos;
		for (int i = 0; i < directions8.Length; i++) {
			newPos = pos + directions8 [i];
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

	/// <summary>
	/// Gets the neighbouring crop cells.
	/// </summary>
	/// <returns>The neighbouring crop cells, no null values.</returns>
	/// <param name="pos">Position.</param>
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

	/// <summary>
	/// Gets the neighbouring cells. No null values
	/// </summary>
	/// <returns>The neighbouring cells.</returns>
	/// <param name="pos">Position.</param>
	public static WorldTile[] GetNeighbouringCells (Vector2 pos)
	{
		List<WorldTile> neighbours = new List<WorldTile> (directions.Length);
		int x, y;
		Vector2 newPos;
		for (int i = 0; i < directions.Length; i++) {
			newPos = pos + directions [i];
			x = (int)newPos.x;
			y = (int)newPos.y;
			if (IsValidWorldPos (x, y)) {
				neighbours.Add (singleton.world [x, y]);
			}
		}
		return neighbours.ToArray ();
	}

	public static WorldTile[] GetNeighbouringCropCells (int x, int y)
	{
		Vector2 pos = new Vector2 (x, y);
		return GetNeighbouringCropCells (pos);
	}

	public WorldTile GetTile (int x, int y)
	{
		return world [x, y];
	}

	public static bool IsValidWorldPos (int x, int y)
	{
		if (x >= 0 && y >= 0 && x < singleton.worldX && y < singleton.worldY) {
			return true;
		}
		return false;
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileData
{
}

public class FieldTileStore : MonoBehaviour
{

	public static FieldTileStore singleton;

	public Mesh linePathMesh;
	public Mesh cornerPathMesh;
	public Mesh tJuncPathMesh;

	void Awake ()
	{
		singleton = this;
	}

}

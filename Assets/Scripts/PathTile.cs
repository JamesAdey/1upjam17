using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PathType
{
	line,
	corner,
	tJunc
}

public class PathTile : MonoBehaviour
{
	int x;
	int y;
	MeshFilter meshFilter;
	MeshCollider meshCollider;

	public void SetPos (int x, int y)
	{
		this.x = x;
		this.y = y;
	}
	// Use this for initialization
	void Awake ()
	{
		meshFilter = GetComponent<MeshFilter> ();
		meshCollider = GetComponent<MeshCollider> ();
	}


	public void SetLookDirection (Vector3 dir)
	{
		transform.rotation = Quaternion.LookRotation (dir, Vector3.up);
	}

	public void SetType (PathType typ)
	{
		switch (typ) {
		case PathType.line:
			meshFilter.sharedMesh = FieldTileStore.singleton.linePathMesh;
			break;
		case PathType.corner:
			meshFilter.sharedMesh = FieldTileStore.singleton.cornerPathMesh;
			break;
		case PathType.tJunc:
			meshFilter.sharedMesh = FieldTileStore.singleton.tJuncPathMesh;
			break;
		}
		meshCollider.sharedMesh = meshFilter.sharedMesh;
		meshCollider.convex = false;
	}

	public Vector2 GetPosition ()
	{
		return new Vector2 (x, y);
	}
}

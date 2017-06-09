using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantPotato : PlantBase
{
	public Transform eyeCone;

	public bool hasPlayer;

	public float angle = 90;
	public float range = 3;

	// Use this for initialization
	void Start ()
	{
		plantType = PlantType.potato;
	}
	
	// Update is called once per frame
	void Update ()
	{
		
	}

	void FixedUpdate ()
	{
		hasPlayer = CheckIfPlayerInCone ();
	}

	bool CheckIfPlayerInCone ()
	{
		// get player position
		Vector3 playerPos = MitePlayer.GetPosition ();
		// check if within sphere
		Vector3 playerDisplacement = playerPos - eyeCone.position;
		if (playerDisplacement.sqrMagnitude > range) {
			return false;
		}
		// check if within angle
		if (Vector3.Angle (eyeCone.forward, playerDisplacement) > angle / 2) {
			hasPlayer = false;
			return false;
		}
		return true;
	}

	public override void OnPlantStanceChanged ()
	{
		// TODO fill this in
		// change graphics or something
	}
}

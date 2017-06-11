using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantPotato : PlantBase
{
	public Transform eyeCone;
	public Transform headBone;
	public Projector proj;
	public new SkinnedMeshRenderer renderer;
	public bool hasPlayer;

	public float angle = 90;
	public float range = 3;

	public float turnAngle = 70;
	public float turnTime = 2;
	Quaternion initialRot;
	Quaternion leftLookRot;
	Quaternion rightLookRot;
	float rotOffset;



	// Use this for initialization
	void Start ()
	{
		proj = GetComponentInChildren<Projector> ();
		renderer = GetComponentInChildren<SkinnedMeshRenderer> ();
		plantType = PlantType.potato;
		initialRot = headBone.localRotation;
		leftLookRot = initialRot * Quaternion.AngleAxis (turnAngle, Vector3.right);
		rightLookRot = initialRot * Quaternion.AngleAxis (-turnAngle, Vector3.right);
		rotOffset = Random.Range (0f, Mathf.PI);

		// turn us to face the nearest path
		WorldTile[] neighbours = PlantManager.GetNeighbouringCells (gridPos);
		for (int i = 0; i < neighbours.Length; i++) {
			if (neighbours [i].tileType == TileType.path) {
				transform.LookAt (new Vector3 (neighbours [i].x, 0, neighbours [i].y), Vector3.up);
				break;
			}
		}
	}
	
	// Update is called once per frame
	void Update ()
	{
	}

	void FixedUpdate ()
	{
		// do we have the player?
		if (CheckIfPlayerInCone ()) {
			if (GetStance () != PlantStance.friendly) {
				// SOUND THE ALARM!!!
				CreateAlarm ();
			}

		}

		float lerpAmount = Mathf.Sin ((Time.time + rotOffset) / turnTime);
		lerpAmount *= lerpAmount;
		headBone.localRotation = Quaternion.Lerp (leftLookRot, rightLookRot, lerpAmount);
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
		switch (GetStance ()) {
		case PlantStance.enemy:
			renderer.sharedMaterial = enemyMaterial;
			break;
		case PlantStance.friendly:
			renderer.sharedMaterial = friendlyMaterial;
			break;
		}
	}
}

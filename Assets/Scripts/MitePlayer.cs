﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MitePlayer : MonoBehaviour
{
	private static MitePlayer singleton;
	private Transform thisTransform;

	private Vector3 force;
	public float acceleration;
	private Rigidbody playerRB;
	public float topSpeed;

	private float nextAttackTime = 0f;

	// Use this for initialization
	void Start ()
	{
		thisTransform = this.transform;
		singleton = this;
		playerRB = this.GetComponent<Rigidbody> ();
		force = new Vector3 (0, 0, 0);
		acceleration = 8.0f;
		topSpeed = 8.0f;
	}
	
	// Update is called once per frame
	void Update ()
	{
		float forceX = Input.GetAxisRaw ("Horizontal") * acceleration;
		float forceZ = Input.GetAxisRaw ("Vertical") * acceleration;
		force.x = forceX;
		force.z = forceZ;
		if (playerRB.velocity.magnitude < topSpeed) {
			playerRB.AddRelativeForce (force, ForceMode.Acceleration);
		}
		if (Input.GetMouseButtonDown (0)) {
			if (Time.time > nextAttackTime) {
				LaunchInfection ();
				nextAttackTime = Time.time + 0.5f;
			}
		}
	}

	public void LaunchInfection ()
	{
		RaycastHit hit;
		if (Physics.Raycast (thisTransform.position, thisTransform.forward, out hit, 0.5f)) {
			Debug.Log ("Hit");
			hit.transform.root.SendMessage ("InfectPlant", SendMessageOptions.DontRequireReceiver);
		}
	}

	public static Vector3 GetPosition ()
	{
		return singleton.thisTransform.position;
	}
}

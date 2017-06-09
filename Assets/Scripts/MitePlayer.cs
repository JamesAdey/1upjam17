using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MitePlayer : MonoBehaviour
{
	private static MitePlayer singleton;
	private Transform thisTransform;

	// Use this for initialization
	void Start ()
	{
		thisTransform = this.transform;
		singleton = this;
	}
	
	// Update is called once per frame
	void Update ()
	{
		
	}

	public static Vector3 GetPosition ()
	{
		return singleton.thisTransform.position;
	}
}

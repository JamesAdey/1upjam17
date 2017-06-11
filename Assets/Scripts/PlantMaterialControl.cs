using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantMaterialControl : MonoBehaviour
{
	public Material friendlyMat;
	public Material enemyMat;
	new Renderer renderer;

	void Start ()
	{
		renderer = GetComponent<Renderer> ();
	}

	public void SetStance (PlantStance stance)
	{
		switch (stance) {
		case PlantStance.enemy:
			renderer.sharedMaterial = enemyMat;
			break;
		case PlantStance.friendly:
			renderer.sharedMaterial = friendlyMat;
			break;
		}
	}
}

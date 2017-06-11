using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantOnion : PlantBase
{

	public override void OnPlantStanceChanged ()
	{
		// SOUND THE ALARM!
		CreateAlarm ();
	}
}

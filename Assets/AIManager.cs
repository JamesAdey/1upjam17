using System.Collections;
using System.Collections.Generic;
using UnityEngine;

struct Alarm
{
	public Alarm (Vector3 pos, float dur)
	{
		position = pos;
		deadTime = Time.time + dur;
	}

	public Vector3 position;
	/// <summary>
	/// Time at which this alarm expires
	/// </summary>
	public float deadTime;
}

public class AIManager : MonoBehaviour
{
	public static AIManager singleton;

	LinkedList<Alarm> alarms = new LinkedList<Alarm> ();

	// Use this for initialization
	void Awake ()
	{
		singleton = this;
	}
	
	// Update is called once per frame
	void Update ()
	{
		// remove alarms after their duration
		LinkedListNode<Alarm> node = alarms.First;
		while (node != null) {
			Alarm al = alarms.First.Value;
			if (Time.time > al.deadTime) {
				alarms.Remove (node);
			}
			node = node.Next;
		}
	}

	void CreateAlarm (Vector3 pos, float duration)
	{
		Alarm newAlarm = new Alarm (pos, duration);
		alarms.AddLast (newAlarm);
	}

	public static void SpawnAlarmAtPosition (Vector3 pos, float duration)
	{
		singleton.CreateAlarm (pos, duration);
	}
}

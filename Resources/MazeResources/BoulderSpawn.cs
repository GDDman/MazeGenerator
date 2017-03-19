using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoulderSpawn : MonoBehaviour {

	GameObject trigger;
	TriggerBoulders tb;

	void Start() {
		// Spawn a boulder in the spawn location every 5 seconds
		InvokeRepeating ("StartBoulders", 2.0f, 5.0f);
		trigger = GameObject.Find ("BoulderTrigger");
		tb = (TriggerBoulders) trigger.transform.GetComponent ("TriggerBoulders");
	}

	void StartBoulders() {

		// Check if the trigger has been activated
		if (tb.GetTriggerred ()) {
			GameObject boulder = Instantiate (Resources.Load ("MazeResources/Boulder")) as GameObject;
		}
	}
}

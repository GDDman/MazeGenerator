using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerBoulders : MonoBehaviour {

	// Simple hitbox to trigger the boulders starting to roll
	private bool triggered;

	void Start() {
		triggered = false;
	}

	void OnTriggerEnter(Collider other) {
		if (other.gameObject.tag == "Player") {
			triggered = true;
		}
	}

	public bool GetTriggerred() {
		return triggered;
	}
}

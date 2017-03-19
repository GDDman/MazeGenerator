using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadOut : MonoBehaviour {

	// Exits cave
	void OnTriggerEnter(Collider other) {
		if (other.gameObject.tag == "Player") {
			SceneManager.LoadScene ("Outside 2");
		}
	}
}

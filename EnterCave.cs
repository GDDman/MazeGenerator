using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnterCave : MonoBehaviour {

	// Enters Maze
	void OnTriggerEnter(Collider other) {
		if (other.gameObject.tag == "Player") {
			SceneManager.LoadScene ("Maze");
		}
	}
}

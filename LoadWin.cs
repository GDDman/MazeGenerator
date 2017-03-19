using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadWin : MonoBehaviour {

	// Go to Win Screen
	void OnTriggerEnter(Collider other) {
		if (other.gameObject.tag == "Player") {
			SceneManager.LoadScene ("WinScreen");
		}
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Playagain : MonoBehaviour {

	// Loads from beginning
	public void OnClick() {
		SceneManager.LoadScene ("Outside");
	}
}

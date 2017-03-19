using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorOpen : MonoBehaviour {

	void OnTriggerEnter(Collider other) {
		if (other.gameObject.tag == "Player") {
			GameObject data = GameObject.Find ("MazeGenerator");
			GenerateMaze gm = (GenerateMaze) data.transform.GetComponent<GenerateMaze> ();
			// Check if player has picked up all the keys
			if (gm.GetKeys () >= 3) {
				// Make door invisible/uncollidable
				Transform alc = transform.parent;
				Transform door = alc.transform.FindChild ("Door");
				BoxCollider b = door.transform.GetComponent<BoxCollider> ();
				b.enabled = false;
				MeshRenderer m = door.transform.GetComponent<MeshRenderer> ();
				m.enabled = false;
			}
		}
	}

}

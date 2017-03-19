using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetKey : MonoBehaviour {

	void OnTriggerEnter(Collider other) {
		if (other.gameObject.tag == "Player") {
			GameObject data = GameObject.Find ("MazeGenerator");
			GenerateMaze gm = (GenerateMaze) data.transform.GetComponent<GenerateMaze> ();
			gm.incKeys ();

			// Remove key
			Transform parent = transform.parent;
			Transform light = parent.transform.FindChild ("light");
			MeshCollider b = transform.GetComponent<MeshCollider> ();
			MeshRenderer m = transform.GetComponent<MeshRenderer> ();
			Light l = light.transform.GetComponent<Light> ();
			b.enabled = false;
			m.enabled = false;
			l.enabled = false;
		}
	}
}

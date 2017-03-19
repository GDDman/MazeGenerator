using System;

namespace AssemblyCSharpfirstpass
{
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;
	using UnityStandardAssets.Utility;

	// Class for the player's single bullet. It can only be fired forward. 
	public class Bullet : MonoBehaviour {

		// True if the bullet is in the air. 
		public bool b_fired;
		public Vector3 velocity;
		public Vector3 pos;
 
		// Use this for initialization
		void Start () {
			b_fired = false;
			velocity = new Vector3 (0, 0, 0);
			pos = transform.position;
		}

		public bool getFired ()
		{
			return b_fired;
		}

		// Update is called once per frame
		void Update () {
			// Enable halo only if fired
			Behaviour b = (Behaviour)GetComponent ("Halo");
			MeshRenderer mr = GetComponent<MeshRenderer> ();
			if (b_fired) {
				b.enabled = true;
				mr.enabled = true;
				pos = pos + velocity;
				transform.position = pos;
			} else {
				b.enabled = false;
				mr.enabled = false;
				pos = transform.position;
			}
		}

		public void Fire () {
			// set velocity based on the trajectory at that time
			b_fired = true;
			velocity = new Vector3 (0, 0, 1);
			velocity = transform.rotation * velocity;
		}

		void OnTriggerEnter(Collider other) {
			if (b_fired && other.gameObject.tag != "BulletThrough") {
				// bullet can go through some things (like invisible walls)
				// Otherwise it collides and returns to player
				b_fired = false;
				velocity = new Vector3 (0, 0, 0);
				transform.localPosition = new Vector3 (0, 1, 0);
			}
		}


	}

}


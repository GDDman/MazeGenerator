using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// Movement and collisions of Boulder
public class BoulderControl : MonoBehaviour {

	GenerateMaze gm;
	float speed;
	// in rotations per second
	float rotation;
	GenerateMaze.Direction facing;

	void Start () {
		GameObject obj = GameObject.Find ("MazeGenerator");
		gm = (GenerateMaze)obj.transform.GetComponent<GenerateMaze> ();
		facing = GenerateMaze.Direction.Forward;
		speed = 13f;
		rotation = 300;
		GameObject bspawn = GameObject.Find ("BoulderSpawner");
		transform.position = bspawn.transform.position;
	}

	// Update Movement and Rotation animation
	void Update () {
		float time = Time.deltaTime;
		float dist = time * speed;
		float angle = time * rotation;
		switch (facing) {
		case GenerateMaze.Direction.Forward:
			transform.position += new Vector3 (0, 0, dist);
			transform.rotation = Quaternion.Euler (angle, 0, 0)*transform.rotation;
			break;
		case GenerateMaze.Direction.Left:
			transform.position += new Vector3 (-dist, 0, 0);
			transform.rotation = Quaternion.Euler (0, 0, angle)*transform.rotation;
			break;
		case GenerateMaze.Direction.Right:
			transform.position += new Vector3 (dist, 0, 0);
			transform.rotation = Quaternion.Euler (0, 0, -angle)*transform.rotation;
			break;
		case GenerateMaze.Direction.Back:
			transform.position += new Vector3 (0, 0, -dist);
			transform.rotation = Quaternion.Euler (-angle, 0, 0) * transform.rotation;
			break;
		}

	}

	void OnTriggerEnter(Collider other) {

		if (other.gameObject.tag == "Player") {
			// Player dies
			SceneManager.LoadScene ("LoseScreen");
		}
		if (other.gameObject.tag == "BoulderHit") {

			// Boulder Hits a wall and must change direction (the wall has a parameter of Halo that decides left or right)

			// Reposition Boulder in center of tile
			float xf = transform.position.x / 10;
			float zf = transform.position.z / 10;
			int x = (int) Mathf.Round (xf);
			int z = (int) Mathf.Round (zf);

			x = x * 10;
			z = z * 10;

			transform.position = new Vector3((float)x, transform.position.y, (float)z);

			// Get which way to turn from wall
			Transform segment = other.gameObject.transform.parent;
			Behaviour right = (Behaviour) segment.transform.GetComponent ("Halo");
			if (!right.enabled)
				facing = GenerateMaze.ChangeDirection (facing, GenerateMaze.Direction.Left);
			else
				facing = GenerateMaze.ChangeDirection (facing, GenerateMaze.Direction.Right);
		}
		// Boulder is shot or hits the end door
		if (other.gameObject.tag == "BoulderDestroy") {
			Destroy (gameObject);
		}
	}
}

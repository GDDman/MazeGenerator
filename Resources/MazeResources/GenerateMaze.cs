using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GenerateMaze : MonoBehaviour {

	static int mazeLength = 15;
	static Vector3 initialcoords = new Vector3(0, 0, 2);

	// Coordinates are scaled down by 10, each segment piece is a 1 by 1 tile
	List<Vector3> occupied = new List<Vector3> ();
	int keyCount;

	Vector3 coords;

	public enum Direction {
		Forward, 
		Back, 
		Left, 
		Right
	}

	Direction facing;
		
	// Use this for initialization
	void Start () {

		coords = initialcoords;
		facing = Direction.Forward;

		keyCount = 0;

		// Adding intial constant hallway
		occupied.Add (new Vector3 (0, 0, 1));
		occupied.Add (new Vector3 (0, 0, 2));
		occupied.Add (new Vector3 (0, 0, 0));
		occupied.Add (new Vector3 (0, 0, -1));
		occupied.Add (new Vector3 (0, 0, -2));
		occupied.Add (new Vector3 (0, 0, -3));
		occupied.Add (new Vector3 (-1, 0, -3));
		occupied.Add (new Vector3 (-2, 0, -3));
		occupied.Add (new Vector3 (-3, 0, -3));
		occupied.Add (new Vector3 (-4, 0, -3));
		occupied.Add (new Vector3 (-4, 0, -4));
		occupied.Add (new Vector3 (-4, 0, -5));
		occupied.Add (new Vector3 (-4, 0, -6));

		// Hard code the direction of the first wall
		GameObject firstturn = GameObject.Find ("firsturn");
		Behaviour fdir = (Behaviour) firstturn.transform.GetComponent ("Halo");
		fdir.enabled = false;

		// Randomly select where to put alcoves
		int third = (int) Mathf.Floor (mazeLength / 3);
		int FirstAlcoveIndex = Random.Range (1, third-1);
		int SecondAlcoveIndex = Random.Range (third+1, 2 * third-1);
		int ThirdAlcoveIndex = Random.Range (2 * third+1, 3 * third-1);

		// Procedurally generate blocks in a unicursal maze, adding 3 alcoves.
		for (int i = 0; i < mazeLength; i++) {

			// Check if the next move is legal (no overlap with prvious maze segements)
			Vector3 leftshift = GetCoordShift (Direction.Left, false);
			Vector3 rightshift = GetCoordShift (Direction.Right, false);
			Vector3 forwardshift = GetCoordShift (Direction.Forward, false);

			Vector3 coordsleft = coords + leftshift;
			Vector3 coordsright = coords + rightshift;
			Vector3 coordsforward = coords + forwardshift;

			bool leftgood = CheckSurroundings (coordsleft);
			bool rightgood = CheckSurroundings (coordsright);
			bool forwardgood = CheckSurroundings (coordsforward);

			// Happens if the maze closes itself in, should not happen very often so just reload the scene
			if (!leftgood && !rightgood && !forwardgood) {
				Restart ();
			}
		
			// 0 if current tile is not connected to an alcove, 1, 2, 3, for the 3 alcoves
			int AlcoveFlag = 0;
			if (i == FirstAlcoveIndex)
				AlcoveFlag = 1;
			else if (i == SecondAlcoveIndex)
				AlcoveFlag = 2;
			else if (i == ThirdAlcoveIndex)
				AlcoveFlag = 3;

			int counter = 0;
			bool redo = false;
			while (true) { 

				// Safeguard for infinite loops
				if (counter > 100) {
					redo = true;
					break;
				}
				counter++;

				// Decide randomly which way to go
				// Slightly Higher chance to go forward: l = 3/10, r = 3/10, f = 4/10
				int rand = Random.Range (0, 10);
				// Left
				if (rand < 3) {
					if (leftgood) {
						// The tile is an alcove
						if (AlcoveFlag > 0) {
							try {
								// Delete the wall where the alcove is and instantiate alcove + key
								Direction AlcDir = PickRandomDir (Direction.Left);
								CreateAlcove (AlcDir, false);
								GameObject segment = PlaceBlock (Direction.Left);
								if (AlcDir == Direction.Forward) {
									Transform wall = segment.transform.FindChild ("l2");
									BoxCollider bc = (BoxCollider) wall.transform.GetComponent<BoxCollider> ();
									bc.enabled = false;
									MeshRenderer mr = (MeshRenderer) wall.transform.GetComponent<MeshRenderer> ();
									mr.enabled = false;
								} else if (AlcDir == Direction.Right) {
									Transform wall = segment.transform.FindChild("r1 (2)");
									BoxCollider bc = (BoxCollider) wall.transform.GetComponent<BoxCollider> ();
									bc.enabled = false;
									MeshRenderer mr = (MeshRenderer) wall.transform.GetComponent<MeshRenderer> ();
									mr.enabled = false;
								}
							} catch {
								print ("err left");
							}
						} else {
							PlaceBlock (Direction.Left);
						}
						break;
					}
				} 
				// Right
				else if (rand < 6) {
					if (rightgood) {
						if (AlcoveFlag > 0) {
							try {
								Direction AlcDir = PickRandomDir (Direction.Right);
								CreateAlcove (AlcDir, true);
								GameObject segment = PlaceBlock (Direction.Right);
								if (AlcDir == Direction.Forward) {
									Transform wall = segment.transform.FindChild ("bot2 (2)");
									BoxCollider bc = (BoxCollider) wall.transform.GetComponent<BoxCollider> ();
									bc.enabled = false;
									MeshRenderer mr = (MeshRenderer) wall.transform.GetComponent<MeshRenderer> ();
									mr.enabled = false;
								} else if (AlcDir == Direction.Left) {
									Transform wall = segment.transform.FindChild ("l1");
									BoxCollider bc = (BoxCollider) wall.transform.GetComponent<BoxCollider> ();
									bc.enabled = false;
									MeshRenderer mr = (MeshRenderer) wall.transform.GetComponent<MeshRenderer> ();
									mr.enabled = false;
								}
							} catch {
								print ("err right");
							}
						} else {
							PlaceBlock (Direction.Right);
						}
						break;
					}

				}
				// Forward
				else {
					if (forwardgood) {
						if (AlcoveFlag > 0) {
							try {
								Direction AlcDir = PickRandomDir (Direction.Forward);
								CreateAlcove (AlcDir, true);
								GameObject segment = PlaceBlock (Direction.Forward);
								if (AlcDir == Direction.Left) {
									Transform wall = segment.transform.FindChild ("l1");
									BoxCollider bc = (BoxCollider) wall.transform.GetComponent<BoxCollider> ();
									bc.enabled = false;
									MeshRenderer mr = (MeshRenderer) wall.transform.GetComponent<MeshRenderer> ();
									mr.enabled = false;
								} else if (AlcDir == Direction.Right) {
									Transform wall = segment.transform.FindChild ("r1");
									BoxCollider bc = (BoxCollider) wall.transform.GetComponent<BoxCollider> ();
									bc.enabled = false;
									MeshRenderer mr = (MeshRenderer) wall.transform.GetComponent<MeshRenderer> ();
									mr.enabled = false;
								}
							} catch {
								print ("err forward");
							}
						} else {
							PlaceBlock (Direction.Forward);
						}
						break;
					}
				}
			}
			if (redo)
				Restart ();
		}
		// Create door and hallway at the end of the maze.
		GameObject endhall = Instantiate (Resources.Load ("MazeResources/EndAlcove")) as GameObject;
		Quaternion hallrotation = Quaternion.Euler (0, GetFacingRotation(), 0);
		Vector3 offset = new Vector3 (0, 0, -1);
		endhall.transform.rotation = hallrotation;
		endhall.transform.position = 10 * (coords + offset);

	}

	// number of keys the player has
	public int GetKeys() {
		return keyCount;
	}

	// increment keys
	public void incKeys() {
		keyCount++;
	}
		
	// Load the scene again, catch-all for weird errors
	void Restart() {
		SceneManager.LoadScene ("Maze");
	}

	// Alcove needs a direction to face and a direction for the boulder to bounce off of
	void CreateAlcove(Direction d, bool right) {
		Vector3 alcovepos = GetAlcoveCoordinates (d);
		int alcoverotation = GetAlcoveRotation (d);
		if (occupied.Contains (alcovepos))
			Restart ();
		occupied.Add (alcovepos);
		GameObject alcove = Instantiate (Resources.Load ("MazeResources/Alcove")) as GameObject;
		alcove.transform.rotation = Quaternion.Euler (0, alcoverotation, 0);
		alcove.transform.position = 10 * alcovepos;

		// Key is in alcove
		GameObject key = Instantiate (Resources.Load ("MazeResources/key")) as GameObject;
		Vector3 offset = new Vector3 (0, 1.5f, 0);
		key.transform.position = alcove.transform.position + offset;

		// Set boulder bounce direction
		Behaviour dir = (Behaviour) alcove.transform.GetComponent ("Halo");
		dir.enabled = right;
	}

	// Makes sure alcove opens the right way
	int GetAlcoveRotation(Direction d) {
		switch (facing) {
		case Direction.Forward:
			if (d == Direction.Left)
				return -90;
			if (d == Direction.Right)
				return 90;
			break;
		case Direction.Left:
			if (d == Direction.Forward)
				return -90;
			if (d == Direction.Left)
				return 180;
			break;
		case Direction.Right:
			if (d == Direction.Forward)
				return 90;
			if (d == Direction.Right)
				return 180;
			break;
		case Direction.Back:
			if (d == Direction.Forward)
				return 180;
			if (d == Direction.Left)
				return 90;
			if (d == Direction.Right)
				return -90;
			break;
		}
		return 0;
	}

	// Get shifted coordinates of alcove 
	Vector3 GetAlcoveCoordinates(Direction d) {
		// Not really sure why this offset is needed
		Vector3 offset = new Vector3 (0, 0, -1);
		switch (facing) {
		case Direction.Forward:
			if (d == Direction.Forward)
				return coords + (new Vector3 (0, 0, 2)) + offset;
			if (d == Direction.Left)
				return coords + (new Vector3 (-1, 0, 0)) + offset;
			if (d == Direction.Right)
				return coords + (new Vector3 (1, 0, 0)) + offset;
			break;
		case Direction.Left:
			if (d == Direction.Forward)
				return coords + (new Vector3 (-2, 0, 0)) + offset;
			if (d == Direction.Left)
				return coords + (new Vector3 (0, 0, -1)) + offset;
			if (d == Direction.Right)
				return coords + (new Vector3 (0, 0, 1)) + offset;
			break;
		case Direction.Right:
			if (d == Direction.Forward)
				return coords + (new Vector3 (2, 0, 0)) + offset;
			if (d == Direction.Left)
				return coords + (new Vector3 (0, 0, 1)) + offset;
			if (d == Direction.Right)
				return coords + (new Vector3 (0, 0, -1)) + offset;
			break;
		case Direction.Back:
			if (d == Direction.Forward)
				return coords + (new Vector3 (0, 0, -2)) + offset;
			if (d == Direction.Left)
				return coords + (new Vector3 (1, 0, 0)) + offset;
			if (d == Direction.Right)
				return coords + (new Vector3 (-1, 0, 0)) + offset;
			break;
		}
		return new Vector3 ();
	}

	// Random Direction for alcove based on the segment type it is attached to
	Direction PickRandomDir(Direction d) {
		int rand = Random.Range (0, 2);
		if (d == Direction.Forward) {
			if (rand == 1)
				return Direction.Left;
			else
				return Direction.Right;
		}
		if (d == Direction.Left) {
			if (rand == 1)
				return Direction.Forward;
			else
				return Direction.Right;
		}
		if (d == Direction.Right) {
			if (rand == 1)
				return Direction.Forward;
			else
				return Direction.Left;
		}
		return d;
	}

	// Check if a tile has any segments around it
	bool CheckSurroundings(Vector3 pos) {

		Vector3 left = pos + (new Vector3 (-1, 0, 0));
		Vector3 right = pos + (new Vector3 (1, 0, 0));
		Vector3 forward = pos + (new Vector3 (0, 0, 1));
		Vector3 back = pos + (new Vector3 (0, 0, -1));

		Vector3 leftfor = pos + (new Vector3 (-1, 0, 1));
		Vector3 rightfor = pos + (new Vector3 (1, 0, 1));
		Vector3 leftback = pos + (new Vector3 (-1, 0, -1));
		Vector3 rightback = pos + (new Vector3 (1, 0, -1));

		if (occupied.Contains (left))
			return false;
		if (occupied.Contains (right))
			return false;
		if (occupied.Contains (forward))
			return false;
		if (occupied.Contains (back))
			return false;
		if (occupied.Contains (leftfor))
			return false;
		if (occupied.Contains (rightfor))
			return false;
		if (occupied.Contains (leftback))
			return false;
		if (occupied.Contains (rightback))
			return false;
		
		return true;
	}

	// Create a segement, hall, leftturn or rightturn
	GameObject PlaceBlock(Direction dir) {
		GameObject segment = null;
		Vector3 coordshift = new Vector3 ();
		Vector3 tempshift = new Vector3 ();
		Vector3 newcoords = new Vector3 ();
		int rotate = 0;

		// Rotate for global rotation
		rotate += GetFacingRotation ();
		tempshift = GetTempShift (dir);
		// Get the correct piece
		switch (dir) 
		{
		case Direction.Forward:
			segment = Instantiate (Resources.Load ("MazeResources/hall")) as GameObject;
			break;
		case Direction.Left:
			segment = Instantiate (Resources.Load ("MazeResources/rightturn")) as GameObject;
			rotate += 90;
			Behaviour lscript = (Behaviour)segment.transform.GetComponent ("Halo");
			lscript.enabled = false;
			break;
		case Direction.Right:
			segment = Instantiate (Resources.Load ("MazeResources/rightturn")) as GameObject;
			Behaviour rscript = (Behaviour)segment.transform.GetComponent ("Halo");
			rscript.enabled = true;
			break;
		}
		// translate to real coordinates
		rotate = rotate % 360;
		segment.transform.rotation = Quaternion.Euler (0, (float)rotate, 0);
		newcoords = coords + tempshift;
		segment.transform.position = 10*newcoords;
		coordshift = GetCoordShift (dir, true);
		coords = coords + coordshift;
		return segment;
	}

	// Necessary offset
	Vector3 GetTempShift(Direction d) {
		switch (facing) {
		case Direction.Left:
			return new Vector3 (-1, 0, -1);
		case Direction.Back:
			return new Vector3 (0, 0, -2);
		case Direction.Right:
			return new Vector3 (1, 0, -1);
		default:
			return new Vector3 ();
		}
	}

	// Rotate for global direction
	int GetFacingRotation() {
		switch (facing) {
		case Direction.Forward:
			return 0;
		case Direction.Left:
			return -90;
		case Direction.Back:
			return 180;
		case Direction.Right:
			return 90;
		default:
			return 0;
		}
	}
		
	// Figure out where you will end up after putting a piece down (used to check valildity)
	Vector3 GetCoordShift(Direction d, bool change) {
		Vector3 shift = new Vector3 ();
		if (d == Direction.Forward) {
			switch (facing) {
			case Direction.Forward:
				shift = new Vector3 (0, 0, 2);
				if (change) occupied.Add (coords + (new Vector3 (0, 0, 1)));
				break;
			case Direction.Right:
				shift = new Vector3 (2, 0, 0);
				if (change) occupied.Add (coords + (new Vector3 (1, 0, 0)));
				break;
			case Direction.Left:
				shift = new Vector3 (-2, 0 ,0);
				if (change) occupied.Add (coords + (new Vector3 (-1, 0, 0)));
				break;
			case Direction.Back:
				shift = new Vector3 (0, 0, -2);
				if (change) occupied.Add (coords + (new Vector3 (0, 0, -1)));
				break;
			}
		} else if (d == Direction.Right) {
			switch (facing) {
			case Direction.Forward:
				shift = new Vector3 (2, 0, 1);
				if (change) {
					occupied.Add (coords + (new Vector3(0, 0, 1)));
					occupied.Add (coords + (new Vector3 (1, 0, 1)));
				}
				break;
			case Direction.Right:
				shift = new Vector3 (1, 0, -2);
				if (change) {
					occupied.Add (coords + (new Vector3(1, 0, 0)));
					occupied.Add (coords + (new Vector3 (1, 0, -1)));
				}
				break;
			case Direction.Left:
				shift = new Vector3 (-1, 0 ,2);
				if (change) {
					occupied.Add (coords + (new Vector3(-1, 0, 0)));
					occupied.Add (coords + (new Vector3 (-1, 0, 1)));
				}
				break;
			case Direction.Back:
				shift = new Vector3 (-2, 0, -1);
				if (change) {
					occupied.Add (coords + (new Vector3(0, 0, -1)));
					occupied.Add (coords + (new Vector3 (-1, 0, -1)));
				}
				break;
			}
		} else if (d == Direction.Left) {
			switch (facing) {
			case Direction.Forward:
				shift = new Vector3 (-2, 0, 1);
				if (change) {
					occupied.Add (coords + (new Vector3(0, 0, 1)));
					occupied.Add (coords + (new Vector3 (-1, 0, 1)));
				}
				break;
			case Direction.Right:
				shift = new Vector3 (1, 0, 2);
				if (change) {
					occupied.Add (coords + (new Vector3(1, 0, 0)));
					occupied.Add (coords + (new Vector3 (1, 0, 1)));
				}
				break;
			case Direction.Left:
				shift = new Vector3 (-1, 0 ,-2);
				if (change) {
					occupied.Add (coords + (new Vector3(-1, 0, 0)));
					occupied.Add (coords + (new Vector3 (-1, 0, -1)));
				}
				break;
			case Direction.Back:
				shift = new Vector3 (2, 0, -1);
				if (change) {
					occupied.Add (coords + (new Vector3(0, 0, -1)));
					occupied.Add (coords + (new Vector3 (1, 0, -1)));
				}
				break;
			}
		} else {
			return shift;
		}
		if (change) {
			occupied.Add (coords + (shift));
			facing = ChangeDirection (facing, d);
		}
		return shift;
	}

	// d is turn left or right
	public static Direction ChangeDirection(Direction current, Direction d) {
		if (d == Direction.Left) {
			switch (current) 
			{
			case Direction.Forward:
				return Direction.Left;				
			case Direction.Left:
				return Direction.Back;
			case Direction.Back:
				return Direction.Right;
			case Direction.Right:
				return Direction.Forward;
			}
		}
		if (d == Direction.Right) {
			switch (current) 
			{
			case Direction.Forward:
				return Direction.Right;
			case Direction.Left:
				return Direction.Forward;
			case Direction.Back:
				return Direction.Left;
			case Direction.Right:
				return Direction.Back;
			}
		}
		return current;
	}

}

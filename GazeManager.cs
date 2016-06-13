using UnityEngine;
using System.Collections;

/// <summary>
/// A class to manager the user's gaze
///		Projects a raycast forward in order to find where the user is looking.
/// </summary>
public class GazeManager : MonoBehaviour {

	// Reference an object to be used as a cursor.
	public Transform cursor;

	// Reference to the main camera, which will be the user's head position.
	public Transform cam;

	// How far the gaze raycast should go.
	public float distance = 3f;

	/// <summary>
	/// Method called when this object is first created.
	/// </summary>
	void Start () {

	}
	
	/// <summary>
	/// Called every frame in which this object is active.
	/// </summary>
	void Update () {
		//Raycast from the camera's position forward.
		RaycastHit hit;

		//If the raycast hit...
		if (Physics.Raycast(cam.position, cam.forward, out hit, distance))
		{
			//Place the cursor at the hit location.
			cursor.position = hit.point;

			//Orient the cursor to the normal of the hit object.
			cursor.forward = hit.normal;

		}

		//Otherwise...
		else
		{
			cursor.position = cam.position + cam.forward * distance;
		}

		//Place the cursor at the furthest range along your gaze.

	}
}

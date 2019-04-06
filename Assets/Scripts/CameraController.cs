using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

	public float leftOffset = 7.0f;
	public float rightOffset = 6.0f;
	
	private Camera cam;
	private List<GameObject> robots;

	// Start is called before the first frame update
	void Start() {
		cam = Camera.main;
		GameObject[] robotsArray = GameObject.FindGameObjectsWithTag("Friend");
		robots = new List<GameObject>(robotsArray);
	}

	// Update is called once per frame
	void Update() {
		CheckCameraPosition(MaisDistante());
	}

	private float MaisDistante() {
		float x = float.MinValue;

		if (robots.Count == 0) 
			return cam.transform.position.x;
		
		foreach (GameObject robot in robots) {
			if (robot == null) {
				robots.Remove(robot);
				return MaisDistante();
			}
			else if (robot.transform.position.x > x)
				x = robot.transform.position.x;
		}

		return x;
	}

	private void CheckCameraPosition(float x) {
		float posDiff = x - cam.transform.position.x;
		if ((-1.0f * posDiff) > leftOffset || posDiff > rightOffset)
			MoveCamera(posDiff);
	}

	private void MoveCamera(float x) {
		float movement = x + ((x < 0) ? leftOffset : (-1 * rightOffset));
		Vector3 newPos = cam.transform.position;
		newPos.x += movement;

		cam.transform.position = Vector3.Lerp(cam.transform.position, newPos, 0.1f);
	}
}

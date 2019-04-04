using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

	public float leftOffset = 7.0f;
	public float rightOffset = 6.0f;
	
	private Camera cam;
	private GameObject[] robots;

	// Start is called before the first frame update
	void Start() {
		cam = Camera.main;
		robots = GameObject.FindGameObjectsWithTag("Friend");
		Debug.Log("TAMANHO: " + robots.Length);
	}

	// Update is called once per frame
	void Update() {
		CheckCameraPosition(MaisDistante());
	}

	private float MaisDistante() {
		float x = float.MinValue;
		for (int i = 0; i < robots.Length; ++i) {
			if (robots[i].transform.position.x > x)
				x = robots[i].transform.position.x;
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

		cam.transform.position = newPos;
	}
}

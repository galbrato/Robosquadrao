using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Objetivo : MonoBehaviour {

	private Battle scriptBatalha;

	// Update is called once per frame
	void Start() {
		scriptBatalha = GameObject.Find("EventSystem").GetComponent<Battle>();
	}

	void OnTriggerEnter(Collider other) {
		if(other.gameObject.layer == 8) {
			scriptBatalha.aliadoNoFinal = true;
			scriptBatalha.VerificaVitoria();
		}
	}

	void OnTriggerExit(Collider other) {
		scriptBatalha.aliadoNoFinal = false;
	}
}

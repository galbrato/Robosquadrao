using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Battle : MonoBehaviour {

	public int battleLevel;

	public Text texto;
	
	void Awake() {
		//texto = GameObject.Find("Recompensas").GetComponent<Text>();
		if (battleLevel < Player.currentLevel)
			texto.enabled = false;
	}

	// Update is called once per frame
	void Update() {
		
	}

	public void BattleVictory() {
		if (battleLevel >= Player.currentLevel) {
			Recompensas();
		}

	}

	private void Recompensas() {
		Player.currentLevel++;
	}
}

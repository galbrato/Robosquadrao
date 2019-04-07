using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SquadSelector : GridLayoutResources {

	private Text[] nomesRobo;

	private void Awake() {
		nomesRobo = GetComponentsInChildren<Text>();
		AtualizaNomes();   // Modifica os texto com o nome dos robôs de cada botão

		elementQuantity = nomesRobo.Length;

		buttonsUnlocked = Player.robotsUnlocked;  // Lê o nivel atual do player para saber até onde já foi desbloqueado
		MakesButtonInteractible(gameObject, Mathf.Min(buttonsUnlocked, elementQuantity));   // Permite interação com os botões permitidos
	}

	public void AtualizaNomes() {
		for (int i = 0; i < nomesRobo.Length; i++) {
			try {
				RobotData robo = SaveSystem.LoadRobot(i);
				nomesRobo[i].text = robo.Name;
			} catch (System.IO.FileNotFoundException) {
				nomesRobo[i].text = "Novo Robô";
			}
		}
	}
}
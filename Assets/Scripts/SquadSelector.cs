using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/*
public class SquadSelector : GridLayoutResources {

	private GameObject[] configuracoes;
	//private ConfigRobos[] scripts;

	private void Awake() {
		gridLayout = GetComponent<GridLayoutGroup>();   // Pega o Grid Layout Group do próprio objeto em que o script está anexado

		elementQuantity = 6;    // Quantidade máxima de robôs que pode serdesbloqueada =)

		//configuracoes = GetConfigs();

		try {
			prefabs = AddElementsToGridLayout(gridLayout, elementPrefab, elementQuantity);    // Chama a função para adicionar os elementos ao Grid
		} catch (ArgumentNullException e) {
			Debug.Log("Argument cant be null! " + e);
		}

		//EditPrefabs(prefabs, configuracoes, scripts);   // Modifica os botões de nível adicionados ao grid

		buttonsUnlocked = Player.robotsUnlocked;  // Lê o nivel atual do player para saber até onde já foi desbloqueado
		MakesButtonInteractible(gameObject, Mathf.Min(buttonsUnlocked, elementQuantity));   // Permite interação com os botões permitidos
	}

	/* private GameObject[] GetConfigs() {
		GameObject parent = GameObject.Find("Configurações");
		scripts = parent.GetComponentsInChildren<ConfigRobos>(true);
		GameObject[] configs = new GameObject[scripts.Length];

		for (int i = 0; i < scripts.Length; i++) {
			configs[i] = scripts[i].gameObject;
		}

		return configs;
	}

	protected override void EditPrefabs(GameObject[] objetos) {
		for (int i = 0; i < objetos.Length; i++) {

			Button botao = objetos[i].GetComponent<Button>();
			//GameObject temp = configs[i];
			//GameObject temp = configs[i];
			//ConfigRobos scriptTemp = scripts[i];
			botao.onClick.AddListener(delegate {
				//temp.SetActive(true);
				//scriptTemp.Load(); 
			});    // Adiciona o evento para ativar a tela de config

		}
	}
}
*/
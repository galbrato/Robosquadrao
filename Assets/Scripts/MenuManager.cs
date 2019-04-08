using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour {

	void Awake() {
		if (SceneManager.GetActiveScene().name == "MainMenu")
			BotaoLoad();
	}

	public void BotaoLoad(){
		try {
			Button continuar = GameObject.Find("Continuar").GetComponent<Button>();
			continuar.interactable = SaveSystem.PlayerDataExists();

			if (!continuar.interactable) {
				try {
					ConfirmationMenu novoJogo = GameObject.Find("Novo Jogo").GetComponent<ConfirmationMenu>();
					novoJogo.needsConfirmation = false;
				} catch(System.NullReferenceException) {
					Debug.LogError("ERRO! Não existe botão de novo jogo nessa cena!");
				}
			}
		} catch(System.NullReferenceException) {
			Debug.LogError("ERRO! Não existe botão de continuar nessa cena!");
		}
	}

	public void LoadGame() {
		Player.Load();
	}

	public void NewGame() {
		Player.CreateNewSave();
	}

	public void SaveGame() {
		Debug.Log("Salvando o jogo!");
		Player.Save();
	}

	public void QuitGame() {
		Application.Quit();
	}

	public void ChangeScene(string sceneName) {
		Debug.Log("Changing to scene " + sceneName);
		SceneManager.LoadScene(sceneName);
	}

	public void SwitchEnable(GameObject gameObj) {
		gameObj.SetActive(!gameObj.activeSelf);
	}
}

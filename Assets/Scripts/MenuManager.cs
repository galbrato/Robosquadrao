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
		GameObject botao_GO = GameObject.Find("Continuar");	// Game Object do botão de continuar
		if (botao_GO == null) {
			Debug.Log("ERRO! Não existe botão de continuar nessa cena");
			return;
		}
		Button botao = botao_GO.GetComponent<Button>();
		botao.interactable = SaveSystem.PlayerDataExists();
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

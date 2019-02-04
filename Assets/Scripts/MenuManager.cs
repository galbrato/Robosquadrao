using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour {

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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Player {

	public static int currentLevel = 1;
	public static int robotsUnlocked = 1;

	public static void Save() {
		SaveSystem.SavePlayer();
	}

	public static void Load() {
		try {
			PlayerData data = SaveSystem.LoadPlayer();

			currentLevel = data.currentLevel;
			robotsUnlocked = data.robotsUnlocked;
		}
		catch (System.IO.FileNotFoundException) {
			CreateNewSave();
		}
	}

	public static void CreateNewSave() {
		currentLevel = 1;
		robotsUnlocked = 1;
	}
}

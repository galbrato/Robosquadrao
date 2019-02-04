using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData {

	public int currentLevel;
	public int robotsUnlocked;

	public PlayerData () {
		currentLevel = Player.currentLevel;
		robotsUnlocked = Player.robotsUnlocked;
	}
}

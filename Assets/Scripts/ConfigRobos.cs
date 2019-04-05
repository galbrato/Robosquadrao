using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text.RegularExpressions;

public class ConfigRobos : MonoBehaviour {

	public string robotName {
		set {
			if (value == "") {	// Caso a string seja vazia, coloca o nome padrão :D
				LoadDefaultName();
			} else {
				realRobotName = value;
				inputField.text = realRobotName;
			}
		}
		get {
			return realRobotName;
		}
	}
	public int robotNumber;

	private string defaultName = "Robô";
	private InputField inputField;
	private string realRobotName;

	private void Awake() {
		inputField = GetComponentInChildren<InputField>();
		if (inputField == null) {
			Debug.Log("Error! InputField couldn't be found!");
		}

		string numero = Regex.Replace(gameObject.name, "[^0-9]", "");
		robotNumber = int.Parse(numero);

		defaultName += robotNumber;
		//Debug.Log("Nome padrão: " + defaultName);
	}

	private void OnEnable() {
		Load();
	}

	public void Save() {
		//SaveSystem.SaveRobot(this);
	}

	public void Load() {

		//Debug.Log("Loading Robot Config");
		try {
			RobotData data = SaveSystem.LoadRobot(robotNumber);

			robotName = data.Name;
		} catch (System.IO.FileNotFoundException){
			CreateNewSave();
		}
	}

	public void CreateNewSave() {
		robotName = defaultName;
	}

	public void LoadDefaultName() {
		realRobotName = defaultName;
		inputField.text = defaultName;
	}
}

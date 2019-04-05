using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveSystem {
	
	public static void SavePlayer () {
		BinaryFormatter formatter = new BinaryFormatter();

		string path = Path.Combine(Application.persistentDataPath, "player.data");
		FileStream stream = new FileStream(path, FileMode.Create);

		PlayerData data = new PlayerData();

		formatter.Serialize(stream, data);
		stream.Close();
	}

	public static PlayerData LoadPlayer () {
		string path = Path.Combine(Application.persistentDataPath, "player.data");

		if (!File.Exists(path))
			throw new FileNotFoundException("Player data not found");

		BinaryFormatter formatter = new BinaryFormatter();
		FileStream stream = new FileStream(path, FileMode.Open);

		PlayerData data = formatter.Deserialize(stream) as PlayerData;
		stream.Close();

		return data;
	}

	public static void SaveRobot(RobotData robo) {
		string fileName = "Robot" + robo.Id + ".data";
		string path = Path.Combine(Application.persistentDataPath, fileName);

		BinaryFormatter formatter = new BinaryFormatter();
		FileStream stream = new FileStream(path, FileMode.Create);

		//RobotData data = new RobotData(robo);

		formatter.Serialize(stream, robo);
		stream.Close();
	}

	public static RobotData LoadRobot(int robotNumber) {
		string fileName = "Robot" + robotNumber + ".data";
		string path = Path.Combine(Application.persistentDataPath, fileName);
        //Debug.Log("caminh: " + Application.persistentDataPath);

        if (!File.Exists(path))
			throw new FileNotFoundException("Player data not found");

		BinaryFormatter formatter = new BinaryFormatter();
		FileStream stream = new FileStream(path, FileMode.Open);

		RobotData data = formatter.Deserialize(stream) as RobotData;
		stream.Close();

		return data;
	}

}

using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;


public class TestSaveScript : MonoBehaviour {
    public Data dt;

    public Text ClicksTxt;
    public Text NamesTxt;

    public InputField inputName;

    private void Start() {
        //dt = new Data();
        dt.namelist = new List<nomi>();
    }

    private void Update() {
        if (Input.GetMouseButtonDown(0)) {
            dt.clicks++;
            UpdateInterface();
        }
        Debug.Log(inputName.text);
    }

    public void addName() {
        if (inputName.text == "") {
            Debug.Log("INPUT VAZIO");
        } else {
            Debug.Log(inputName.text);
            dt.namelist.Add(new nomi(inputName.text, dt.clicks));
            UpdateInterface();
        }
    }

    public void SaveStateMachine() {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/guacamole.dat");

        bf.Serialize(file, dt);
        file.Close();
    }

    public void LoadStateMachine() {
        if(File.Exists(Application.persistentDataPath + "/guacamole.dat")) {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/guacamole.dat", FileMode.Open);
            dt = (Data)bf.Deserialize(file);
            file.Close();
            UpdateInterface();
        }
    }

    void UpdateInterface() {
        ClicksTxt.text = "#" + dt.nome + " " + dt.clicks;
        string concat = "Lista de Nome:\n";
        foreach (nomi item in dt.namelist) {
            concat += item.nomee + "\n";
        }
        NamesTxt.text = concat;
    }
}


[Serializable]
public class Data {
    public string nome;
    public int clicks;
    public List<nomi> namelist;
    //public GameObject State;
}
[Serializable]
public class nomi {
    public string nomee;
    public int id;
    

    public nomi(string n, int c) {
        nomee = n;
        id = c;
    }
}
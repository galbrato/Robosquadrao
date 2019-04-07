using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Battle : MonoBehaviour {

	public int battleLevel;

	public static Vector3 Objetivo;
	public static Vector3 Inicio;
	
    public List<GameObject> Friends;
    public List<GameObject> Enemies;

	public bool destruirInimigos;
	public bool chegarFinal;

    public bool inimigosMortos = false;
	public bool aliadoNoFinal = false;

	private Text texto;
    private GameObject telaVitoria;
    private GameObject telaDerrota;
    private SphereCollider colliderObjetivo;

	void Awake() {
		Inicio = GameObject.Find("Inicio").transform.position;

		GameObject gameObjectObjetivo = GameObject.Find("Objetivo");
		Objetivo = gameObjectObjetivo.transform.position;
        colliderObjetivo = gameObjectObjetivo.GetComponent<SphereCollider>();

		GameObject canvas = GameObject.Find("Battle_Canvas");
		telaDerrota = canvas.transform.GetChild(1).gameObject;
		telaVitoria = canvas.transform.GetChild(2).gameObject;

		texto = telaVitoria.transform.GetChild(2).GetComponent<Text>();
		if (battleLevel < Player.currentLevel)
			texto.enabled = false;

        Friends.AddRange(GameObject.FindGameObjectsWithTag("Friend"));
        Enemies.AddRange(GameObject.FindGameObjectsWithTag("Enemy"));
	}

    void Start() {

        if(chegarFinal == true){
            colliderObjetivo.enabled = true;
        }
    }

	// Update is called once per frame
	void Update() {
        Friends.RemoveAll(x => x == null);
        Enemies.RemoveAll(x => x == null);
		if (Enemies.Count == 0) inimigosMortos = true;
        
		VerificaDerrota();
		VerificaVitoria();
	}

	private void Recompensas() {
		Player.currentLevel++;
	}

	public void Vitoria() {
		telaVitoria.SetActive(true);
		if (battleLevel >= Player.currentLevel) {
			Recompensas();
		}
	}

	public void VerificaVitoria() {
		if (chegarFinal) {
			if(destruirInimigos) {
				//if ((destruirInimigos && aliadoNoFinal && inimigosMortos) || aliadoNoFinal)
				if (aliadoNoFinal && inimigosMortos)
					Vitoria();
			} else {
				if (aliadoNoFinal)
					Vitoria();
			}
		} else {
			if (inimigosMortos) {
				Vitoria();
			}
		}
	}

	public void Derrota() {
		telaDerrota.SetActive(true);
	}

	public void VerificaDerrota() {
		if (Friends.Count == 0)		// Caso morram todos os robôs
			Derrota();
	}

}

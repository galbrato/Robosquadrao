using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Battle : MonoBehaviour {

	public int battleLevel;

	public GameObject allyPrefab;	// Prefab do robô aliado a ser spawnado

	public static Vector3 Objetivo;
	public static Vector3 Inicio;
	
    public List<RobotCode> Friends;
    public List<GameObject> Enemies;

	public bool destruirInimigos;
	public bool chegarFinal;
	public bool recuperarVidaAliados;

    public bool inimigosMortos = false;
	public bool aliadoNoFinal = false;
	public bool aliadosVidaCheia = false;

	private Text texto;
    private GameObject telaVitoria;
    private GameObject telaDerrota;
    private SphereCollider colliderObjetivo;
	private Vector3[] spawnPositions;

	void Awake() {
		// Descobre as posições para spawnar os robôs já desbloqueados
		try {
			MeshRenderer[] posRobo = GameObject.Find("AllySpawnPosition").GetComponentsInChildren<MeshRenderer>();
			spawnPositions = new Vector3[posRobo.Length];
			for (int i = 0; i < posRobo.Length; ++i)
				spawnPositions[i] = posRobo[i].transform.position;
		} catch (System.NullReferenceException) {
			Debug.LogError("ERRO! Cenário não possui o objeto AllySpawnPosition com as posições de spawn");
		}

		allyPrefab = (GameObject) Resources.Load("Robot/Robot");
		if (allyPrefab == null)
			Debug.LogError("ERRO! Não foi possível encontrar o prefab do robô!");
		
		SpawnaRobos(allyPrefab);
		
		GameObject gameObjectObjetivo = GameObject.Find("Objetivo");
        colliderObjetivo = gameObjectObjetivo.GetComponent<SphereCollider>();

		Objetivo = gameObjectObjetivo.transform.position;
		Inicio = GameObject.Find("Inicio").transform.position;

		GameObject canvas = GameObject.Find("Battle_Canvas");
		telaDerrota = canvas.transform.GetChild(1).gameObject;
		telaVitoria = canvas.transform.GetChild(2).gameObject;

		List<GameObject> amigos = new List<GameObject>();
        amigos.AddRange(GameObject.FindGameObjectsWithTag("Friend"));
        Enemies.AddRange(GameObject.FindGameObjectsWithTag("Enemy"));
		foreach(GameObject amigo in amigos) {
			Friends.Add(amigo.GetComponent<RobotCode>());
		}

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

		ConfereVidaAliados();
        
		VerificaDerrota();
		VerificaVitoria();
	}

	private void ConfiguraTelaVitoria() {
		texto = telaVitoria.transform.GetChild(2).GetComponent<Text>();
		if (battleLevel < Player.currentLevel) {
			texto.enabled = false;
			telaVitoria.transform.GetChild(3).gameObject.SetActive(false);
		}
	}

	private void SpawnaRobos(GameObject robo) {
		int limit = Mathf.Min(Player.robotsUnlocked, spawnPositions.Length);
		for (int i = 0; i < limit; ++i) {
			GameObject instancia = Instantiate(robo, spawnPositions[i], Quaternion.identity);
			RobotCode codigo = instancia.GetComponent<RobotCode>();
			codigo.myID = i;
			codigo.VidaAtual = codigo.VidaMax;
		}
	}

	private void ConfereVidaAliados() {
		foreach(RobotCode friend in Friends) {
			if (friend.VidaAtual < friend.VidaMax)
				return;
		}
		aliadosVidaCheia = true;
	}

	private void AplicaRecompensas() {
		Player.currentLevel++;
		switch(battleLevel) {
			case 1:
				break;
			case 2:
				break;
			case 3:
				break;
			case 4:
				Player.robotsUnlocked++;
				break;
            case 5:
                Player.robotsUnlocked++;
                break;
            case 6:
                break;
            default:
				break;
		}
	}

	public void Vitoria() {
		telaVitoria.SetActive(true);
		if (battleLevel >= Player.currentLevel) {
			AplicaRecompensas();
		}
	}

	public void VerificaVitoria() {
		if (recuperarVidaAliados) {
			if (aliadosVidaCheia)
				Vitoria();
		} else if (chegarFinal) {
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

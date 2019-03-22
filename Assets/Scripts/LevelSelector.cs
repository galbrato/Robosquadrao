using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Runtime.Serialization;
using System.Text.RegularExpressions;

public class LevelSelector : GridLayoutResources {

	public MenuManager menuManager;

	public GameObject Preview;

	public Row[] robos;
	public Button comecar;
	public Button cancelar;
	public GameObject robo_models;
	public GameObject content;
	public GameObject Objetivo;

	private int nonBattleScenes = 2;    // Quantidade de cenas da build que não são de batalha (como o "MainMenu" e o "Game")
	private List<Scene> scenes;

	private void Awake() {
		gridLayout = GetComponent<GridLayoutGroup>();   // Pega o Grid Layout Group do próprio objeto em que o script está anexado
		elementQuantity = SceneManager.sceneCountInBuildSettings - nonBattleScenes;

		scenes = GetBattleScenes(nonBattleScenes, elementQuantity);	// Pega todas as cenas de batalha

		try {
			prefabs = AddElementsToGridLayout(gridLayout, elementPrefab, elementQuantity);    // Chama a função para adicionar os elementos ao Grid
		} catch (ArgumentNullException e) {
			Debug.Log("Argument cant be null! " + e);
		}

		EditPrefabs(prefabs);	// Modifica os botões de nível adicionados ao grid

		buttonsUnlocked = Player.currentLevel;	// Lê o nivel atual do player para saber até onde já foi desbloqueado
		MakesButtonInteractible(gameObject, Mathf.Min(buttonsUnlocked, elementQuantity));   // Permite interação com os botões permitidos
	}

	/// <summary>
	/// Percorre a lista de cenas adicionadas à build, utilizando o índice no intervalo [start, end]
	/// </summary>
	/// <param name="start">Indice inicial para começar a pegar as cenas (incluso)</param>
	/// <param name="end">Indice final das cenas (excluso)</param>
	/// <returns>Lista com as cenas de batalha</returns>
	private List<Scene> GetBattleScenes(int start, int end) {
		List<Scene> cenas = new List<Scene>();
		for (int i = start; i < end; ++i) {
			cenas.Add(SceneManager.GetSceneByBuildIndex(i));	// Pega as cenas da build a partir do indice
		}

		return cenas;
	}

	/// <summary>
	/// Vincula as cenas de batalha ao seu respectivo botão de nível, para que o jogador vá para o local certo com a seleção de nível.
	/// Também atualiza o texto do botão para indicar a dificuldade da fase
	/// </summary>
	/// <param name="objetos">Vetor do prefab dos botões de nível já adicionados à grid</param>
	protected override void EditPrefabs(GameObject[] objetos) {
		for (int i = 0; i < objetos.Length; i++) {

			Button botao = objetos[i].GetComponent<Button>();
			string s = "Batalha " + (i + 1);		// Cria a string com o nome da cena de batalha
			botao.onClick.AddListener(delegate { 
				//menuManager.ChangeScene(s); 
				Preview.SetActive(true);
				comecar.onClick.AddListener(delegate(){menuManager.ChangeScene(s);});
				cancelar.onClick.AddListener(delegate(){
					foreach (Transform child in content.transform){
						Destroy(child.gameObject);
					}
					cancelar.gameObject.transform.parent.gameObject.SetActive(false);
				});
				
				String result = Regex.Match(s, @"\d+").Value;
				i = Int32.Parse(result) - 1;
				for(int j = 0; j < robos[i].descricao.Length; j++){
					GameObject aux = Instantiate(robo_models, new Vector3(0,0,0), Quaternion.identity, content.transform);
					
					aux.transform.GetChild(0).GetComponent<Image>().sprite = robos[i].imagem[j];
					aux.transform.GetChild(1).GetComponent<Text>().text = robos[i].descricao[j];
				}
				
				Objetivo.GetComponent<Text>().text = robos[i].objetivo;
			});	// Adiciona o evento de mudança de cena ao botão, para levar à batalha

			objetos[i].GetComponentInChildren<Text>().text = (i+1).ToString();	// Muda o texto do botão
		}
	}
}

[Serializable]
public struct Row{
	public Sprite[] imagem;
	[TextArea]
	public String[] descricao;
	public String objetivo;
}

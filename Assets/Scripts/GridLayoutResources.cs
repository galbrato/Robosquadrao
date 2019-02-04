using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GridLayoutResources: MonoBehaviour {

	public GameObject elementPrefab;

	protected GameObject[] prefabs;
	protected GridLayoutGroup gridLayout;
	protected int elementQuantity;        // Quantidade total de prefabs que serão spawnados
	protected int buttonsUnlocked;        // Quantidade de botões "desbloqueados" até o momento (deve ser lido do save do player)

	// Função para inicializar o GridLayoutGroup com uma quantidade "n" do objeto "prefab"
	public static GameObject[] AddElementsToGridLayout(GridLayoutGroup grid, GameObject prefab, int n) {

		// Tratamento de erro para verificar se os argumentos são nulos
		if (grid == null)
			throw new ArgumentNullException("Grid Layout Group");
		if (prefab == null)
			throw new ArgumentNullException("Prefab");

		GameObject[] prefabVector = new GameObject[n];

		for (int i = 0; i < n; i++) {
			prefabVector[i] = Instantiate(prefab, grid.transform);	// Cria novas cópias do prefab e os adiciona como filhos do grid
		}

		return prefabVector;
	}

	// Função para permitir que os "n" primeiros botões, filhos de "parent" sejam interagíveis.
	// ATENÇÃO!!!! LEMBRE-SE DE DEIXAR OS BOTÕES JÁ DESATIVADOS POR PADRÃO!
	public void MakesButtonInteractible(GameObject parent, int n) {

		if (parent == null)
			throw new ArgumentNullException("Parent");
		
		Button[] buttons = parent.GetComponentsInChildren<Button>();	// Pega o vetor de botões dos objetos filhos
		for (int i = 0; i < n; ++i) {
			buttons[i].interactable = true;
		}
	}

	// Declaração da função de editar os objetos adicionados ao Grid Layout Group. Deve ser sobrescrita
	//	nas classes filhas, de acordo com a necessidade
	protected virtual void EditPrefabs(GameObject[] objetos) {

	}
}

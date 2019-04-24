using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonTutorial : MonoBehaviour {

	public Button botao;

	public GameObject fundo;
	public Sprite spriteBotao;

	private GameObject canvas;
	private GameObject instanciaFundo;

	// Start is called before the first frame update
	void Start() {
		if (botao == null)
			Debug.LogWarning("ERRO! Referência ao botão do tutorial inexistente!");

		
		canvas = FindObjectOfType<Canvas> ().gameObject;

		AtivaTutorial();
	}

	// Update is called once per frame
	void Update() {
		
	}

	public void CompletaTutorial() {
		Destroy(instanciaFundo);
	}

	public void AtivaTutorial() {
		instanciaFundo = Instantiate(fundo, canvas.transform);
		GameObject instanciaBotao = Instantiate(botao.gameObject, instanciaFundo.transform);

		if (instanciaBotao.GetComponent<ButtonTutorial>() != null)
			Destroy(instanciaBotao.GetComponent<ButtonTutorial> ());

		instanciaBotao.GetComponent<Image> ().sprite = spriteBotao;
		instanciaBotao.GetComponent<Image> ().type = Image.Type.Simple;
		instanciaBotao.GetComponent<Button> ().onClick.AddListener(CompletaTutorial);
	}
}

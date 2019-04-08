using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class ConfirmationMenu : MonoBehaviour {

	private static GameObject prefab;	// Referência ao prefab da Tela de Confirmação (TdC)

	// >> Configurations of the screen
	[Range(0.0f, 1.0f)]
	public float anchorMin = 0.2f;	// Ajusta os valores X e Y min da âncora da TdC
	[Range(0.0f, 1.0f)]
	public float anchorMax = 0.8f;	// Ajusta os valores X e Y max da âncora da TdC
	public Sprite backgroundSprite;	// O sprite a ser adicionado à imagem de fundo
	public Color backgroundColor = Color.grey;	// Cor da imagem de fundo
	[TextArea]
	public string titleText = "Title";	// Título mostrado na TdC
	[TextArea]
	public string bodyText = "<Message>";	// Mensagem explicando sobre a confirmação
	// Configurations <<

	private Button firstButton;		// Referência ao botão qhe este script está acoplado
	private Button.ButtonClickedEvent confirmationEvent;	// Evento chamado pelo botão da TdC
	private Transform canvas;		// Reference to the canvas of the scene

	private GameObject instance = null;	// Referência à INSTÂNCIA da TdC
	private GameObject background_instance = null;	// Referência ao background

	public bool needsConfirmation = true;	// Bool para indicar se a TdC necessita ser aberta ou se
	// o botão terá seu evento normal
	
	void Awake() {
		if (prefab == null) {	// Caso o prefab ainda não tenha sido carregado
			prefab = (GameObject) Resources.Load("ConfirmationScreen");	// Recupera o recurso da TdC
			if (prefab == null)	// Caso não encontre o prefab, indica erro
				Debug.LogError("ERROR!! The resource ConfirmationScreen could not be loaded!");
		}
	}

	// Start is called before the first frame update
	void Start() {
		try {
			canvas = GameObject.FindGameObjectWithTag("Canvas").transform;
		} catch (System.NullReferenceException) {
			canvas = GameObject.Find("Canvas").transform;	// Get the canvas
		}
		
		ConfigButton();
	}

	public void CreateConfirmationMenu() {
		if (!needsConfirmation)			// Caso não necessite abrir a TdC
			confirmationEvent.Invoke();	// Apenas chama os eventos que haviam no botão
		else if (instance == null) {
			instance = Instantiate(prefab, canvas);
			foreach(Transform trans in instance.transform) {	// Percorre todos os filhos diretos da TdC
				if (trans.name == "Background")
					background_instance = trans.gameObject;
			}
			Configurations();
		} else
			instance.SetActive(true);
	}

	public void Configurations() {
		ConfigAnchors();
		ConfigBg();
		ConfigTexts();
		ConfigConfirmation();
	}

	/// <summary>
	/// Configura o botão ao qual este script está acoplado, removendo o evento que havia (salva em "confirmationEvent")
	/// e criando um novo para acionar a TdC
	/// </summary>
	private void ConfigButton() {
		firstButton = GetComponent<Button>();
		if (firstButton == null) {	// Caso não haja botões acoplados, retorna erro
			Debug.LogError("Error! This Game Object must have an Button attached to it!");
			return;
		}

		confirmationEvent = firstButton.onClick;	// Salva o evento que estava no botão para ser adicionado à TdC

		Button.ButtonClickedEvent newButtonEvent = new Button.ButtonClickedEvent();
		newButtonEvent.AddListener(CreateConfirmationMenu);	// Cria um novo evento para chamar a TdC

		firstButton.onClick = newButtonEvent;		// Now the button will call the confirmation menu =D
	}

	/// <summary>
	/// Configura as âncoras da tela
	/// </summary>
	private void ConfigAnchors() {
		if (anchorMin >= anchorMax) {
			Debug.Log("ANCHOR PROBLEM! Anchor Min must be lower than Anchor Max.");
			return;
		}
		RectTransform tranf = background_instance.GetComponent<RectTransform>();
		tranf.anchorMin = new Vector2(anchorMin, anchorMin);
		tranf.anchorMax = new Vector2(anchorMax, anchorMax);
	}

	/// <summary>
	/// Configura a imagem de fundo
	/// </summary>
	private void ConfigBg() {
		Image bg = background_instance.GetComponent<Image>();
		bg.color = backgroundColor;
		if (backgroundSprite != null)
			bg.sprite = backgroundSprite;
	}

	/// <summary>
	/// Configura os textos da TdC
	/// </summary>
	private void ConfigTexts() {
		Text title = null;		// Referência do titulo
		Text message = null;	// Referência da mensagem

		foreach(Transform trans in background_instance.transform) {	// Percorre todos os filhos diretos da TdC
			Text aux = trans.GetComponent<Text>();			// Pega o componente de texto
			if (aux != null && aux.name == "Título")		// Se o componente existir e for o título, salva a referência
				title = aux;
			else if (aux != null && aux.name == "Mensagem")	// O mesmo para a mensagem...
				message = aux;
		}
		
		if (title == null || message == null) {		// Caso não encontre alguma referência, retorna erro
			Debug.Log("ERRO!!!! Título e corpo de mensagem não encontrados");
			return;
		}

		title.text = titleText;		// Coloca os textos no lugar
		message.text = bodyText;
	}

	/// <summary>
	/// Configura o botão de confirmação da TdC
	/// </summary>
	private void ConfigConfirmation() {
		foreach(Transform trans in background_instance.transform) {	// Percorre todos os filhos diretos da TdC
			Button button = trans.GetComponent<Button>();	// Pega o componente de botão
			if (button != null && button.name == "Confirmar") {	// Caso seja o botão de confirmação
				button.onClick =  confirmationEvent;		// Coloca os antigos eventos do botão inicial
				button.onClick.AddListener(delegate {instance.SetActive(false); });	// e desativa a TdC quando confirmado
			}
		}
	}
}

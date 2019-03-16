using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class ConfirmationMenu : MonoBehaviour {

	public GameObject prefab;

	// >> Configurations
	[Range(0.0f, 1.0f)]
	public float anchorMin = 0.2f;
	[Range(0.0f, 1.0f)]
	public float anchorMax = 0.8f;
	public Sprite backgroundSprite;
	public Color backgroundColor = Color.grey;
	[TextArea]
	public string titleText = "Title";
	[TextArea]
	public string bodyText = "<Message>";
	public Button.ButtonClickedEvent buttonConfirmation;
	// Configurations <<

	private Transform canvas;
	private GameObject instance = null;

	// Start is called before the first frame update
	void Start() {
		canvas = GameObject.Find("Canvas").transform;
		ConfigAnchors();
		ConfigBg();
		ConfigTexts();
		ConfigConfirmation();
	}

	public void CreateConfirmationMenu() {
		if (instance == null)
			instance = Instantiate(prefab, canvas);
		else
			instance.SetActive(true);
	}

	private void ConfigAnchors() {
		if (anchorMin >= anchorMax) {
			Debug.Log("ANCHOR PROBLEM! Anchor Min must be lower than Anchor Max.");
			return;
		}
		RectTransform tranf = prefab.GetComponent<RectTransform>();
		tranf.anchorMin = new Vector2(anchorMin, anchorMin);
		tranf.anchorMax = new Vector2(anchorMax, anchorMax);
	}

	private void ConfigBg() {
		Image bg = prefab.GetComponent<Image>();
		bg.color = backgroundColor;
		if (backgroundSprite != null)
			bg.sprite = backgroundSprite;
	}

	private void ConfigTexts() {
		Text title = null;
		Text message = null;

		foreach(Transform trans in prefab.transform) {
			Text aux = trans.GetComponent<Text>();
			if (aux != null && aux.name == "Título")
				title = aux;
			else if (aux != null && aux.name == "Mensagem")
				message = aux;
		}
		
		if (title == null || message == null) {
			Debug.Log("ERRO!!!! Título e corpo de mensagem não encontrados");
			return;
		}

		title.text = titleText;
		message.text = bodyText;
	}

	// Changes the OnClick action of the button confirm to the action in "buttonConfirmation"
	// Next step is to get the event from the button this script is atached to and set it to the confirmation (also,
	// set the first button event to open the confirm interface)
	private void ConfigConfirmation() {
		foreach(Transform trans in prefab.transform) {
			Button button = trans.GetComponent<Button>();
			if (button != null && button.name == "Confirmar")
				button.onClick =  buttonConfirmation;
		}
	}
}

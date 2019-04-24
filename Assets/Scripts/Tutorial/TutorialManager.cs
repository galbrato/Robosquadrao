using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour {

	private static TutorialManager m_instance;
	public static TutorialManager instance {
		get {
			if (m_instance == null)
				m_instance = FindObjectOfType<TutorialManager>();

			if (m_instance == null)
				Debug.LogError("ERROR! Could not find any TutorialManager in the Scene!");

			return m_instance;
		}
	}

	public int currentOrder;

	// Start is called before the first frame update
	void Start() {
		
	}

	// Update is called once per frame
	void Update() {
		
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class Tabulation : MonoBehaviour {
    HorizontalLayoutGroup myLayout;
    // Start is called before the first frame update
    void Start() {
    }

    // Update is called once per frame
    void Update() {
        if (transform.parent.name != "Content") {
            return;
        }
        int NumberOfTabulations = 0;
        Transform CodeContent = transform.parent;
        int myLine = transform.GetSiblingIndex();
        for (int i = 0; i < myLine; i++) {
            if (CodeContent.GetChild(i).childCount > 0) {
                if (CodeContent.GetChild(i).GetChild(0).name.Contains("Se")) NumberOfTabulations++;
                else if (CodeContent.GetChild(i).GetChild(0).name.Contains("FimEntao")) NumberOfTabulations--;
            }
        }
        if (CodeContent.GetChild(myLine).childCount > 0 && CodeContent.GetChild(myLine).GetChild(0).name.Contains("FimEntao")) NumberOfTabulations--;
        if (myLayout == null && transform.childCount > 0) {
            myLayout = transform.GetChild(0).GetComponent<HorizontalLayoutGroup>();
        }
        if (myLayout != null) {
            myLayout.padding.left = 100 * NumberOfTabulations;
        }
    }
}

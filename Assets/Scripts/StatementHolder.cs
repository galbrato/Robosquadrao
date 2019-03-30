using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatementHolder : MonoBehaviour {
    public Statement _OriginalStatement;
    //public RectTransform _StatementUI;
    public CodeManager _CodeManager;

    public int LineNumber;
    // Start is called before the first frame update
    void Start() {
        _CodeManager = FindObjectOfType<CodeManager>();
    }

    // Update is called once per frame
    void Update() {

    }

    public void _Select() {
        _SelectForReal();
    }
    public bool _SelectForReal() {
        
        Debug.Log("Clico em " + name + " " + transform.childCount + " filhos");
        StatementHolder[] holders = GetComponentsInChildren<StatementHolder>();
        if (holders.Length > 1) {

            //Debug.Log(name + " filho:" + _StatementUI.name);
            //posicionar o cursor no meu filho se tiver espaço
            for (int i = 1; i < holders.Length; i++) {
                if (holders[i]._SelectForReal()) {
                    return true;
                }
            }
            _CodeManager.Cursor.rectTransform.SetParent(holders[1].transform);
            //Debug.Log("Cursor parent: " + _CodeManager.Cursor.rectTransform.parent.name);

        } else {
            //posicionar o cursor em mim
            _CodeManager.Cursor.rectTransform.SetParent(transform);
            return true;
        }
        return false;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class CursorBehaviour : MonoBehaviour{
    [SerializeField] float WinkDelay = 0.5f;
    private float _WinkDelayCounter;
    private Text _mText;
    // Start is called before the first frame update
    void Start(){
        _mText = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update(){
        _WinkDelayCounter += Time.deltaTime;
        if (_WinkDelayCounter >= WinkDelay) {
            _WinkDelayCounter = 0;
            if (_mText.text == "|") {
                _mText.text = " ";
            } else {
                _mText.text = "|";
            }
        }
    }
    private void OnDestroy() {
        Debug.Log("AAAAAAAAAAAAAAAAAAAAAAAAA TO SENDO DESTRUIDO SOCORRO");
    }
}

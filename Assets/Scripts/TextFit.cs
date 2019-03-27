using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class TextFit : MonoBehaviour {
    RectTransform _mRect;
    List<Text> _TextsList;
    public bool _FitInside = false;
    
    // Start is called before the first frame update
    void Start() {
        _mRect = GetComponent<RectTransform>();
        _UpdateTextList();
    }

    void _UpdateTextList() {
        _TextsList = new List<Text>(transform.GetComponentsInChildren<Text>());

        // Deligar os texts fit dos filhos caso eles sejam filhos de um statment que ja existe
        TextFit[] FitList = transform.GetComponentsInChildren<TextFit>();
        foreach (TextFit item in FitList) {
            if (item != this) {
                item.FitInside();
            } else {
                //Debug.Log("Me Achei, nem vo fitinside");
            }
        }

        if (_FitInside) {
            _CanvasResize();
        } else {
            _FontResize();
        }
    }

    public void FitInside() {
        _FitInside = true;
    }

    void _CanvasResize() {
        float TotalWidth = 0f;
        _TextsList = new List<Text>(transform.GetComponentsInChildren<Text>());

        foreach (Text item in _TextsList) {
            TotalWidth += item.rectTransform.rect.width;
        }
        _mRect.sizeDelta = new Vector2(TotalWidth, _mRect.rect.height);

    }

    void _FontResize() {
        int TotalChars = 0;
        foreach (Text item in _TextsList) {
            TotalChars += item.text.Length;
        }

        foreach (Text item in _TextsList) {
            int nChars = item.text.Length;
            //item.fontSize = ...
            Rect rect = item.rectTransform.rect;
           // Debug.Log(item.name + " possui chars:" + nChars + " hight:" + _mRect.rect.height + " width:" + rect.width);
            item.rectTransform.sizeDelta = new Vector2( _mRect.rect.width * ((float)nChars/(float)TotalChars), _mRect.rect.height);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (_FitInside) {
            _CanvasResize();
        } else {
            _UpdateTextList();
        }
    }
}

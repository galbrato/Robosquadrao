using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class CanvasFit : MonoBehaviour{
    [SerializeField]float _minSize = 0f;
    RectTransform _mRect;
    List<RectTransform> _RectList;
    HorizontalLayoutGroup _mLayout;
    // Start is called before the first frame update
    void Start() {
        _mRect = GetComponent<RectTransform>();
        if(_minSize <= 0f)_minSize = _mRect.rect.width;
        _UpdateRectList();
        _CanvasResize();
        _mLayout = GetComponent<HorizontalLayoutGroup>();
    }

    void _UpdateRectList() {
        _RectList = new List<RectTransform>();
        foreach (RectTransform r in _mRect) {
            _RectList.Add(r);
        }
    }


    void _CanvasResize() {
        float TotalWidth = 0f;
        string d= "Calculando largura de " + _mRect.name + " filho de " + _mRect.parent.name;
        foreach (RectTransform r in _RectList) {
            d+= "\n\tSomando " + r.name + " largura de " + r.rect.width;
            TotalWidth += r.rect.width;
        }
        if (_mLayout != null) {
            TotalWidth += _mLayout.padding.left;
        }
        d += "\t\nTOTAL: " + TotalWidth;
        //Debug.Log(d);
        _mRect.sizeDelta = new Vector2(Mathf.Max(TotalWidth, _minSize), _mRect.rect.height);

    }

    // Update is called once per frame
    void Update() {
        //if (_mRect.childCount != _RectList.Count) {
        _UpdateRectList();
        //}
        _CanvasResize();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class TextFit : MonoBehaviour {
    RectTransform _mRect;
    List<RectTransform> _RectList;
    
    // Start is called before the first frame update
    void Start() {
        _mRect = GetComponent<RectTransform>();
        _UpdateRectList();
    }

    void _UpdateRectList() {

        _RectList = new List<RectTransform>();
        foreach (RectTransform r in _mRect) {
            _RectList.Add(r);
        }
        _CanvasResize();
    }
    

    void _CanvasResize() {
        float TotalWidth = 0f;
       
        foreach (RectTransform r in _RectList) {
            TotalWidth += r.rect.width;
        }
        _mRect.sizeDelta = new Vector2(TotalWidth, _mRect.rect.height);

    }

    // Update is called once per frame
    void Update(){
        if (_mRect.childCount != _RectList.Count) {
            _UpdateRectList();
        }
    }
}

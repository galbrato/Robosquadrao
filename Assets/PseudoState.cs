using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class PsudoState: MonoBehaviour{
    public RobotState MyVirtualState;
    public StateMachineHandle SMHandler;
    bool _isClicked;
    [SerializeField] float _MinDragTime = 0.1f;
    float _DragCounter = 0f;
    bool _isDragging;
    // Start is called before the first frame update
    void Start() {
        _isClicked = false;
        UpdatePosition();
    }

    // Update is called once per frame
    void Update() {
        if (_isClicked) {
            _DragCounter += Time.deltaTime;
            if (_DragCounter > _MinDragTime) {
                _isDragging = true;
            }
        }

        if (_isDragging) {
            Vector3 p = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            p.z = 0f;
            transform.position = p;
        }
    }

    private void OnMouseDown() {
        Debug.Log("Clico em " + name);
        _DragCounter = 0f;
        _isClicked = true;
    }

    private void OnMouseUp() {
        Debug.Log("levantou");
        if (_isClicked && _DragCounter < _MinDragTime) {
            //Abrir menu de edição do estado
        }
        _isClicked = false;
        _DragCounter = 0f;
        _isDragging = false;
    }

    private void OnMouseExit() {
        if (_isClicked) {
            _isDragging = true;
        }
    }

    public void UpdatePosition() {
        MyVirtualState.SetPosition(transform.position);
    }

}

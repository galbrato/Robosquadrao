using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachineHandle : MonoBehaviour {
    [SerializeField] GameObject StatePrefab;
    [SerializeField] GameObject TransitionPrefab;
    private RobotStateMachine CurrentSM;
    // Start is called before the first frame update
    void Start() {

    }

    // Update is called once per frame
    void Update() {

    }


    public void CreateState() {
        PsudoState PS = Instantiate(StatePrefab, this.transform).GetComponent<PsudoState>();
        PS.MyVirtualState = CurrentSM.CreateState();
        PS.SMHandler = this;
    }

    //Esta função ira carregar estado passado por referencia na interface
    public void ManipulateSM(RobotStateMachine SM) {
        CurrentSM = SM;
    }
}

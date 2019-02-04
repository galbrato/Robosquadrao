using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class RobotStateMachine : MonoBehaviour {
    
    public RobotState InitialState;
    List<RobotState> States;
    // Start is called before the first frame update
    void Start(){
        States = new List<RobotState>();
    }

    // Update is called once per frame
    void Update(){
        
    }

    public RobotState CreateState() {
        RobotState newState = new RobotState(States.Count);
        States.Add(newState);
        return newState;
    }
}

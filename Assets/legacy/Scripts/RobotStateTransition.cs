using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class RobotStateTransition {
    public string StateName;
    public RobotState OriginState;
    public RobotState FinalState;
    //Lista de comando que guardão o codigo de vertifição de transição

    RobotStateTransition(string name, RobotState origin, RobotState final) {
        StateName = name;
        OriginState = origin;
        FinalState = final;
    }

    RobotStateTransition(RobotState origin, RobotState final) {
        StateName = "Transition";
        OriginState = origin;
        FinalState = final;
    }
}

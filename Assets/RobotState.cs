using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class StatePosition {
    public float x;
    public float y;
}

[Serializable]
public class RobotState {
    public string StateName;
    public int StateID;
    private StatePosition Position;

    //Lista de comandos do estado que sera o codigo

    //Lista de transições que tem este estado como origem
    public List<RobotStateTransition> Transitions;
    public RobotState(int id) {
        Transitions = new List<RobotStateTransition>();
        StateName = "Estado " + id;
        StateID = id;
    }

    public void SetPosition(Vector3 pos) {
        Position.x = pos.x;
        Position.y = pos.y;
    }
    public void SetPosition(Vector2 pos) {
        Position.x = pos.x;
        Position.y = pos.y;
    }

    public Vector3 GetPositon() {
        return new Vector3(Position.x, Position.y, 0f);
    }
}

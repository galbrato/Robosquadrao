using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class IA_easy : MonoBehaviour {
    
    private RobotCode myRobotCode;
    private NavMeshAgent agent;
    private RobotCode alvo = null;
    private float menor_dist;
    public Transform Objetivo = null;

    // Start is called before the first frame update
    void Start()
    {
        myRobotCode = this.GetComponent<RobotCode>();
        agent = transform.GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update(){
        if(Objetivo != null && myRobotCode.Inimigos.Count == 0){
            myRobotCode.WalkTo(Objetivo.position);
        }else if(myRobotCode.Inimigos.Count > 0) {
            if(alvo == null){
                menor_dist = 8000.0f;
                foreach(RobotCode robot in myRobotCode.Inimigos){
                    float dist = (robot.transform.position - myRobotCode.transform.position).magnitude;
                    if(dist < menor_dist){
                        menor_dist = dist;
                        alvo = robot;
                    }
                }
            }

            myRobotCode.WalkTo(alvo.robotPosition);
            
            if(agent.isStopped){
                if(alvo.VidaAtual > 0){
                    myRobotCode.Attack(alvo.robotPosition);
                }
            }
        }
    }
}

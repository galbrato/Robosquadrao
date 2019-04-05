using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class IA_easy : MonoBehaviour {
    
    public RobotCode myRobotCode;
    public NavMeshAgent agent;
    private RobotCode alvo = null;
    private float menor_dist;

    // Start is called before the first frame update
    void Start()
    {
        myRobotCode = this.GetComponent<RobotCode>();
        agent = transform.parent.GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update(){
        if(myRobotCode.Inimigos.Count > 0) {
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class IA_easy : MonoBehaviour
{
    public RobotCode myRobotCode;
    public NavMeshAgent agent;

    // Start is called before the first frame update
    void Start()
    {
        myRobotCode = this.GetComponent<RobotCode>();
        agent = transform.parent.GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update(){
        if(myRobotCode.Inimigos.Count > 0){
            myRobotCode.WalkTo(myRobotCode.Inimigos[0].robotPosition);
        
            if(agent.isStopped){
                if(myRobotCode.Inimigos[0].VidaAtual > 0){
                    myRobotCode.Attack(myRobotCode.Inimigos[0].transform.position);
                }
            }
        }else{

        }
    }
}

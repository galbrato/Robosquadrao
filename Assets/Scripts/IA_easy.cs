using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IA_easy : MonoBehaviour
{
    private RobotCode myRobotCode;
    private Rigidbody rigid;
    private RobotCode alvo = null;
    private float menor_dist;

    // Start is called before the first frame update
    void Start()
    {
        myRobotCode = this.GetComponent<RobotCode>();
        rigid = transform.parent.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update(){
        if(myRobotCode.Inimigos.Count > 0){
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

                myRobotCode.WalkTo(alvo.transform.position);
                
                if(rigid.velocity == Vector3.zero){
                    if(alvo.VidaAtual > 0){
                        myRobotCode.Attack(alvo.transform.position);
                    }
                }
        }else{

        }
    }
}

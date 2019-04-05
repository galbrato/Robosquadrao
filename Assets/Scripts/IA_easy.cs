using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IA_easy : MonoBehaviour
{
    public RobotCode myRobotCode;
    public Rigidbody rigid;

    // Start is called before the first frame update
    void Start()
    {
        myRobotCode = this.GetComponent<RobotCode>();
        rigid = transform.parent.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update(){
        if(myRobotCode.Inimigos.Count > 0){
                myRobotCode.WalkTo(myRobotCode.Inimigos[0].transform.position);
            
                if(rigid.velocity == Vector3.zero){
                    if(myRobotCode.Inimigos[0].VidaAtual > 0){
                        myRobotCode.Attack(myRobotCode.Inimigos[0].transform.position);
                    }
                }
        }else{

        }
    }
}

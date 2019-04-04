using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IA_easy : MonoBehaviour
{
    public Transform alvo;
    public RobotCode myRobotCode;
    public Rigidbody rigid;
    public Vector3 posicao;
    // Start is called before the first frame update
    void Start()
    {
        myRobotCode = this.GetComponent<RobotCode>();
        rigid = GetComponent<Rigidbody>();
        posicao = alvo.position;
    }

    // Update is called once per frame
    void Update(){
        if(alvo != null){   
            myRobotCode.WalkTo(alvo.position);
        
            if(rigid.velocity == Vector3.zero){
                myRobotCode.Attack(alvo.position);
            }
        }
    }
}

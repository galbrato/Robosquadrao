using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sensor : MonoBehaviour
{
    private RobotCode my_robot;
    private SphereCollider myCollider;
    // Start is called before the first frame update
    void Start()
    {
        my_robot = transform.parent.GetComponent<RobotCode>();
        myCollider = GetComponent<SphereCollider>();
    }

    // Update is called once per frame
    void Update(){
        
    }

    void OnTriggerEnter(Collider other){
        if((other.gameObject.layer != transform.parent.gameObject.layer) && (other.gameObject.layer == 8 || other.gameObject.layer == 9)){
            my_robot.Inimigos.Add(other.gameObject.GetComponent<RobotCode>());
        }

        if(other.gameObject.layer == transform.parent.gameObject.layer){
            my_robot.Aliados.Add(other.gameObject.GetComponent<RobotCode>());
        }
    }

    void OnTriggerExit(Collider other){
        if((other.gameObject.layer != transform.parent.gameObject.layer) && (other.gameObject.layer == 8 || other.gameObject.layer == 9)){
            my_robot.Inimigos.Remove(other.gameObject.GetComponent<RobotCode>());
        }

        if(other.gameObject.layer == transform.parent.gameObject.layer){
            my_robot.Aliados.Remove(other.gameObject.GetComponent<RobotCode>());
        }
    }
}

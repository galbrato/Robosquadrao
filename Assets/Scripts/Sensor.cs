using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sensor : MonoBehaviour
{
    public List <RobotCode> adversarios;
    public float raio_Sensor;
    private SphereCollider myCollider;
    // Start is called before the first frame update
    void Start()
    {
        adversarios = new List<RobotCode>();
        myCollider = GetComponent<SphereCollider>();
        myCollider.radius = raio_Sensor;
    }

    // Update is called once per frame
    void Update()
    { 
    }

    void OnTriggerEnter(Collider other){
        if((other.gameObject.layer != gameObject.transform.parent.gameObject.layer) && other.gameObject.layer != 11){
            adversarios.Add(other.gameObject.GetComponent<RobotCode>());
        }
    }

    void OnTriggerExit(Collider other){
        if((other.gameObject.layer != gameObject.transform.parent.gameObject.layer) && other.gameObject.layer != 11){
            adversarios.Remove(other.gameObject.GetComponent<RobotCode>());
        }
    }
}

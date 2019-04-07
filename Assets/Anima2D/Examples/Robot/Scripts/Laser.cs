using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class Laser : MonoBehaviour
{
	public Vector3 target;
	public float speed = 50;
	public RobotCode mae;

	void Start(){
		Vector3 direction = target - transform.position;
		transform.right = direction.normalized;
	}

    // Update is called once per frame
    void Update(){	
        if(target != null){ // MUDEI O TARGET DE TRANSFORM PARA VECTOR3
            GetComponent<Rigidbody>().velocity = transform.right * speed;
        }

        if(Vector3.Distance(transform.position, target) < 1.3f){
            mae.ApplyLaserDamage(target);
            Destroy(this.gameObject);
        }
    }

    void FixedUpdate(){
    	GetComponent<SortingGroup>().sortingOrder = -(int)(this.transform.position.z*100);
    }
}

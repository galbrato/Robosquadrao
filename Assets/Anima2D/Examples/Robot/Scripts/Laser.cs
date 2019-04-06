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

		if(transform.position == target){
			Destroy(this);
			mae.ApplyLaserDamage(target);
		}
  }

    void FixedUpdate(){
    	GetComponent<SortingGroup>().sortingOrder = -(int)(this.transform.position.z*100);
    }
}

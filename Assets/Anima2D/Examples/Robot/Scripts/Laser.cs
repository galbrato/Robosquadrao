﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class Laser : MonoBehaviour
{
	public Vector3 target;
	public float speed = 50;
	public LayerMask mask;

	void Start(){
		Vector3 direction = target - transform.position;
		//transform.LookAt(target.position, transform.up);
		transform.right = direction.normalized;
	}

    // Update is called once per frame
  void Update(){	
    if(target != null){ // MUDEI O TARGET DE TRANSFORM PARA VECTOR3
    	GetComponent<Rigidbody>().velocity = transform.right * speed;
    }

		Collider[] hitcolliders = Physics.OverlapBox(transform.position, transform.localScale/2, Quaternion.identity, mask);

		if(hitcolliders.Length > 0){
			hitcolliders[0].GetComponent<Alive>().lifes -= 1.0f;
			hitcolliders[0].transform.GetChild(9).GetComponent<Animator>().SetTrigger("Hitted");
			Destroy(this.gameObject);
		}
  }

    void FixedUpdate(){
    	GetComponent<SortingGroup>().sortingOrder = -(int)(this.transform.position.z*100);
    }
}

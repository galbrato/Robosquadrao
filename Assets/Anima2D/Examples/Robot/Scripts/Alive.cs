using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class Alive : MonoBehaviour
{
    public float lifes = 3;
    public float speed;

    private Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody> ();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S)){
            this.GetComponent<Animator>().SetBool("IsMoving", true);
            if(Input.GetKey(KeyCode.A)){
                this.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
            }
            if(Input.GetKey(KeyCode.D)){
                this.transform.localScale = new Vector3(-1.0f, 1.0f, 1.0f);
            }

        }else {
            this.GetComponent<Animator>().SetBool("IsMoving", false);
        }

        if (lifes >= 3.0f){
            lifes = 3.0f;
        } else if(lifes == 0.0f){
            Destroy(gameObject);
        }
    }

    void FixedUpdate(){
        float moveHorizontal = Input.GetAxis ("Horizontal");
        float moveVertical = Input.GetAxis ("Vertical");

        Vector3 movement = new Vector3 (moveHorizontal, 0.0f, moveVertical);

        rb.AddForce (movement * speed);

        GetComponent<SortingGroup>().sortingOrder = -(int)(this.transform.position.z*100);
    }
}

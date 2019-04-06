using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Habilities : MonoBehaviour
{
	private Animator anima;
	public Transform hand;
	public Transform chave;
	public Transform LaserPosition;
	public Transform target;
	public GameObject LaserBeam;
    public LayerMask my_mask;
    public Rigidbody my_rig;
    // Start is called before the first frame update
    void Start()
    {
    	anima = GetComponent<Animator>();
        my_rig = GetComponent<Rigidbody>();   
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKey(KeyCode.Alpha1)){
        	anima.SetBool("Attack", true);
            my_rig.velocity = new Vector3(0,0,0);
        }else if(Input.GetKey(KeyCode.Alpha2)){
        	anima.SetBool("Heal", true);
            my_rig.velocity = new Vector3(0,0,0);
        }else if(Input.GetKey(KeyCode.Alpha3)){
        	anima.SetBool("Laser", true);
            my_rig.velocity = new Vector3(0,0,0);
            if(target.position.x > transform.position.x){
                this.transform.localScale = new Vector3(-1.0f, 1.0f, 1.0f);
            }else{
                this.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
            }
        }else{
        	anima.SetBool("Attack", false);
        	anima.SetBool("Heal", false);
        	anima.SetBool("Laser", false);
        }
    }

    public void ApplyDamage(){
    	LayerMask mask = (1 << this.gameObject.layer);
        mask |= (1 << 11);
        mask = ~mask;
    	Collider[] hitColliders = Physics.OverlapBox(hand.position, transform.localScale/2, Quaternion.identity, mask);

        if(hitColliders.Length > 0){
            print(hitColliders[0].name);
        	hitColliders[0].GetComponent<Alive>().lifes -= 1.0f;
            hitColliders[0].transform.GetChild(9).GetComponent<Animator>().SetTrigger("Hitted");
        }
    }

    public void ApplyHeal(){
    	LayerMask mask = 1 << this.gameObject.layer;
    	Collider[] hitColliders = Physics.OverlapBox(chave.position, transform.localScale/2, Quaternion.identity, mask);

        if(hitColliders.Length > 0){
        	hitColliders[0].GetComponent<Alive>().lifes += 1.0f;
            hitColliders[0].transform.GetChild(9).GetComponent<Animator>().SetTrigger("Healed");
        }
    }

    public void ApplyLaserBeam(){
    	GameObject laser = Instantiate(LaserBeam, LaserPosition.position, Quaternion.identity) as GameObject;
    	laser.transform.localScale = transform.localScale;
    	laser.GetComponent<Laser>().target = target.position;
    }
}

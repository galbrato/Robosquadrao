using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Habilities : MonoBehaviour
{
	private Animator anima;
	public 	LayerMask m_LayerMask;
	public Transform hand;
	public Transform chave;
	public Transform LaserPosition;
	public Transform target;
	public GameObject LaserBeam;
	private bool m_Started = false;
    // Start is called before the first frame update
    void Start()
    {
    	anima = GetComponent<Animator>();   
    	m_Started = true;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKey(KeyCode.Alpha1)){
        	anima.SetBool("Attack", true);
        }else if(Input.GetKey(KeyCode.Alpha2)){
        	anima.SetBool("Heal", true);
        }else if(Input.GetKey(KeyCode.Alpha3)){
        	anima.SetBool("Laser", true);
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

    void OnDrawGizmos(){
    	Gizmos.color = Color.red;
        //Check that it is being run in Play Mode, so it doesn't try to draw this in Editor mode
        if (m_Started){
            //Draw a cube where the OverlapBox is (positioned where your GameObject is as well as a size)
            Gizmos.DrawWireCube(chave.position, transform.localScale/2);
        }
    }

    public void ApplyDamage(){
    	LayerMask mask = LayerMask.GetMask("Enemy");
    	Collider[] hitColliders = Physics.OverlapBox(hand.position, transform.localScale/2, Quaternion.identity, mask);
        int i = 0;

        while (i < hitColliders.Length)
        {
            //Output all of the collider names
            Debug.Log("Hit : " + hitColliders[i].name + i);
            //Increase the number of Colliders in the array
            i++;
        }
        if(hitColliders.Length > 0){
        	hitColliders[0].GetComponent<Alive>().lifes -= 1.0f;
            hitColliders[0].transform.GetChild(9).GetComponent<Animator>().SetTrigger("Hitted");
        }
    	print("Ataquei");
    }

    public void ApplyHeal(){
    	LayerMask mask = LayerMask.GetMask("Friend");
    	Collider[] hitColliders = Physics.OverlapBox(chave.position, transform.localScale/2, Quaternion.identity, mask);
        int i = 0;

        while (i < hitColliders.Length)
        {
            //Output all of the collider names
            Debug.Log("Hit : " + hitColliders[i].name + i);
            //Increase the number of Colliders in the array
            i++;
        }
        if(hitColliders.Length > 0){
        	hitColliders[0].GetComponent<Alive>().lifes += 1.0f;
            hitColliders[0].transform.GetChild(9).GetComponent<Animator>().SetTrigger("Healed");
        }

    	print("Curei");
    }

    public void ApplyLaserBeam(){
    	GameObject laser = Instantiate(LaserBeam, LaserPosition.position, Quaternion.identity) as GameObject;
    	laser.transform.localScale = transform.localScale;
    	laser.GetComponent<Laser>().target = target;

    }
}

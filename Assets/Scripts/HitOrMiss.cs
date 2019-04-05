using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitOrMiss : MonoBehaviour
{
    // Start is called before the first frame update
    public List<GameObject> Friends;
    public List<GameObject> Enemies;

    public GameObject Win;
    public GameObject Lose;
    public bool DestruirIni;
    public bool ChegarObj;
    private SphereCollider col;
    private bool MortosIni = false;
    void Start()
    {
        Friends.AddRange(GameObject.FindGameObjectsWithTag("Friend"));
        Enemies.AddRange(GameObject.FindGameObjectsWithTag("Enemy"));
        col = GetComponent<SphereCollider>();

        if(ChegarObj == true){
            col.enabled = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        Friends.RemoveAll(x => x == null);
        Enemies.RemoveAll(x => x == null);
        
        if (Friends.Count == 0){
            Lose.SetActive(true);
        }

        if(DestruirIni == true){
            if (Enemies.Count == 0){
                if(ChegarObj == false){
                   Win.SetActive(true);
                }else{
                    MortosIni = true;
                }
            }
        }
    }

    void OnTriggerEnter(Collider other){
        print("Entrou");
        if(other.gameObject.layer == 8){
            if((DestruirIni == true && MortosIni == true) || DestruirIni == false){
                Win.SetActive(true);
            }
        }
    }
}

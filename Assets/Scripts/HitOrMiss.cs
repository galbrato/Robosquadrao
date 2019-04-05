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
    void Start()
    {
        Friends.AddRange(GameObject.FindGameObjectsWithTag("Friend"));
        Enemies.AddRange(GameObject.FindGameObjectsWithTag("Enemy"));
    }

    // Update is called once per frame
    void Update()
    {
        Friends.RemoveAll(x => x == null);
        Enemies.RemoveAll(x => x == null);
        
        if (Friends.Count == 0){
            Lose.SetActive(true);
        }

        if (Enemies.Count == 0){
            Win.SetActive(true);
        }

    }
}

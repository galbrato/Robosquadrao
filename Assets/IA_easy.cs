using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IA_easy : MonoBehaviour
{
    public Transform alvo;
    public RobotCode myRobotCode;
    // Start is called before the first frame update
    void Start()
    {
        myRobotCode = this.GetComponent<RobotCode>();
    }

    // Update is called once per frame
    void Update()
    {
       // myRobotCode.WalkToo(new Vector3(0,0,0));
    }
}

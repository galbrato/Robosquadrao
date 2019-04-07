using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotAnim : MonoBehaviour
{
    RobotCode mRobot;
    // Start is called before the first frame update
    void Start()
    {
        mRobot = GetComponentInParent<RobotCode>();
    }
    public void ApplyDamage() {
        mRobot.ApplyDamage();
    }

    public void ApplyLaserBeam() {
        mRobot.ApplyLaserBeam();
    }

}

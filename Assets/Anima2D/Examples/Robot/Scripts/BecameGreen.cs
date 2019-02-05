using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BecameGreen : StateMachineBehaviour
{
    List<SpriteRenderer> robot_Sprites;
    private float time = 0.0f;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        time = 0.0f;
        robot_Sprites = new List<SpriteRenderer>();
        animator.gameObject.transform.parent.GetComponentsInChildren<SpriteRenderer>(robot_Sprites);
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        time += Time.deltaTime*5f;
        foreach(SpriteRenderer spt in robot_Sprites){
            spt.material.color = Color.Lerp(spt.material.color,Color.green, time);
        }
        if(robot_Sprites[robot_Sprites.Count - 1].material.color == Color.green){
            animator.SetTrigger("BeNormal");
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}

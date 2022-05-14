using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundLandEffects : StateMachineBehaviour
{

    public ParticleSystem GroundLandParticles;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        GameObject playerReference = GameObject.Find("Player");
        Transform playerPos = playerReference.transform;
        BoxCollider2D playerBoxCollider = playerReference.GetComponent<BoxCollider2D>();

        float boxColYExtents = playerBoxCollider.bounds.extents.y;
        CameraShake.Instance.ShakeCamera(2f, 2f, 0.3f);
        ParticleSystem groundLandEffect = Instantiate(GroundLandParticles, new Vector2(playerPos.transform.position.x, playerPos.transform.position.y - boxColYExtents), 
            Quaternion.identity);
        groundLandEffect.Play();

    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    //override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

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

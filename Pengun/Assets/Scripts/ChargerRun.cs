using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargerRun : StateMachineBehaviour
{

    Transform player;
    Rigidbody2D rigidbody;
    ChargerAI chargerAI;


    public float speed;
    public float attackRange;
    public float playerRange;
    public float chargeForce;
    bool isCharging = false;
    bool isFlipped;
    bool isPlayerInRange = false;
    bool isReturningToSpawn = false;

    float distanceToPlayer;

    //OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        rigidbody = animator.GetComponent<Rigidbody2D>();
        chargerAI = animator.GetComponent<ChargerAI>();
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        CheckDistanceToPlayer();

        if (isPlayerInRange)
        {
            chargerAI.LookAtPlayer();
        }

        isFlipped = chargerAI.GetIsFlipped();

        Vector2 target = new Vector2(player.position.x, rigidbody.position.y);
        Vector2 newPosition = Vector2.MoveTowards(rigidbody.position, target, speed * Time.fixedDeltaTime);

        ReturnToSpawnIfNotGrounded();

        if (!isCharging && isPlayerInRange && chargerAI.GetIsTouchingGround())
        {
            rigidbody.MovePosition(newPosition);            
        }

        distanceToPlayer = Vector2.Distance(player.transform.position, rigidbody.transform.position);

        if (distanceToPlayer <= attackRange)
        {
            isCharging = true;
            animator.SetTrigger("Attack");
            animator.SetBool("IsFinishedCharging", false);
            chargerAI.ChargeDelay(target, chargeForce);
        }
        else
        {
            isCharging = false;
            chargerAI.StopDelayCharge();
            animator.SetBool("IsFinishedCharging", true);
        }

    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.ResetTrigger("Attack");
    }

    void CheckDistanceToPlayer()
    {
        if (distanceToPlayer <= playerRange)
        {
            isPlayerInRange = true;
        }
        else
        {
            isPlayerInRange = false;
        }
    }

    void ReturnToSpawnIfNotGrounded()
    {
        Vector2 backToSpawn = Vector2.MoveTowards(rigidbody.position, chargerAI.GetSpawnPosition(), speed * Time.fixedDeltaTime);

        if (chargerAI.GetIsTouchingGround() == false && !isPlayerInRange)
        {
            isReturningToSpawn = true;
        }

        if (isReturningToSpawn)
        {
            if (rigidbody.position == chargerAI.GetSpawnPosition() || isPlayerInRange)
            {
                isReturningToSpawn = false;
            }
            rigidbody.MovePosition(backToSpawn);
            chargerAI.LookAtSpawnPosition();
        }
    }
}

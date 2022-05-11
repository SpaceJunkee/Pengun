using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrombieAI : MonoBehaviour
{
    EnemyPathfinding enemyPathFinding;
    EnemyHealthManager grombieHealthManager;
    bool hasStopped = false;
    float grombieOriginalSpeed;
    bool isInAttackRange;
    bool isInChaseRange;
    bool isCurrentlyVomitting = false;
    public float attackRange;
    public float chaseRange;
    public float chaseSpeed;
    bool hasPuddleDropped = false;

    public GameObject gromPuddle;

    GameObject attackPoint;
    GameObject puddleSpawnPositionObject;

    Vector2 gromPuddleSpawnPosition;

    public Animator animator;

    public float dragTimer;

    private void Start()
    {
        enemyPathFinding = this.GetComponent<EnemyPathfinding>();
        grombieHealthManager = this.GetComponent<EnemyHealthManager>();
        grombieOriginalSpeed = enemyPathFinding.speed;
        attackPoint = this.transform.GetChild(1).gameObject;
        puddleSpawnPositionObject = this.transform.GetChild(2).gameObject;
    }

    private void Update()
    {
        AttemptSeekingRange();
        AttemptChasingRange();
        AttemptAttackRange();

        DropGromPuddleWhenDead();

    }   

    private void OnDestroy()
    {
        
    }

    void AttemptSeekingRange()
    {
        if (enemyPathFinding.playerIsInRange && !hasStopped && !isInAttackRange && !isCurrentlyVomitting)
        {
            StartCoroutine("MoveGrombieLikeAZombie");
            animator.SetBool("isPlayerInRange", true);
        }
        else if (!enemyPathFinding.playerIsInRange && !isInAttackRange && !isCurrentlyVomitting)
        {
            animator.SetBool("isPlayerInRange", false);
        }
    }

    void AttemptChasingRange()
    {
        if (enemyPathFinding.distanceToPlayer < chaseRange && enemyPathFinding.distanceToPlayer > attackRange && !isCurrentlyVomitting)
        {
            isInChaseRange = true;
            enemyPathFinding.speed = grombieOriginalSpeed * chaseSpeed;
            animator.SetBool("isInChaseRange", true);
        }
        else
        {
            isInChaseRange = false;
            animator.SetBool("isInChaseRange", false);
        }
    }

    void AttemptAttackRange()
    {
        if (enemyPathFinding.distanceToPlayer < attackRange && !isCurrentlyVomitting)
        {
            isInAttackRange = true;
            enemyPathFinding.speed = 0;
            animator.SetBool("isInAttackRange", true);
            animator.SetTrigger("Vomit");
            isCurrentlyVomitting = true;
            animator.SetBool("isCurrentlyVomitting", true);

            StartCoroutine("ActivateAttackPoint");
            StartCoroutine("ResetVomit");
        }
        else
        {
            isInAttackRange = false;
            animator.SetBool("isInAttackRange", false);
        }
    }

    void DropGromPuddleWhenDead()
    {
        gromPuddleSpawnPosition = puddleSpawnPositionObject.transform.position;

        if (grombieHealthManager.isDead && !hasPuddleDropped)
        {
            hasPuddleDropped = true;
            Instantiate(gromPuddle, gromPuddleSpawnPosition, Quaternion.identity);
        }
    }



    IEnumerator MoveGrombieLikeAZombie()
    {
        hasStopped = true;
        enemyPathFinding.speed = grombieOriginalSpeed;
        animator.SetFloat("MovementSpeed", grombieOriginalSpeed);
        yield return new WaitForSeconds(dragTimer);
        enemyPathFinding.speed = 0;
        animator.SetFloat("MovementSpeed", 0);

        yield return new WaitForSeconds(dragTimer);
        hasStopped = false;
    }

    IEnumerator ResetVomit()
    {
        yield return new WaitForSeconds(2);
        isCurrentlyVomitting = false;
        animator.SetBool("isCurrentlyVomitting", false);
        enemyPathFinding.speed = grombieOriginalSpeed;
    }

    IEnumerator ActivateAttackPoint()
    {
        yield return new WaitForSeconds(0.75f);
        attackPoint.SetActive(true);
        yield return new WaitForSeconds(0.75f);
        attackPoint.SetActive(false);
    }
}

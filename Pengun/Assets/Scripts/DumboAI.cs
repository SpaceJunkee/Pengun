using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DumboAI : MonoBehaviour
{
    EnemyPathfinding enemyPathFinding;


    bool isInAttackRange;
    public float attackRange;
    bool isInAttackMode = false;
    GameObject attackPoint;
    public Animator animator;

    public bool isPlayerInRange;


    private void Start()
    {
        enemyPathFinding = this.GetComponent<EnemyPathfinding>();
        attackPoint = this.transform.GetChild(0).gameObject;
    }

    private void Update()
    {
        isPlayerInRange = enemyPathFinding.playerIsInRange;

        ManageAnimationLogic();

        if (enemyPathFinding.distanceToPlayer < attackRange)
        {
            isInAttackRange = true;
            animator.SetBool("isInAttackRange", true);
        }
        else
        {
            isInAttackRange = false;
            animator.SetBool("isInAttackRange", false);
        }

        if (isInAttackRange)
        {
            Invoke("Attack", 1f);          
        }
        else if(!isInAttackRange)
        {
            attackPoint.SetActive(false);
        }

    }

    void Attack()
    {
        attackPoint.SetActive(true);
    }

    private void OnDestroy()
    {
        DumboSpawner.RemoveADumbo();
    }

    void ManageAnimationLogic()
    {
        if (isPlayerInRange)
        {
            animator.SetBool("isPlayerInRange", true);
        }
        else
        {
            animator.SetBool("isPlayerInRange", false);
        }
    }

}

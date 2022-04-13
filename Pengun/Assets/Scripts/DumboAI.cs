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


    private void Start()
    {
        enemyPathFinding = this.GetComponent<EnemyPathfinding>();
        attackPoint = this.transform.GetChild(0).gameObject;
    }

    private void Update()
    {

        if (enemyPathFinding.distanceToPlayer < attackRange)
        {
            isInAttackRange = true;
        }
        else
        {
            isInAttackRange = false;
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

}

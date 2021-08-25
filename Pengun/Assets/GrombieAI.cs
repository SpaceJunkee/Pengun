using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrombieAI : MonoBehaviour
{
    EnemyPathfinding enemyPathFinding;
    EnemyHealthManager grombieHealthManager;
    bool hasStopped = false;
    float dumboOriginalSpeed;
    bool isInAttackRange;
    public float attackRange;
    bool hasPuddleDropped = false;

    public GameObject gromPuddle;

    GameObject attackPoint;
    GameObject puddleSpawnPositionObject;

    Vector2 gromPuddleSpawnPosition;

    private void Start()
    {
        enemyPathFinding = this.GetComponent<EnemyPathfinding>();
        grombieHealthManager = this.GetComponent<EnemyHealthManager>();
        dumboOriginalSpeed = enemyPathFinding.speed;
        attackPoint = this.transform.GetChild(1).gameObject;
        puddleSpawnPositionObject = this.transform.GetChild(2).gameObject;

        if(gromPuddle)
        {
            Destroy(gromPuddle);
        }
    }

    private void Update()
    {
        if (enemyPathFinding.playerIsInRange && !hasStopped)
        {
            StartCoroutine("MoveGrombieLikeAZombie");
        }

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
            attackPoint.SetActive(true);
        }
        else
        {
            attackPoint.SetActive(false);
        }

        gromPuddleSpawnPosition = puddleSpawnPositionObject.transform.position;

        if (grombieHealthManager.isDead && !hasPuddleDropped)
        {
            hasPuddleDropped = true;
            Instantiate(gromPuddle, gromPuddleSpawnPosition, Quaternion.identity);
        }

    }   


    private void OnDestroy()
    {
        
    }


    IEnumerator MoveGrombieLikeAZombie()
    {
        hasStopped = true;
        enemyPathFinding.speed = dumboOriginalSpeed;
        yield return new WaitForSeconds(0.4f);
        enemyPathFinding.speed = 0;

        yield return new WaitForSeconds(0.4f);
        hasStopped = false;
    }

}

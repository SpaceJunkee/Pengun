using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrombieAI : MonoBehaviour
{
    EnemyPathfinding enemyPathFinding;
    bool hasStopped = false;
    float dumboOriginalSpeed;
    bool isInAttackRange;
    public float attackRange;

    public GameObject gromPuddle;

    GameObject attackPoint;
    GameObject puddleSpawnPositionObject;

    Vector2 gromPuddleSpawnPosition;

    private void Start()
    {
        enemyPathFinding = this.GetComponent<EnemyPathfinding>();
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
            StartCoroutine("MoveDumboLikeAZombie");
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

    }   


    private void OnDestroy()
    {
        Instantiate(gromPuddle, gromPuddleSpawnPosition, Quaternion.identity);
    }


    IEnumerator MoveDumboLikeAZombie()
    {
        hasStopped = true;
        enemyPathFinding.speed = dumboOriginalSpeed;
        yield return new WaitForSeconds(0.4f);
        enemyPathFinding.speed = 0;

        yield return new WaitForSeconds(0.4f);
        hasStopped = false;
    }

}

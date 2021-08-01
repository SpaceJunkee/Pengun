using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargerAI : MonoBehaviour
{
    [SerializeField]

    EnemyPathfinding enemyPathFinding;
    public Rigidbody2D rigidbody;

    private bool reachedEndOfPath;


    private void Update()
    {
        reachedEndOfPath = enemyPathFinding.GetReachedEndOfPath();
    }


    private void FixedUpdate()
    {
        if (reachedEndOfPath)
        {
            StartCoroutine("HaltPosition");
        }
    }

    IEnumerator HaltPosition()
    {
        rigidbody.mass = 250;
        yield return new WaitForSeconds(0.5f);
        rigidbody.mass = 1f;

    }
}

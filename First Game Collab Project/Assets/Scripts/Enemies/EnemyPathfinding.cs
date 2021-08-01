using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class EnemyPathfinding : MonoBehaviour
{
    public Transform target;

    public float speed = 200f;
    public float nextWaypointDistance = 3f;
    public float pathUpdateRate;
    public float range;
    float distanceToPlayer;
    public float targetDistanceOverlap;

    public Transform enemyGFX;

    Path path;
    int currentWaypoint = 0;
    bool reachedEndOfPath = false;
    bool playerIsInRange = false;

    Seeker seeker;
    Rigidbody2D rigidBody;

    public float enemyScaleX;
    public float enemyScaleY; 

    private void Start()
    {
        seeker = GetComponent<Seeker>();
        rigidBody = GetComponent<Rigidbody2D>();

        enemyScaleX = enemyGFX.localScale.x;
        enemyScaleY = enemyGFX.localScale.y;

        //UpdatePath();

        //Paste this in specific enemytype class and modify to their behaviour
        InvokeRepeatingUpdatePath();
    }

    private void Update()
    {
        distanceToPlayer = Vector2.Distance(target.transform.position, this.gameObject.transform.position);

        if (distanceToPlayer < range)
        {
            playerIsInRange = true;
        }
    }

    public void UpdatePath()
    {
        if (seeker.IsDone())
        {
            seeker.StartPath(rigidBody.position, target.position, OnPathComplete);
           // rigidBody.constraints = PlayerMovement.originalConstraints;
        }
        
    }

    void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            path = p;
            currentWaypoint = 0;
        }
    }

    private void FixedUpdate()
    {
        
        if(path == null)
        {
            return;
        }

        if(currentWaypoint >= path.vectorPath.Count)
        {
            reachedEndOfPath = true;
            return;
        }
        else
        {
            reachedEndOfPath = false;
        }

        Vector2 direction = ((Vector2)path.vectorPath[currentWaypoint] - rigidBody.position).normalized;


        Vector2 force = direction * speed * Time.deltaTime;

        if (playerIsInRange)
        {
            rigidBody.AddForce(force);
        }
        

        float distance = Vector2.Distance(rigidBody.position, path.vectorPath[currentWaypoint]);

        if(distance < nextWaypointDistance)
        {
            currentWaypoint++;
        }

        if(force.x >= 0.01f)
        {
            enemyGFX.localScale = new Vector3(-enemyScaleX, enemyScaleY, 1f);
            
        }
        else if(force.x <= -0.01f)
        {
            enemyGFX.localScale = new Vector3(enemyScaleX, enemyScaleY, 1f);
        }
    }

    public bool GetReachedEndOfPath()
    {
        return reachedEndOfPath;
    }

    public void InvokeRepeatingUpdatePath()
    {
        InvokeRepeating("UpdatePath", 0f, pathUpdateRate);
    }
}

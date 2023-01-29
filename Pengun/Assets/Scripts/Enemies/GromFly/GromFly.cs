using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Pathfinding;

public class GromFly : MonoBehaviour
{
    public Transform target;
    public float speed = 200f;
    public float nextWaypointDistance = 3f;
    public float range = 20f;

    public Transform gromFlyGFX;
    public Animator animator;

    Path path;
    int currentWaypoint = 0;
    bool reachedEndOfPath = false;

    Seeker seeker;
    Rigidbody2D rigidbody;

    private void Start()
    {
        seeker = GetComponent<Seeker>();
        rigidbody = GetComponent<Rigidbody2D>();

        InvokeRepeating("UpdatePath", 0f, 0.2f);
        
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

        //Move bird
        Vector2 direction = ((Vector2)path.vectorPath[currentWaypoint] - rigidbody.position).normalized;
        Vector2 force = direction * speed * Time.deltaTime;

        rigidbody.AddForce(force);

        float distance = Vector2.Distance(rigidbody.position, path.vectorPath[currentWaypoint]);

        if(distance < nextWaypointDistance)
        {
            currentWaypoint++;
        }

        //Flip graphics
        if (rigidbody.velocity.x >= 0.01f)
        {
            gromFlyGFX.localScale = new Vector3(1f, 1f, 1f);
        }
        else if(rigidbody.velocity.x <= 0.01f)
        {
            gromFlyGFX.localScale = new Vector3(-1f, 1f, 1f);
        }
    }

    void UpdatePath()
    {
        float distance = Vector2.Distance(rigidbody.position, target.position);
        
        if (distance <= range && seeker.IsDone())
        {
            animator.SetBool("IsSeekingPlayer", true);
            seeker.StartPath(rigidbody.position, target.position, OnPathComplete);
        }
        else
        {
            animator.SetBool("IsSeekingPlayer", false);
            path = null;
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
}


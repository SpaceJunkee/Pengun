using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Pathfinding;

public class GromFly : MonoBehaviour
{
    public Transform target;
    EnemyHealthManager enemyHealthManager;
    GromFlyAudioManager gromFlyAudioManager;
    public float speed = 200f;
    public float nextWaypointDistance = 3f;
    public float range = 20f;
    bool isPlayingChaseSound = false;

    public Transform gromFlyGFX;
    public Animator animator;

    Path path;
    int currentWaypoint = 0;
    bool reachedEndOfPath = false;

    Seeker seeker;
    Rigidbody2D rigidbody;

    bool isBeingChased = false;

    private void Start()
    {
        enemyHealthManager = this.GetComponent<EnemyHealthManager>();
        target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        gromFlyAudioManager = GetComponent<GromFlyAudioManager>();
        seeker = GetComponent<Seeker>();
        rigidbody = GetComponent<Rigidbody2D>();

        InvokeRepeating("UpdatePath", 0f, 0.2f);
        
    }

    private void FixedUpdate()
    {
        if (path == null)
        {
            return;
        }

        if (currentWaypoint >= path.vectorPath.Count)
        {
            reachedEndOfPath = true;
            return;
        }
        else
        {
            reachedEndOfPath = false;
        }

        // Move bird
        Vector2 direction = ((Vector2)path.vectorPath[currentWaypoint] - rigidbody.position).normalized;
        Vector2 force = direction * speed * Time.deltaTime;

        rigidbody.AddForce(force);

        float distance = Vector2.Distance(rigidbody.position, path.vectorPath[currentWaypoint]);

        if (distance < nextWaypointDistance)
        {
            currentWaypoint++;
        }

        // Flip graphics
        if (rigidbody.velocity.x >= 0.01f)
        {
            gromFlyGFX.localScale = new Vector3(1f, 1f, 1f);
        }
        else if (rigidbody.velocity.x <= 0.01f)
        {
            gromFlyGFX.localScale = new Vector3(-1f, 1f, 1f);
        }

        float distanceToTarget = Vector2.Distance(rigidbody.position, target.position);

        if (isBeingChased && distanceToTarget > range)
        {
            isBeingChased = false;
            isPlayingChaseSound = false;
            gromFlyAudioManager.StopAudioSource("Chase");
        }

        if (enemyHealthManager.isDead)
        {
            gromFlyAudioManager.StopAudioSource("Chase");
        }
    }

    public float alertCooldown = 5f;
    float alertTimer = 0f;

    void UpdatePath()
    {
        float distance = Vector2.Distance(rigidbody.position, target.position);

        if (distance <= range && seeker.IsDone())
        {
            isBeingChased = true;
            if (alertTimer <= 0)
            {
                gromFlyAudioManager.PlayAudioSource("Alert");
                animator.SetTrigger("Alerted");
                alertTimer = alertCooldown;
            }

            if (isBeingChased && !isPlayingChaseSound)
            {
                isPlayingChaseSound = true;
                gromFlyAudioManager.PlayAudioSource("Chase");
            }

            if (alertTimer > 0)
            {
                alertTimer = alertCooldown;
            }


            animator.SetBool("IsSeekingPlayer", true);
            seeker.StartPath(rigidbody.position, target.position, OnPathComplete);
        }
        else
        {
            isBeingChased = false;
            animator.SetBool("IsSeekingPlayer", false);
            path = null;


            if (alertTimer > 0)
            {
                alertTimer -= Time.fixedDeltaTime * 10;
            }


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


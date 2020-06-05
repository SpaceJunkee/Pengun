using UnityEngine;

public class EnemyScript1 : MonoBehaviour
{
    public int health = 100;
    public float speed;
    public float minPatrolRangeXValue;
    public float maxPatrolRangeXValue;
    public float startWaitTime;
    public float enemySightLength;

    private Vector3 offset;
    private bool movingRight = true;
    private bool move = true;
    private bool canStop = true;
    private Vector2 leftEndPosition;
    private Vector2 rightEndPosition;
    private Vector2 randomStopPosition;
    private float waitTime;
    private float randomXPosition;
    private GameObject player;

    [SerializeField]
    private GameObject
    deathChunkParticle,
    deathBloodParticle;

    private enum State
    {
        Stationary,
        Patrolling,
        Attacking
    }

    private State currentState = State.Patrolling;

    //Enemy bullet variables
    public Transform shootingPosition;
    public GameObject bulletPrefab;
    public float weaponCoolDownTime;

    private float coolDownTime;
    private bool canShoot;

    //Lookout function variables
    private Vector2 lookDirection = Vector2.right;

    //Initialise patrol range and first random stop point
    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");

        leftEndPosition = new Vector2(minPatrolRangeXValue, transform.position.y);
        rightEndPosition = new Vector2(maxPatrolRangeXValue, transform.position.y);

        randomXPosition = Random.Range(minPatrolRangeXValue + 2, maxPatrolRangeXValue - 2);

        randomStopPosition = new Vector2(randomXPosition , transform.position.y);

        waitTime = startWaitTime;
        coolDownTime = weaponCoolDownTime;
    }

    void Update()
    {
        switch (currentState)
        {
            case State.Patrolling:
                Patrol();
                if (CanSeePlayer())
                {
                    currentState = State.Attacking;
                }
                break;

            case State.Attacking:
                if (CanSeePlayer())
                {
                    Shoot();

                }
                else
                {
                    if (IsPlayerOutOfSight())
                    {
                        Shoot();
                        FaceRandomWay();
                    }
                    else
                    {
                        CheckPlayerPosition();
                        Shoot();
                    }
                }
                break;
        }
    }

    //Function to make enemy patrol between min x value and max x value
    private void Patrol()
    {
        if (move)
        {
            transform.Translate(Vector2.right * speed * Time.deltaTime);
        }

        if (Vector2.Distance(transform.position, leftEndPosition) < 0.6f ||
            Vector2.Distance(transform.position, rightEndPosition) < 0.6f)
        {
            if (movingRight)
            {
                transform.eulerAngles = new Vector3(0, -180, 0);
                movingRight = false;
                canStop = true;
            }
            else
            {
                transform.eulerAngles = new Vector3(0, 0, 0);
                movingRight = true;
            }
        }

        if (Vector2.Distance(transform.position, randomStopPosition) < 0.25f && canStop)
        {
            move = false;
            waitTime -= Time.deltaTime;

            if (CanSeePlayer())
            {
                currentState = State.Attacking;
            }

            if (waitTime <= 0)
            {
                randomXPosition = Random.Range(minPatrolRangeXValue + 2, maxPatrolRangeXValue - 2);
                waitTime = startWaitTime;
                randomStopPosition = new Vector2(randomXPosition, transform.position.y);
                move = true;
                canStop = false;
                Debug.Log(randomStopPosition);
            }
        }
    }

    public bool CanSeePlayer()
    {

        if (movingRight)
        {
            lookDirection = Vector2.right;
            offset = new Vector3(0.75f, 0, 0);
        }
        else
        {
            lookDirection = Vector2.left;
            offset = new Vector3(-0.75f, 0, 0);
        }

        RaycastHit2D hit = Physics2D.Raycast(transform.position + offset, lookDirection, enemySightLength);
        Debug.DrawRay(transform.position + offset, lookDirection * enemySightLength, Color.green);

        if (hit.collider != null)
        {
            if (hit.collider.name == "Player")
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            return false;
        }
    }
  

    //Enemy to take damage
    public void TakeDamage(int damage)
    {
        health -= damage;

        if(health <= 0)
        {
            die();
        }
    }

    void die()
    {
        Instantiate(deathChunkParticle, gameObject.transform.position, deathChunkParticle.transform.rotation);
        Instantiate(deathBloodParticle, gameObject.transform.position, deathBloodParticle.transform.rotation);
        Destroy(gameObject);
    }

    //Enemy shoot function

    private void Shoot()
    {
        if (canShoot)
        {
            Instantiate(bulletPrefab, shootingPosition.position, shootingPosition.rotation);
            canShoot = false;
        }

        coolDownTime -= Time.deltaTime;

        if(coolDownTime <= 0)
        {
            coolDownTime = weaponCoolDownTime;
            canShoot = true;
        }
    }

    private void CheckPlayerPosition()
    {
        player = GameObject.FindGameObjectWithTag("Player");

        if (player.transform.position.x < transform.position.x && movingRight)
        {
            Debug.Log("Player has moved to the left.");
            transform.eulerAngles = new Vector3(0, -180, 0);
            movingRight = false;
        }
        else if (player.transform.position.x > transform.position.x && !movingRight)
        {
            Debug.Log("Player has moved to the right.");
            transform.eulerAngles = new Vector3(0, 0, 0);
            movingRight = true;
        }

    }

    private void FaceRandomWay()
    {
        waitTime -= Time.deltaTime;

        if(waitTime <= 0)
        {
            if (movingRight)
            {
                transform.eulerAngles = new Vector3(0, -180, 0);
                movingRight = false;
                waitTime = startWaitTime;
            }
            else
            {
                transform.eulerAngles = new Vector3(0, 0, 0);
                movingRight = true;
                waitTime = startWaitTime;
            }
        }
    }

    private bool IsPlayerOutOfSight()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        if (Vector2.Distance(transform.position, player.transform.position) < enemySightLength || Vector2.Distance(transform.position, player.transform.position) > -enemySightLength)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

[System.Serializable]
public struct PlayerDetectorCollider
{
    [SerializeField]
    public Collider2D[] collider;
    [SerializeField]
    public Vector2 offset;
    [SerializeField]
    public float lightRange;
}

public class MovingTurret : MonoBehaviour
{
    public Transform lightTransform;
    public GameObject bulletPrefab;
    EdgeCollider2D edgeCollider;
    public float bulletSpeed = 10f;
    public float conveyorSpeed = 2f;
    public Transform conveyorStart;
    public Transform conveyorEnd;
    public float fireRate = 2f;
    public Color originalLightColor = Color.white;
    public Color alertLightColor = Color.green;
    public Color firingLightColor = Color.red;
    public Color laserColor = Color.red;
    public float alertDuration = 2f;

    private bool movingTowardsEnd = true;
    private float nextFireTime = 0f;
    private bool isAlerting = false;
    private float alertEndTime = 0f;

    public float fireDelay = 0.5f;
    private bool canFire = false;

    LineRenderer laserBeam;
    bool isPlayerInsideLight = false;

    public PlayerDetectorCollider[] playerDetectorColliders;

    private void Start()
    {
        conveyorStart.parent = null;
        conveyorEnd.parent = null;
        laserBeam = GetComponent<LineRenderer>();
        edgeCollider = GetComponentInChildren<EdgeCollider2D>();

    }


    void OnDrawGizmos()
    {
        foreach(PlayerDetectorCollider playerDetectorCollider in playerDetectorColliders)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere((Vector2)transform.position + playerDetectorCollider.offset, playerDetectorCollider.lightRange);
        }
    }


    void FixedUpdate()
    {

        if (!isAlerting && !isPlayerInsideLight)
        {
            DisableLaserBeam();
            canFire = false;
        }
        // Move the turret along the conveyor system
        if (!isPlayerInsideLight && Time.time > alertEndTime)
        {
            Vector2 direction = movingTowardsEnd ? (conveyorEnd.position - transform.position).normalized : (conveyorStart.position - transform.position).normalized;
            transform.Translate(direction * conveyorSpeed * Time.deltaTime);
            lightTransform.GetComponent<Light2D>().color = originalLightColor;
        }

        // Check if the turret has reached the end of the conveyor system
        if (!isPlayerInsideLight && Vector2.Distance(transform.position, movingTowardsEnd ? conveyorEnd.position : conveyorStart.position) < 0.1f)
        {
            movingTowardsEnd = !movingTowardsEnd;
        }

        // Check if the player is inside the light
        isPlayerInsideLight = false;

        if (lightTransform.GetComponent<Light2D>().enabled)
        {
            for(int i = 0; i < playerDetectorColliders.Length; i++)
            {
                playerDetectorColliders[i].collider = Physics2D.OverlapCircleAll((Vector2)lightTransform.position + playerDetectorColliders[i].offset, playerDetectorColliders[i].lightRange);

                foreach (Collider2D collider in playerDetectorColliders[i].collider)
                {
                    if (collider.CompareTag("Player"))
                    {
                        // Check if there are any obstacles between the turret and the player
                        Vector2 directionToPlayer = (collider.transform.position - lightTransform.position);
                        RaycastHit2D hit = Physics2D.Linecast((Vector2)transform.position, (Vector2)transform.position + (Vector2)directionToPlayer * playerDetectorColliders[i].lightRange);
                        Debug.DrawLine((Vector2)transform.position, (Vector2)transform.position + (Vector2)directionToPlayer * playerDetectorColliders[i].lightRange, Color.red);
                        if (hit.collider == null || !hit.collider.CompareTag("Player"))
                        {
                            return;
                        }
                        else if (hit.collider.CompareTag("Player"))
                        {

                            isPlayerInsideLight = true;

                            // Alert the player and stop the turret if not already alerting
                            if (!isAlerting)
                            {
                                isAlerting = true;
                                lightTransform.GetComponent<Light2D>().color = alertLightColor;
                            }

                            // Fire at the player if enough time has passed since the last shot and the alert period has ended
                            if (!canFire && Time.time > nextFireTime && isAlerting && isPlayerInsideLight)
                            {
                                nextFireTime = Time.time / fireRate;
                                StartCoroutine(FireWithDelay(fireDelay));
                            }

                            if (canFire)
                            {
                                FireBullet(collider.transform.position);
                            }


                            break;
                        }
                    }
                }
            }
        }

        // If the player is not inside the light, reset the alert end time and the turret state
        if (!isPlayerInsideLight && isAlerting && Time.time > alertEndTime)
        {
            alertEndTime = Time.time + alertDuration;
            isAlerting = false;
        }

    }

    public LineRenderer laserPrefab;
    public float laserWidth = 0.2f;

    private LineRenderer currentLaser;
    private Vector3 laserDirection;
    public float laserStartWidth = 1;
    public float laserEndWidth = 1;

    IEnumerator FireWithDelay(float delay)
    {
        canFire = false;

        yield return new WaitForSeconds(delay);

        canFire = true;

    }

    void FireBullet(Vector2 targetPosition)
    {
        // Set the properties of the LineRenderer to create the laser beam
        laserBeam.enabled = true;
        edgeCollider.enabled = true;
        lightTransform.GetComponent<Light2D>().color = firingLightColor;
        laserBeam.startWidth = laserStartWidth;
        laserBeam.endWidth = laserEndWidth;
        laserBeam.material.color = laserColor;
        laserBeam.positionCount = 2;

        Vector3[] positions = new Vector3[] { transform.position, targetPosition };
        laserBeam.SetPositions(positions);

        // Set the positions of the two points to be the position of the turret's barrel and the position of the target that the laser is following
        laserBeam.SetPosition(0, transform.position);
        laserBeam.SetPosition(1, targetPosition);

        edgeCollider.isTrigger = true;
        edgeCollider.points = System.Array.ConvertAll<Vector3, Vector2>(positions, v3 => new Vector2(v3.x - transform.position.x, v3.y - transform.position.y));

        // Destroy the LineRenderer object after a short delay to make it look like the laser beam disappears

        // Play shooting sound effect
    }

    void DisableLaserBeam()
    {
        laserBeam.enabled = false;
        edgeCollider.enabled = false;
    }
}

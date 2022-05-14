using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingSpikes : MonoBehaviour
{
    Rigidbody2D rigidbody;
    CapsuleCollider2D capCollider;
    public float distance;
    bool isSpikeFalling = false;
    public float fallSpeed;
    public float detectionOffset;
    float originalDetectionOffset;
    PlayerMovement playerMovement;
    public bool isSprintingObstacle;

    private void Start()
    {
        originalDetectionOffset = detectionOffset;
        playerMovement = GameObject.Find("Player").GetComponent<PlayerMovement>();
        rigidbody = GetComponent<Rigidbody2D>();
        capCollider = GetComponent<CapsuleCollider2D>();
    }

    private void Update()
    {

        if (isSprintingObstacle)
        {
            if (playerMovement.getIsFastRunning())
            {
                detectionOffset = 0;
            }
            else
            {
                detectionOffset = originalDetectionOffset;
            }
        }

        Physics2D.queriesStartInColliders = false;
        if (!isSpikeFalling)
        {
            float boxColXExtents = capCollider.bounds.extents.x;
            RaycastHit2D hit = Physics2D.Raycast(new Vector2(transform.position.x - (boxColXExtents - detectionOffset), transform.position.y), Vector2.down, distance);

            Debug.DrawRay(new Vector2(transform.position.x - (boxColXExtents- detectionOffset), transform.position.y), Vector2.down * distance, Color.red);

            if(hit.transform != null)
            {
                if(hit.transform.tag == "Player")
                {
                    rigidbody.gravityScale = fallSpeed;
                    isSpikeFalling = true;
                }
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
           Destroy(gameObject, 0.05f); ;
        }
        else if(isSpikeFalling)
        {
            rigidbody.gravityScale = 0;
            capCollider.enabled = false;
        }

    }
}

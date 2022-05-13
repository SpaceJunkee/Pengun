using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingSpikes : MonoBehaviour
{
    Rigidbody2D rigidbody;
    BoxCollider2D boxCollider;
    public float distance;
    bool isSpikeFalling = false;
    public float fallSpeed;

    private void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();
    }

    private void Update()
    {
        Physics2D.queriesStartInColliders = false;
        if (!isSpikeFalling)
        {
            float boxColXExtents = boxCollider.bounds.extents.x;
            RaycastHit2D hit = Physics2D.Raycast(new Vector2(transform.position.x - boxColXExtents, transform.position.y), Vector2.down, distance);

            Debug.DrawRay(new Vector2(transform.position.x - boxColXExtents, transform.position.y), Vector2.down * distance, Color.red);

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
            boxCollider.enabled = false;
        }

    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveEnemyOnPoints : MonoBehaviour
{
    public Transform targetA, targetB;
    public float Speed = 200f;
    public Transform startPosition;
    Rigidbody2D rigidbody;
    SpriteRenderer spriteRenderer;
    public BoxCollider2D boxCollider2D;

    public Vector3 nextTarget;

    public bool isFlipped;

    public bool isMoving = true;

    private void Start()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        nextTarget = startPosition.position;
        rigidbody = this.GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {

        if (transform.position.x <= targetA.position.x)
        {
            //FlipMeshRenderer
            spriteRenderer.flipX = false;
            if (isFlipped)
            {
                FlipSlugCollider();
                isFlipped = false;
            }

            nextTarget = targetB.position;
        }
        else if (transform.position.x >= targetB.position.x)
        {
            //FlipMeshRenderer
            spriteRenderer.flipX = true;
            
            nextTarget = targetA.position;

            if (!isFlipped)
            {
                FlipSlugCollider();
                isFlipped = true;
            }
            
        }

        if (isMoving)
        {
            Vector2 direction = (this.transform.position - nextTarget).normalized;
            Vector2 force = direction * Speed * Time.deltaTime;
            rigidbody.AddForce(-force);
        }


    }

    void FlipSlugCollider()
    {
        boxCollider2D.transform.Rotate(0f, 180f, 0f);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(targetA.position, targetB.position);
    }
}

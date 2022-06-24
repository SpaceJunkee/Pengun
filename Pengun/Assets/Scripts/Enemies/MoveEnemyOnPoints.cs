using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveEnemyOnPoints : MonoBehaviour
{
    public Transform targetA, targetB;
    public float Speed = 200f;
    public Transform startPosition;
    Rigidbody2D rigidbody;
    Transform meshTransform;
    public float targetOffset = 0;
    //public BoxCollider2D boxCollider2D;

    public Vector3 nextTarget;

    public bool isFlipped;

    public bool isMoving = true;
    public bool hasReachedTargetA = false;
    public bool hasReachedTargetB = false;

    private void Start()
    {
        meshTransform = gameObject.transform.GetChild(0);
        nextTarget = startPosition.position;
        rigidbody = this.GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {

        if (transform.position.x -targetOffset <= targetA.position.x)
        {
            hasReachedTargetA = true;
            hasReachedTargetB = false;
            if (isFlipped)
            {
                //FlipMeshRenderer();
                //FlipSlugCollider();
                isFlipped = false;
            }

            nextTarget = targetB.position;
        }
        else if (transform.position.x + targetOffset >= targetB.position.x)
        {
            hasReachedTargetA = false;
            hasReachedTargetB = true;

            if (!isFlipped)
            {
                //FlipMeshRenderer();
                //FlipSlugCollider();
                isFlipped = true;
            }

            nextTarget = targetA.position;

        }

        if (isMoving)
        {
            Vector2 direction = (this.transform.position - nextTarget).normalized;
            Vector2 force = direction * Speed * Time.deltaTime;
            rigidbody.AddForce(-force);
        }


    }

    public void FlipMeshRenderer()
    {
        meshTransform.Rotate(0, 180, 0);
    }

/*    void FlipSlugCollider()
    {
        boxCollider2D.transform.Rotate(0f, 180f, 0f);
    }*/

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(targetA.position, targetB.position);
    }
}

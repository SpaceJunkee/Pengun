using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveEnemyOnPoints : MonoBehaviour
{
    public GameObject targetA, targetB;
    public Vector3 targetAOriginalPos, targetBOriginalPos;
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
        startPosition = targetA.transform;
        meshTransform = gameObject.transform.GetChild(0);
        nextTarget = startPosition.position;
        rigidbody = this.GetComponent<Rigidbody2D>();
        targetA.transform.parent = null;
        targetAOriginalPos = targetA.transform.position;
        targetB.transform.parent = null;
        targetBOriginalPos = targetB.transform.position;
    }

    private void FixedUpdate()
    {

        if (transform.position.x - targetOffset <= targetA.gameObject.transform.position.x)
        {
            hasReachedTargetA = true;
            hasReachedTargetB = false;
            if (isFlipped)
            {
                //FlipMeshRenderer();
                //FlipSlugCollider();
                isFlipped = false;
            }

            nextTarget = targetB.transform.position;
        }
        else if (transform.position.x + targetOffset >= targetB.transform.position.x)
        {
            hasReachedTargetA = false;
            hasReachedTargetB = true;

            if (!isFlipped)
            {
                //FlipMeshRenderer();
                //FlipSlugCollider();
                isFlipped = true;
            }

            nextTarget = targetA.transform.position;

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
        Gizmos.DrawLine(targetA.transform.position, targetB.transform.position);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePlatform : MonoBehaviour
{
    public Transform position1, position2;
    public float speed;
    public Transform startPosition;

    Vector3 nextPosition;

    private void Start()
    {
        nextPosition = startPosition.position;
    }

    private void FixedUpdate()
    {
        if(transform.position == position1.position)
        {
            nextPosition = position2.position;
        }
        else if(transform.position == position2.position)
        {
            nextPosition = position1.position;
        }

        transform.position = Vector3.MoveTowards(transform.position, nextPosition, speed * Time.deltaTime);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.collider.transform.SetParent(transform);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.collider.transform.SetParent(null);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(position1.position, position2.position);
    }
}

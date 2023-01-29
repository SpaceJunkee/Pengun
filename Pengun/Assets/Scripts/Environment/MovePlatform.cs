using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePlatform : MonoBehaviour
{
    public GameObject[] positions;
    int direction = 1;
    int currentPoint = 0;
    public float speed;
    public Transform startPosition;

    Vector3 nextPosition;

    private void Start()
    {
        nextPosition = startPosition.position;
    }

    private void FixedUpdate()
    {
        if (transform.position == positions[currentPoint].transform.position)
        {
            currentPoint = currentPoint + direction;
            if (currentPoint == positions.Length)
            {
                currentPoint = positions.Length - 2;
                direction = -1;
            }
            else if (currentPoint < 0)
            {
                currentPoint = 1;
                direction = 1;
            }
            nextPosition = positions[currentPoint].transform.position;
        }

        transform.position = Vector3.MoveTowards(transform.position, nextPosition, speed * Time.deltaTime);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if(transform.position.y < collision.transform.position.y - 0.8)
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
        for (int i = 0; i < positions.Length - 1; i++)
        {
            Gizmos.DrawLine(positions[i].transform.position, positions[i + 1].transform.position);
        }
    }
}

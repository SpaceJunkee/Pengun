using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerOtherObjectMover : MonoBehaviour
{

    public GameObject[] objectsToMove;
    public float movementSpeed;
    public Vector2 movementDirection;
    public bool hasAlreadyMoved = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (!hasAlreadyMoved)
            {
                foreach (GameObject obj in objectsToMove)
                {

                    obj.transform.Translate(movementDirection.normalized * movementSpeed * Time.deltaTime);

                }
                hasAlreadyMoved = true;
            }          
            
        }
    }
}

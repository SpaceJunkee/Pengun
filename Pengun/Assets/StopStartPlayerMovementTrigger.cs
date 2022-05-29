using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StopStartPlayerMovementTrigger : MonoBehaviour
{
    public bool stopMovement, startMovement;
    public float stopDelay, startDelay;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && stopMovement && !startMovement)
        {
            StartCoroutine("StopMovement");
        }
        else if (collision.CompareTag("Player") && startMovement && !stopMovement)
        {
            StartCoroutine("StartMovement");
        }
        
    }

    IEnumerator StopMovement()
    {
        yield return new WaitForSeconds(stopDelay);
        PlayerMovement.canMove = false;
    }

    IEnumerator StartMovement()
    {
        yield return new WaitForSeconds(startDelay);
        PlayerMovement.canMove = true;
    }
}

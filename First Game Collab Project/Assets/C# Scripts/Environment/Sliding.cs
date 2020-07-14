using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sliding : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //If the trap collides with player kill him
        if (collision.CompareTag("Player"))
        {
            

        }
        else
        {

        }
    }
}

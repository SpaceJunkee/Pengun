using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlidingSlope : MonoBehaviour

{
    
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag.Equals("Player"))
        {
            PlayerMovement.canMove = false;
            

        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag.Equals("Player"))
        {
            PlayerMovement.canMove = true;
           
            
        }
    }
    
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityFlipper : MonoBehaviour
{
    public float gravitySpeed;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<Rigidbody2D>().gravityScale *= -gravitySpeed;
            PlayerMovement.canUseInput = false;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<Rigidbody2D>().gravityScale = 4;
            PlayerMovement.canUseInput = true;

        }
    }
}

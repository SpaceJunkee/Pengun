using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetTriggerStop : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("PlayerDetector"))
        {
            if(collision.gameObject.GetComponentInParent<SlugromAI>() != null)
            {
                collision.gameObject.GetComponentInParent<SlugromAI>().isTouchingTarget = true;
            }
            
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponentInParent<SlugromAI>() != null)
        {
            collision.gameObject.GetComponentInParent<SlugromAI>().isTouchingTarget = false;
        }
    }
}

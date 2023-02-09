using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerTargetTeleportAnimation : MonoBehaviour
{

    public Animator playerAnimator;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            playerAnimator.SetBool("isFalling", true);
        }
        
    }
}

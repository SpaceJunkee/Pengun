using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpPad : MonoBehaviour
{
    public Animator animator;
    public float bounceHeight = 45f;
    public float requiredHeight = 0.5f; // Height above the JumpPad to trigger jump

    PlayerMovement playerMovement;
    Collider2D jumpPadCollider;

    private void Start()
    {
        playerMovement = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();
        jumpPadCollider = GetComponent<Collider2D>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") || collision.gameObject.layer == 14 || collision.gameObject.layer == 26)//Includes layers crates, nutsBolts
        {
            Vector2 contactPoint = collision.GetContact(0).point;
            float playerHeight = contactPoint.y - jumpPadCollider.bounds.min.y;

            if (playerHeight > requiredHeight * jumpPadCollider.bounds.size.y)
            {
                Rigidbody2D playerRigidbody = collision.gameObject.GetComponent<Rigidbody2D>();
                playerRigidbody.velocity = new Vector2(playerRigidbody.velocity.x, 0f); // Reset the player's vertical velocity
                playerRigidbody.AddForce(transform.up * bounceHeight, ForceMode2D.Impulse);
                playerMovement.isBouncing = true;

                if (collision.gameObject.CompareTag("Player")){
                    if (playerMovement.getIsFastRunning())
                    {
                        animator.SetTrigger("Jump");
                        animator.SetBool("isSprinting", false);
                    }
                    else
                    {
                        animator.SetTrigger("Jump");
                        animator.SetBool("isSprinting", false);
                    }
                }
                
            }
        }
    }


}

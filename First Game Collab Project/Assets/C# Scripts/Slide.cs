using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slide : MonoBehaviour
{

    public bool isSliding;

    public PlayerMovement pm;
    public Rigidbody2D rigidBody;

    public BoxCollider2D regularCollider;
    public BoxCollider2D slideCollider;
    public SpriteRenderer normalSprite;
    public SpriteRenderer slidingSprite;

    public float slideSpeed = 5f;
    public float slideLength = 0.8f;
    public float coolDownTime;

    private float nextSlideTime = 0;


    private void Update()
    {
        if(Time.time > nextSlideTime)
        {
            if (Input.GetButtonDown("Fire2") && pm.getIsGrounded() == true)
            {
                performSlide();
                nextSlideTime = Time.time + coolDownTime;
            }
        }
       
    }

    private void performSlide()
    {
        isSliding = true;

        regularCollider.enabled = false;
        slideCollider.enabled = true;
        normalSprite.enabled = false;
        slidingSprite.enabled = true;



        if (pm.getPlayerFaceRight() == true)
        {
            rigidBody.AddForce(Vector2.right * slideSpeed);
        }
        else
        {
            rigidBody.AddForce(Vector2.left * slideSpeed);
        }

        StartCoroutine("stopSliding");
    }

    IEnumerator stopSliding()
    {
        yield return new WaitForSeconds(slideLength);
        regularCollider.enabled = true;
        slideCollider.enabled = false;
        normalSprite.enabled = true;
        slidingSprite.enabled = false;
        isSliding = false;
    }
}

﻿using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    /* Public variables are seen in the unity editor and can be changed. 
     * Handy if you would like to change values on the fly.
     */

    //Variables
    
    private Rigidbody2D rigidbody;
    private float movementDirection;
    private RigidbodyConstraints2D originalConstraints;

    //Player speed
    public float movementSpeed = 8f;
    
    //Jumping
    public float jumpForce;
    public int maxJumpCount;
    public float fallMultiplier = 2.5f;
    public float smallJumpMultiplier = 2f;
    private int jumpCount;

    //Remember jump press so you can jump again before hitting the ground.
    float pressedJumpRemember = 0;
    float pressedJumpTime = 0.2f;

    //Checks
    public Transform ceilingCheck;
    public Transform groundCheck;
    public Transform wallCheck;
    public LayerMask groundObjects;
    public LayerMask wallObjects;
    public float checkRadius;
    private bool playerFaceRight = true;
    private bool isJumping = false;
    private bool isGrounded;
    private bool isTouchingWall;
    private bool isWallSliding;
    private bool isDashing;


    //Wall sliding and jumping
    public float wallCheckDistance;
    public float wallSlidingSpeed;
    public float maxWallSlideSpeed;
    public float hopSpeed;

    //Dashing
    public float dashTime;
    public float dashSpeed;
    public float distanceBetweenImages;
    public float dashCooldown;
    private float dashTimeLeft;
    private float lastImageXpos;
    private float lastDash = -100f;//Check when last dash was



    //Awake method is called before the start method when the objects are being initialized.
    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();//Will get a component on this object of type rigidbody.
        originalConstraints = rigidbody.constraints;
    }

    private void Start()
    {
        jumpCount = maxJumpCount;

    }

    // Update is called once per frame(updates every frame so if 60fps update runs 60 times per second)
    void Update()
    {
        ProcessInputs();

        fastWallSlide();

        FlipCharDirection();

        CheckIfWallSliding();

        WallHop();

    }

    //Better than update for physics handling like movement or gravity, can be called multiple times per update frame.
    private void FixedUpdate()
    {
        //Check if player is standing on the ground
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, groundObjects);

        //Check if player is touching a wall
        isTouchingWall = Physics2D.Raycast(wallCheck.position, transform.right, wallCheckDistance, wallObjects);

        checkIfCanJump();

        CheckDash();

        Move();
    }

    //Set up movement inputs for character
    private void ProcessInputs()
    {
        //Left and right movement
        
        movementDirection = Input.GetAxis("Horizontal");//Scale of -1 to 1 (-1 being left and 1 being right)

        //Dashing inputs
        if (Input.GetButtonDown("Dash") && !isWallSliding)
        {
            if(Time.time >=  (lastDash + dashCooldown))
            {
                AttemptDash();
            }
        }

        //Jumping inputs

        pressedJumpRemember -= Time.deltaTime;

        if (Input.GetButtonDown("Jump")) 
        {
            pressedJumpRemember = pressedJumpTime;
            isJumping = true;

        }

        if ((pressedJumpRemember > 0) && jumpCount > 0)
        {
            pressedJumpRemember = 0;
            rigidbody.velocity = Vector2.up * jumpForce;
            isJumping = true;

        }

    }

    //Moves Character 
    private void Move()
    {
        //Moves player in the y axis * the movement speed
        if (!isWallSliding)
        {
             rigidbody.velocity = new Vector2(movementDirection * movementSpeed, rigidbody.velocity.y);
        }

        //Set down velocity for wall sliding
        if (isWallSliding)
        {
            if (rigidbody.velocity.y < -wallSlidingSpeed)
            {
                rigidbody.velocity = new Vector2(rigidbody.velocity.x, -wallSlidingSpeed);
            }
        }

        //Jumping
        Jump();

    }

    private void AttemptDash()
    {
        isDashing = true;
        dashTimeLeft = dashTime;
        lastDash = Time.time;

        AfterImagePool.Instance.GetFromPool();
        lastImageXpos = transform.position.x;
    }

    private void CheckDash()
    {
        if (isDashing)
        {
            if (dashTimeLeft > 0 && playerFaceRight)
            {
                rigidbody.AddForce(Vector2.right * dashSpeed, 0.0f);
                rigidbody.constraints = RigidbodyConstraints2D.FreezePositionY;
                rigidbody.constraints = RigidbodyConstraints2D.FreezeRotation;

                dashTimeLeft -= Time.deltaTime;

                if (Mathf.Abs(transform.position.x - lastImageXpos) > distanceBetweenImages)
                {
                    AfterImagePool.Instance.GetFromPool();
                    lastImageXpos = transform.position.x;
                }
            }else if(dashTimeLeft > 0 && !playerFaceRight)
            {
                rigidbody.AddForce(Vector2.left * dashSpeed, 0.0f);
                rigidbody.constraints = RigidbodyConstraints2D.FreezePositionY;
                rigidbody.constraints = RigidbodyConstraints2D.FreezeRotation;

                dashTimeLeft -= Time.deltaTime;

                if (Mathf.Abs(transform.position.x - lastImageXpos) > distanceBetweenImages)
                {
                    AfterImagePool.Instance.GetFromPool();
                    lastImageXpos = transform.position.x;
                }
            }

            if (dashTimeLeft <= 0 || isTouchingWall)
            {
                isDashing = false;
                rigidbody.constraints = originalConstraints;

            }

        }
    }

    private void Jump()
    {
        //Lets the player jump at a maximum of 2 times normally and infinitley while wall jumping
        if (isJumping && !isWallSliding)
        {
            jumpCount--;
        }
        else if(isJumping && isWallSliding || isTouchingWall)
        {
            jumpCount = maxJumpCount;
        }

        /*This will create a bigger gravity spike when falling from the peak of a jump meaning you fall faster than you normally would
        It also allows for a quick button press to small jump and a big jump when button is held for longer.*/
        if (rigidbody.velocity.y < 0)
        {
            rigidbody.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
        }
        else if (rigidbody.velocity.y > 0 && !Input.GetButton("Jump"))
        {
            rigidbody.velocity += Vector2.up * Physics2D.gravity.y * (smallJumpMultiplier - 1) * Time.deltaTime;
        }

        isJumping = false;
    }

    private void checkIfCanJump()
    {
        if (isGrounded)
        {
            jumpCount = maxJumpCount;
        }

    }

    private void CheckIfWallSliding()
    {
        if(isTouchingWall && !isGrounded && rigidbody.velocity.y < 0)
        {
            isWallSliding = true;
            
        }
        else if(isTouchingWall && !isGrounded && rigidbody.velocity.y > 0)
        {
            isWallSliding = true;
            
        }
        else
        {
            isWallSliding = false;

        }

        if(isWallSliding && (movementDirection < 0 || movementDirection > 0) && Input.GetButtonDown("Jump"))
        {
            isWallSliding = false;
            
        }

       
    }

    //Lets the player slide down the wall faster when pushing slide button.
    private void fastWallSlide()
    {
        if(isWallSliding && Input.GetButtonDown("Fire2"))
        {
            wallSlidingSpeed = maxWallSlideSpeed;
        }
        else if (isGrounded ||isJumping)
        {
            wallSlidingSpeed = 1.5f;
        }
    }

    //Wall hop lets the player jump off the wall without jumping 
    private void WallHop()
    {
        if(isWallSliding && Input.GetButtonDown("Dash") && playerFaceRight && movementDirection < 0)
        {
            rigidbody.AddForce(Vector2.left * hopSpeed, 0.0f);
            
        }else if(isWallSliding && Input.GetButtonDown("Dash") && !playerFaceRight && movementDirection > 0)
        {
            rigidbody.AddForce(Vector2.right * hopSpeed, 0.0f);
          
        }
    }

   
    //Flips character rotation depending on which way the character is facing
    public void FlipCharDirection()
    {
        if (movementDirection > 0 && !playerFaceRight && !isWallSliding)
        {
            TurnCharacterDirection();    
        }
        else if (movementDirection < 0 && playerFaceRight && !isWallSliding)
        {
            TurnCharacterDirection();        
        }
        
    }


    //Turns character
    private void TurnCharacterDirection()
    {
            playerFaceRight = !playerFaceRight; //Opposite direction
            transform.Rotate(0f, 180f, 0f);
    }

    //Getters
    public bool getPlayerFaceRight()
    {
        return playerFaceRight;
    }

    public bool getIsGrounded()
    {
        return isGrounded;
    }

    public bool getIsWallSliding()
    {
        return isWallSliding;
    }

}


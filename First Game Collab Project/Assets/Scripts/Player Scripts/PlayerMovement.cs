using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UIElements;
using Vector2 = UnityEngine.Vector2;
using Spine.Unity;
using UnityEngine.Events;

public class PlayerMovement : MonoBehaviour
{

    /* Public variables are seen in the unity editor and can be changed. 
     * Handy if you would like to change values on the fly.
     */

    //Variables
    public CassetteTapes cassetteTapes;
    private Rigidbody2D rigidbody;
    private float movementDirection;
    public static RigidbodyConstraints2D originalConstraints;
    public HurtKnockBack hurtKnockBack;

    //Timer
    private IEnumerator coroutine;

    //Animation
    public Animator animator;

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
    public float inAirTime = 0.1f;

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
    private bool isStandingOnLava;
    private bool isTouchingWall;
    private bool isWallSliding;
    public static bool isDashing;
    public static bool canMove = true;



    //Wall sliding and jumping
    public float wallCheckDistance;
    public float wallSlidingSpeed;
    public float maxWallSlideSpeed;
    public float hopSpeed;
    public float wallClimbStamina;
    public float originalWallClimbStamina = 30f;

    //Dashing
    public int dashCount;
    public int maxDashInAir = 1;
    public float dashTime;
    public float dashSpeed;
    public float distanceBetweenImages;
    public float dashCooldown;
    private float dashTimeLeft;
    private float lastImageXpos;
    private float lastDash = -100f;//Check when last dash was

    //Fast run
    private bool canFastRun = true;
    public TrailRenderer speedTrail;

    //Falling
    public static bool isfalling = false;
    public float fallingTime = 1;

    //particle
    public ParticleSystem particleSystem;
    public ParticleSystem musicDirectionParticles;


    //Awake method is called before the start method when the objects are being initialized.
    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        originalConstraints = rigidbody.constraints;
    }

    private void Start()
    {
        jumpCount = maxJumpCount;
        dashCount = maxDashInAir;
    }

    // Update is called once per frame(updates every frame so if 60fps update runs 60 times per second)
    void Update()
    {
        if (canMove)
        {
            ProcessInputs();
        }

        CheckIfFalling();

        FlipCharDirection();

        CheckIfWallSliding();

        WallHop();

        //Animation calls
        RunAnim();
        FastRunAnim();

    }

    //Better than update for physics handling like movement or gravity, can be called multiple times per update frame.
    private void FixedUpdate()
    {

        //Check if player is standing on the ground
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, groundObjects);
        isStandingOnLava = hurtKnockBack.getIsStandingOnLava();

        //Check if player is touching a wall
        isTouchingWall = Physics2D.Raycast(wallCheck.position, transform.right, wallCheckDistance, wallObjects);

        checkIfCanJump();

        CheckDash();

        FastRun();

        if (canMove)
        {
            Move();
        }
        
    }
    void OnDrawGizmosSelected()
    {
        // Draw a yellow sphere at the transform's position
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(groundCheck.position, checkRadius);
    }


    //Set up movement inputs for character
    private void ProcessInputs()
    {

        ManageCassetteTapes();

        //Left and right movement

        movementDirection = Input.GetAxis("Horizontal");

        if (isGrounded && movementDirection != 0 || isStandingOnLava && movementDirection != 0)
        {
            animator.SetBool("Running", true);
        }
        else if (isGrounded && movementDirection == 0 || isStandingOnLava && movementDirection == 0)
        {
            animator.SetBool("Running", false);
        }

        //Deadzone makes player move at full speed 
        if (movementDirection < 0)
        {
            movementDirection = Math.Min(movementDirection, -1);
        }
        else if(movementDirection > 0)
        {
            movementDirection = Math.Max(movementDirection, 1);
        }

        //Dashing inputs
        if (Input.GetButtonDown("Dash") && !isWallSliding)
        {
            if(Time.time >=  (lastDash + dashCooldown))
            {
                AttemptDash();
            }
        }

        //Parry
        if (Input.GetButtonDown("Parry") && !isWallSliding)
        {
            Parry();
        }

        //Jumping inputs

        pressedJumpRemember -= Time.deltaTime;
        

        if (Input.GetButtonDown("Jump")) 
        {
            pressedJumpRemember = pressedJumpTime;
            isJumping = true;

            if (isGrounded || isStandingOnLava)
            {
                animator.SetTrigger("Jump");
            }
            
        }

        if ((pressedJumpRemember > 0) && jumpCount > 0)
        {
            animator.SetTrigger("Jump");
            pressedJumpRemember = 0;
            rigidbody.velocity = Vector2.up * jumpForce;
            isJumping = true;
        }

        if (!isGrounded && !isTouchingWall && !isStandingOnLava)
        {
            inAirTime -= Time.deltaTime;
            
            if(inAirTime < 0)
            {
                jumpCount = 0;
                isJumping = false;
            }
            
        }
        else
        {
            inAirTime = 0.1f;
        }

    }

    //Parry
    private void Parry()
    {
        animator.SetTrigger("Parry");
        particleSystem.Play();
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

        animator.SetTrigger("Dash");

        if (!isGrounded && dashCount != 0)
        {
            isDashing = true;
            dashTimeLeft = dashTime;
            lastDash = Time.time;
            

            AfterImagePool.Instance.GetFromPool();
            lastImageXpos = transform.position.x;

            dashCount = 0;
        }
        else if(isGrounded && !isTouchingWall)
        {
            isDashing = true;
            dashTimeLeft = dashTime;
            lastDash = Time.time;

            AfterImagePool.Instance.GetFromPool();
            lastImageXpos = transform.position.x;

        }
        
    }

    private void CheckIfFalling()
    {
        if (!isGrounded && !isJumping && !isWallSliding)
        {
            fallingTime -= Time.deltaTime;

            if(fallingTime <= 0.1)
            {
                isfalling = true;
            }
        }

        if (isGrounded)
        {
            fallingTime += 0.4f;
            fallingTime += Time.deltaTime;

            if(fallingTime >= 0.5)
            {
                isfalling = false;
                fallingTime = 1;
            }
            
        }
    }

    private void CheckDash()
    {
        if(isGrounded || isWallSliding)
        {
            dashCount = 1;
        }

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

        //Lets the player jump at a maximum of 1 times normally and infinitley while wall jumping
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
        if (isGrounded || isStandingOnLava)
        {
            jumpCount = maxJumpCount;
        }

    }

    private void CheckIfWallSliding()
    {
        if(isTouchingWall && !isGrounded && rigidbody.velocity.y < 0)
        {
            isWallSliding = true;
            wallSlidingSpeed = -2f;
            wallClimbStamina -= Time.deltaTime;
        }
        else if(isTouchingWall && !isGrounded && rigidbody.velocity.y > 0)
        {
            isWallSliding = true;
            wallSlidingSpeed = -2f;
            wallClimbStamina -= Time.deltaTime;

        }
        else
        {
            isWallSliding = false;

        }
     

        if (isWallSliding && (movementDirection < 0 || movementDirection > 0) && Input.GetButtonDown("Jump"))
        {
            isWallSliding = false;
        }

        if(isTouchingWall && Input.GetButtonDown("Jump"))
        {
            wallClimbStamina = originalWallClimbStamina;
        }

        if (wallClimbStamina <= 0)
        {
            //Fast wall slide
            if (isWallSliding && Input.GetAxis("Vertical") < 0)
            {
                wallSlidingSpeed = maxWallSlideSpeed;
            }
            else
            {
                wallSlidingSpeed = 3.5f;
            }
                
        }else if(wallClimbStamina >= 0 && Input.GetAxis("Vertical") < 0)
        {
            wallSlidingSpeed = maxWallSlideSpeed;
        }

        if (isGrounded || !isWallSliding)
        {
            wallClimbStamina = originalWallClimbStamina;
        }



    }

    private void FastRun()
    {
        if (Input.GetButton("FastRun") && canFastRun && (movementDirection > 0 || movementDirection < 0))
        {
            movementSpeed = 16;

            if (isGrounded)
            {
                speedTrail.emitting = true;
            }
            
        }
        else
        {
            movementSpeed = 13f;

            if (isGrounded)
            {
                speedTrail.emitting = false;
            }
        }
    }

    //Wall hop lets the player jump off the wall without jumping 
    private void WallHop()
    {
        if(isWallSliding && Input.GetButtonDown("Dash") && playerFaceRight && movementDirection < 0)
        {
            rigidbody.AddForce(Vector2.left * hopSpeed, 0.0f);
            
        }
        else if(isWallSliding && Input.GetButtonDown("Dash") && !playerFaceRight && movementDirection > 0)
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

    public void StopPlayer()
    {
        animator.SetBool("Running", false);
        rigidbody.velocity = Vector2.zero;
        rigidbody.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezePositionY;
    }


    //Turns character
    private void TurnCharacterDirection()
    {
        playerFaceRight = !playerFaceRight; //Opposite direction
        transform.Rotate(0f, 180f, 0f);
    }

    private void ManageCassetteTapes()
    {
        if (DPadButtons.IsLeft)
        {
            cassetteTapes.ChangeToBaseTrackLeft();

            RotateMusicParticles(180f, new Color32(0,255,177,255));
            
        }
        else if (DPadButtons.IsRight)
        {
            cassetteTapes.ChangeToTrackRight();

            RotateMusicParticles(0f, new Color32(255, 42,0,255));
        }
        else if (DPadButtons.IsUp)
        {
            cassetteTapes.ChangeToTrackUp();

            RotateMusicParticles(90f, new Color32(255,252,0,255));
        }
        else if (DPadButtons.IsDown)
        {
            cassetteTapes.ChangeToTrackDown();

            RotateMusicParticles(270f, new Color32(0, 200, 255, 255));
        }
    }

    void RotateMusicParticles(float zValue, Color32 color)
    {
        musicDirectionParticles.startColor = color;
        Quaternion target = Quaternion.Euler(0, 0, zValue);
        musicDirectionParticles.transform.rotation = target;
        musicDirectionParticles.Play();
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

    public bool getIsDashing()
    {
        return isDashing;
    }

    public bool getIsWallSliding()
    {
        return isWallSliding;
    }

    public Transform getGroundCheck()
    {
        return groundCheck;
    }

    public float getCheckRadius()
    {
        return checkRadius;
    }

    public float getMovementDirection()
    {
        return movementDirection;    }

    //Animation methods

    //Run
    private void RunAnim()
    {
        //animator.SetFloat("MovementSpeed", Mathf.Abs(movementDirection)); 
    }

    private void FastRunAnim()
    {
    
    }
   
}


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
    public HealthManager healthManager;
    public PlayerDamageController playerDamageController;
    public TimeManager timemanager;
    public ParticleSystem dustParticles;
    public ParticleSystem dashBubblesParticles;
    public ParticleSystem dashLinesParticles;
    public ParticleSystem dashPopParticles;
    public ParticleSystem speedTrailParticles;
    public AudioSource dashAudio;
    public AudioSource bloodWaveAudio;
    public GameObject BloodSlamBlast;
    public SkeletonMecanim skeletonMec;
    GromEnergyBarController gromEnergyBarController;
    Color mecanimColor;
    Color dashBlackColor;
    bool isRightTriggerInUse = false;

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
    public bool isJumping = false;
    public static bool hasJustLeftTheGroundAfterJumping = false;
    private bool isGrounded;
    private bool isStandingOnLava;
    private bool isTouchingWall;
    private bool isWallSliding;
    public static bool isDashing;
    public static bool canMove;
    public static bool canUseButtonInput = true;
    public static bool canUseMovementInput = true;

    //Wall sliding and jumping
    public float wallCheckDistance;
    public float wallSlidingSpeed;
    public float maxWallSlideSpeed;
    public float hopSpeed;
    public float wallJumpForce = 18f;
    public float wallJumpDirection = -1;
    public Vector2 wallJumpAngle;

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

    //Falling
    public static bool isfalling = false;
    public float fallingTime = 1;

    //Music Abilities
    private bool canFastRun = false;
    private bool isFastRunning = false;
    private bool hasArmour;
    private bool isBerzerkModeActivated = false;
    private bool hasSelectedTimedTape = false;
    private bool hasSelectedSpecialTape = false;
    private bool hasSelectedConsumableTape = false;

    //Music Ability variables
    int damageMultiplier = 2;
    int originalDamageMultiplier = 1;


    //Awake method is called before the start method when the objects are being initialized.
    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        originalConstraints = rigidbody.constraints;
    }

    private void Start()
    {
        gromEnergyBarController = GameObject.Find("GromEnergyBarController").GetComponent<GromEnergyBarController>();
        canMove = true;
        jumpCount = maxJumpCount;
        dashCount = maxDashInAir;

        mecanimColor = skeletonMec.skeleton.GetColor();
        dashBlackColor = Color.blue;
        dashBlackColor.a = Mathf.Clamp(0.3f, 0, 1);
    }

    // Update is called once per frame(updates every frame so if 60fps update runs 60 times per second)
    void Update()
    {
        if(rigidbody.velocity.y < -7|| isGrounded)
        {
            isJumping = false;
        }

        if (canMove)
        {
            ProcessInputs();
            FlipCharDirection();

            //Animation calls
            RunAnim();
            FastRunAnim();
        }


        CheckIfFalling();
        
        if (isBerzerkModeActivated)
        {
            playerDamageController.setMeleeDamageOutput(damageMultiplier);
        }
        else
        {
            playerDamageController.setMeleeDamageOutput(originalDamageMultiplier);
        }

    }

    //Better than update for physics handling like movement or gravity, can be called multiple times per update frame.
    private void FixedUpdate()
    {

        CheckIfWallSliding();
        WallJump();

        //Check if player is standing on the ground
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, groundObjects);

        if (isGrounded)
        {
            isJumping = false;
        }

        animator.SetBool("Grounded", isGrounded);
        isStandingOnLava = hurtKnockBack.getIsStandingOnLava();

        //Check if player is touching a wall
        isTouchingWall = Physics2D.Raycast(wallCheck.position, transform.right, wallCheckDistance, wallObjects);

        CheckDash();

        if (canMove)
        {
            Move();

            checkIfCanJump();           

            FastRun();
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
        ManageSpecialAbilities();
        ManageCassetteTapes();
        OpenRadialMenu();

        animator.SetBool("IsJumping", isJumping);

        if (Input.GetAxis("LeftTrigger") == 1)
        {
            canFastRun = true;

        }
        else
        {
            canFastRun = false;
        }

        //Left and right movement

        movementDirection = Input.GetAxis("Horizontal");

        if (isGrounded && movementDirection != 0 || isStandingOnLava && movementDirection != 0 || !isGrounded && movementDirection != 0)
        {
            animator.SetBool("Running", true);
        }
        else if (isGrounded && movementDirection == 0 || isStandingOnLava && movementDirection == 0)
        {
            animator.SetBool("Running", false);
        }
        else if (!isGrounded)
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

        if (canUseButtonInput)
        {
            //Dashing inputs
            if (Input.GetButtonDown("Dash") && !isWallSliding)
            {
                if (Time.time >= (lastDash + dashCooldown))
                {
                    AttemptDash();
                }
            }

            //Jumping inputs

            pressedJumpRemember -= Time.deltaTime;


            if (Input.GetButtonDown("Jump"))
            {
                pressedJumpRemember = pressedJumpTime;
                StartCoroutine("ToggleIsJumping");
                StartCoroutine("ToggleHasJustLeftGround");

                if ((isGrounded || isStandingOnLava) || (inAirTime > 0 && !isGrounded && !isStandingOnLava))
                {
                    animator.SetTrigger("Jump");
                    CreateDustParticles();
                }

            }
        }
        

        if ((pressedJumpRemember > 0) && jumpCount > 0)
        {
            //Still plays jump animation when remembering jump.
            if ((isGrounded || isStandingOnLava) || (inAirTime > 0 && !isGrounded && !isStandingOnLava))
            {
                animator.SetTrigger("Jump");
                CreateDustParticles();
            }

            pressedJumpRemember = 0;
            rigidbody.velocity = Vector2.up * jumpForce;
            StartCoroutine("ToggleIsJumping");
            StartCoroutine("ToggleHasJustLeftGround");
        }

        if (!isGrounded && !isTouchingWall && !isStandingOnLava)
        {
            inAirTime -= Time.deltaTime;
            
            if(inAirTime < 0)
            {
                jumpCount = 0;
            }
            
        }
        else
        {
            inAirTime = 0.1f;
        }

    }

    IEnumerator ToggleHasJustLeftGround()
    {
        hasJustLeftTheGroundAfterJumping = true;
        yield return new WaitForSeconds(0.3f);
        hasJustLeftTheGroundAfterJumping = false;
    }

    IEnumerator ToggleIsJumping()
    {       
        yield return new WaitForSeconds(0.1f);
        isJumping = true;       
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

    private void CheckIfFalling()
    {
        if (!isGrounded && !isJumping && !isWallSliding)
        {
            fallingTime -= Time.deltaTime;

            if (fallingTime <= -0.2f)
            {
                isfalling = true;
                animator.SetBool("isFalling", true);
            }
        }

        if (isGrounded)
        {
            fallingTime += 0.4f;
            fallingTime += Time.deltaTime;

            if (fallingTime >= 0.5)
            {
                isfalling = false;
                animator.SetBool("isFalling", false);
                fallingTime = 1;
            }

        }
    }

    private void PerformDashEffectsAndAnims()
    {
        animator.SetTrigger("Dash");
        dashBubblesParticles.Play();
        dashLinesParticles.Play();
        dashPopParticles.Play();
        skeletonMec.skeleton.SetColor(dashBlackColor);
        CameraShake.Instance.ShakeCamera(3f, 0.075f, 0.3f);
        PostProcessingController.myVignette.active = true;
        dashAudio.Play();
    }

    private void AttemptDash()
    {
        if (!isGrounded && dashCount != 0 && !P_Melee.isMelee)
        {
            isDashing = true;
            dashCount--;
            animator.SetBool("isDashFinished", false);
            dashTimeLeft = dashTime;
            lastDash = Time.time;
            PerformDashEffectsAndAnims();

            AfterImagePool.Instance.GetFromPool();
            lastImageXpos = transform.position.x;
         
        }
        else if(isGrounded && !isTouchingWall && !P_Melee.isMelee)
        {
            isDashing = true;
            animator.SetBool("isDashFinished", false);
            dashTimeLeft = dashTime;
            lastDash = Time.time;
            PerformDashEffectsAndAnims();
            AfterImagePool.Instance.GetFromPool();
            lastImageXpos = transform.position.x;

        }
        
    }

    private void CheckDash()
    {
        if(isGrounded || isWallSliding)
        {
            dashCount = 2;
        }

        if (isDashing)
        {
            canMove = false;
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

            if ((dashTimeLeft <= 0 && !KillPlayer.isDead) || isTouchingWall)
            {
                isDashing = false;
                animator.SetBool("isDashFinished", true);
                skeletonMec.skeleton.SetColor(mecanimColor);
                PostProcessingController.myVignette.active = false;
                canMove = true;
                rigidbody.constraints = originalConstraints;
            }
           
            if(dashTimeLeft <= 0 && KillPlayer.isDead)
            {
                rigidbody.velocity = Vector2.zero;
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

        if (rigidbody.velocity.y < 0 && isGrounded)
        {
            rigidbody.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
        }
        else if (rigidbody.velocity.y > 0 && !Input.GetButton("Jump"))
        {
            rigidbody.velocity += Vector2.up * Physics2D.gravity.y * (smallJumpMultiplier - 1) * Time.deltaTime;
        }

    }

    private void checkIfCanJump()
    {
        if (isGrounded || isStandingOnLava )
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
        }
        else if(isTouchingWall && !isGrounded && rigidbody.velocity.y > 0)
        {
            isWallSliding = true;
            wallSlidingSpeed = -2f;
        }
        else
        {
            isWallSliding = false;
        }

    }

    void WallJump()
    {

        if (isWallSliding && isTouchingWall && Input.GetButtonDown("Jump"))
        {
            rigidbody.AddForce(new Vector2(wallJumpForce * wallJumpDirection * wallJumpAngle.x, wallJumpForce * wallJumpAngle.y), ForceMode2D.Impulse);

            //animator.SetBool("isWallSliding", false);
            //animator.SetTrigger("WallClimb");            
        }

    }

    private void FastRun()
    {
        if (canFastRun && (movementDirection > 0 || movementDirection < 0))
        {
            isFastRunning = true;
            movementSpeed = 18f;

            if (isGrounded)
            {
                animator.SetBool("isSprinting", true);
            }

            animator.SetBool("Running", false);
            if (isGrounded)
            {
                speedTrailParticles.Play();
            } 
            
        }
        else
        {
            isFastRunning = false;
            movementSpeed = 13f;
            animator.SetFloat("SpeedMultiplier", 1f);
            animator.SetBool("Running", true);
            animator.SetBool("isSprinting", false);
            speedTrailParticles.Stop();
            
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

    public void StopPlayer(bool freezeY, bool freezeX, bool stopVelocity)
    {
        canMove = false;
        animator.SetBool("Running", false);

        if (stopVelocity)
        {
            rigidbody.velocity = Vector2.zero;
        }
        
        if(freezeY && freezeX)
        {
            rigidbody.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezePositionY;
        }
        else if(!freezeY && freezeX)
        {
            rigidbody.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;
        }
        else if(freezeY && !freezeX)
        {
            rigidbody.constraints = RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation;
        }
        
    }

    public void EnableMovement()
    {
        canMove = true;
        rigidbody.constraints = originalConstraints;
    }


    //Turns character
    private void TurnCharacterDirection()
    {
        playerFaceRight = !playerFaceRight; //Opposite direction
        transform.Rotate(0f, 180f, 0f);
        this.transform.Find("AbilityWheel").transform.Rotate(0f, 180f, 0f);
        wallJumpDirection *= -1;
        if (isGrounded)
        {
            CreateDustParticles();
        }
        
    }
    
    bool hasTapeChanged1, hasTapeChanged2, hasTapeChanged3 = false;

    private void ManageCassetteTapes()
    {
        if (RadialMenuScript.selection == 2 && !hasTapeChanged1)
        {
            cassetteTapes.ChangeToBaseTrackUp();
            ManageMusicAbilities(false, true, false);
            ResetHasTrackedChanged(true, false, false);
        }
        else if (RadialMenuScript.selection == 1 && !hasTapeChanged2)
        {
            cassetteTapes.ChangeToTrackRight();
            ManageMusicAbilities(true, false, false);
            FastRun();
            ResetHasTrackedChanged(false, true, false);
        }
        else if (RadialMenuScript.selection == 0 && !hasTapeChanged3)
        {
            cassetteTapes.ChangeToTrackLeft();
            ManageMusicAbilities(false, false, true);
            ResetHasTrackedChanged(false, false, true);
        }
        
    }

    void ManageMusicAbilities(bool isTimedTapes, bool isConsumableTapes, bool isSpecialTapes)
    {
        canFastRun = isTimedTapes;
        isBerzerkModeActivated = isSpecialTapes;

        hasSelectedConsumableTape = isConsumableTapes;
        hasSelectedTimedTape = isTimedTapes;
        hasSelectedSpecialTape = isSpecialTapes;
    }

    void ManageSpecialAbilities()
    {
        if (Input.GetAxisRaw("RightTrigger") == 1 && hasSelectedConsumableTape)
        {
            if (!isRightTriggerInUse)
            {
                isRightTriggerInUse = true;
                Debug.Log("Armour");
            }
            
        }
        else if (Input.GetAxisRaw("RightTrigger") == 1 && hasSelectedTimedTape)
        {
            if (!isRightTriggerInUse)
            {
                isRightTriggerInUse = true;
                Debug.Log("Speed");
            }
        }
        else if (Input.GetAxisRaw("RightTrigger") == 1 && hasSelectedSpecialTape)
        {
            //Play animation
            if (!isWallSliding && canUseButtonInput && !isRightTriggerInUse)
            {
                isRightTriggerInUse = true;
                ActivateBloodWave();
            }
        }
        
        if(Input.GetAxisRaw("RightTrigger") == 0)
        {
            if (isRightTriggerInUse)
            {
                isRightTriggerInUse = false;
            }
        }
    }


    private void ActivateBloodWave()
    {
        //bloodWaveAudio.Play();
        if (!isJumping && isGrounded)
        {
            rigidbody.velocity = Vector2.up * jumpForce * 1.3f;
            Invoke("StopPlayer", 0.2f);
            Invoke("BloodBlast", 0.2f);
        }
        else
        {
            StopPlayer(true, true, true);
            BloodBlast();
        }
        
       // canMove = false;
        CameraShake.Instance.ShakeCamera(4f, 2f, 1.5f);
        
        
        Invoke("EnableMovement", 1.5f);
        Debug.Log("Strength");
    }

    void BloodBlast()
    {
        Instantiate(BloodSlamBlast, this.transform.position, Quaternion.identity);
    }


    bool isNotInMenu = false;

    void OpenRadialMenu()
    {
        if (Input.GetButton("OpenAbilityMenu") && canMove)
        {
            timemanager.StartSlowMotion(0.1f);
            RadialMenuScript.isActive = true;
            isNotInMenu = false;
            canUseButtonInput = false;
        }
        else if(isNotInMenu == false)
        {
            timemanager.StopSlowMotion();
            RadialMenuScript.isActive = false;
            isNotInMenu = true;
            canUseButtonInput = true;
        }
        else
        {
            return;
        }
    }

    void ResetHasTrackedChanged(bool track1, bool track2, bool track3)
    {
        hasTapeChanged1 = track1;
        hasTapeChanged2 = track2;
        hasTapeChanged3 = track3;
    }

    //Getters
    public bool getPlayerFaceRight()
    {
        return playerFaceRight;
    }

    public bool getIsFastRunning()
    {
        return isFastRunning;
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
        return movementDirection;    
    }

    public Rigidbody2D getRigidbody2D()
    {
        return rigidbody;
    }

    //Animation methods

    //Run
    private void RunAnim()
    {
        //animator.SetFloat("MovementSpeed", Mathf.Abs(movementDirection)); 
    }

    private void FastRunAnim()
    {
    
    }

    public void CreateDustParticles()
    {
        dustParticles.Play();
    }

}


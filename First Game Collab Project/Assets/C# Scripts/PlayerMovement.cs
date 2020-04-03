using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    /* Public variables are seen in the unity editor and can be changed. 
     * Handy if you would like to change values on the fly.
     */
    
    //Variables
    public float movementSpeed;
    public float jumpForce;
    public Transform ceilingCheck;
    public Transform groundCheck;
    public LayerMask groundObjects;
    public float checkRadius;
    public int maxJumpCount;
    public float fallMultiplier = 2.5f;


    private Rigidbody2D rigidbody;
    private bool playerFaceRight = true;
    private float movementDirection;
    private bool isJumping = false;
    private bool isGrounded;
    private int jumpCount;

    //Awake method is called before the start method when the objects are being initialized.
    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();//Will get a component on this object of type rigidbody.
    }

    private void Start()
    {
        jumpCount = maxJumpCount;
    }

    // Update is called once per frame(updates every frame so if 60fps update runs 60 times per second)
    void Update()
    {
        ProcessInputs();
        
        FlipCharDirection();
    }

    //Better than update for physics handling like movement or gravity, can be called multiple times per update frame.
    private void FixedUpdate()
    {
        //Check if player is standing on the ground
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, groundObjects);

        if (isGrounded)
        {
            jumpCount = maxJumpCount;
        }

        Move();
    }

    //Set up movement inputs for character
    private void ProcessInputs()
    {
        //Left and right movement
        movementDirection = Input.GetAxis("Horizontal");//Scale of -1 to 1 (-1 being left and 1 being right)

        //Jumping
        if (Input.GetButtonDown("Jump") && jumpCount > 0) 
        {
            rigidbody.velocity = Vector2.up * jumpForce;
            isJumping = true;
        }
    }

    //Moves Character 
    private void Move()
    {
        //Moves player in the y axis * the movement speed
        rigidbody.velocity = new Vector2(movementDirection * movementSpeed, rigidbody.velocity.y);

        //Lets the player jump
        if (isJumping)
        {
            jumpCount--;
        }

        if (rigidbody.velocity.y < 0)
        {
            rigidbody.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
        }

    isJumping = false;
    }
    
    //Flips character rotation depending on which way the character is facing
    private void FlipCharDirection()
    {
        if (movementDirection > 0 && !playerFaceRight)
        {
            TurnCharacterDirection();
        }
        else if (movementDirection < 0 && playerFaceRight)
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
}

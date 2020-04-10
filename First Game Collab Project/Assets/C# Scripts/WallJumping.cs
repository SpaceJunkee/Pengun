using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallJumping : MonoBehaviour
{

    private int facingDirection  =1;

    public PlayerMovement pm;
    public Rigidbody2D rigidBody;

    public Vector2 wallHopDirection;
    public Vector2 wallJumpDirection;

    //Store force to hop or jump off wall
    public float wallHopForce;
    public float wallJumpForce;

    public Transform wallCheck;


    private void Start()
    {
        //Makes the vector equal to 1
        wallHopDirection.Normalize();
        wallJumpDirection.Normalize();
    }
}

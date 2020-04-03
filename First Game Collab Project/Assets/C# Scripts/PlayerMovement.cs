using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    /* Public variables are seen in the unity editor and can be changed. 
     * Handy if you would like to change values on the fly.
     */
    
    //Variables
    public float movementSpeed;

    private Rigidbody2D rigidbody;
    private bool playerFaceRight = true;
    private float movementDirection;

    //Awake method is called before the start method when the objects are being initialized.
    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();//Will get a component on this object of type rigidbody.
    }

    // Update is called once per frame(updates every frame so if 60fps update runs 60 times per second)
    void Update()
    {
        //Get inputs for movement
        movementDirection = Input.GetAxis("Horizontal");//Scale of -1 to 1 (-1 being left and 1 being right)

        //Move  
        rigidbody.velocity = new Vector2(movementDirection * movementSpeed, rigidbody.velocity.y);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGroundCheck : MonoBehaviour
{

    public BoxCollider2D groundCheckBox;
    public Transform groundCheck;
    public bool isGrounded;
    public float checkRadius;
    public LayerMask groundObjects;

    private void FixedUpdate()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, groundObjects);
    }


}

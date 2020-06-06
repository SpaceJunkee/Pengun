using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicEnemy : Enemy
{
     private bool
        groundDetected,
        wallDetected;

    [SerializeField]
    private float
        groundCheckDistance,
        wallCheckDistance,
        movementSpeed,
        idleTime;

    [SerializeField]
    private Transform
        groundCheck,
        wallCheck;
    
    [SerializeField]
    private LayerMask 
        groundCheckLayer;

     private Vector2 
        movement;

    // Start is called before the first frame update
    void Start()
    {
        Initialise("Basic");
        //SetMaxHealth(100);
    }
    

    protected override void UpdateMoveState(){
        groundDetected = Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckDistance, groundCheckLayer);
        wallDetected = Physics2D.Raycast(wallCheck.position, transform.right, wallCheckDistance, groundCheckLayer);

        if(PlayerDetected()){
            Debug.Log("Switching to Alert");
            SwitchState(State.Alert);
        }
        else if(!groundDetected || wallDetected)
        {
            Debug.Log("Switching to Idle");
            SwitchState(State.Idle);
        }
        else
        {
            Debug.Log("Moving");
            movement.Set(movementSpeed * facingDirection, enemyRb.velocity.y);
            enemyRb.velocity = movement;
        }

    }

    protected override void EnterIdleState(){
        idleTime = 2;
    }
    protected override void UpdateIdleState(){
        idleTime -= Time.deltaTime;

        if(idleTime < 0){
            Flip();
            SwitchState(State.Move);
        }
    }

    protected override void UpdateAlertState(){
         alertTime -= Time.deltaTime;

        if(alertTime < 0){
            if(PlayerDetected()){
                SwitchState(State.Attack);
            }
            else{
                SwitchState(State.Move);
            }
        }
    }

     protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
        Gizmos.DrawLine(groundCheck.position, new Vector2(groundCheck.position.x, groundCheck.position.y - groundCheckDistance));
        Gizmos.DrawLine(wallCheck.position, new Vector2(wallCheck.position.x + wallCheckDistance, wallCheck.position.y));
    }
   





}

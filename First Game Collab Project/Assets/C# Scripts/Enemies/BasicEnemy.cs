using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicEnemy : Enemy
{
     private bool
        groundDetected,
        wallDetected,
        crateDetected;

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
        groundCheckLayer,
        crateCheckLayer;

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
        wallDetected = Physics2D.Raycast(wallCheck.position, transform.right * facingDirection, wallCheckDistance, groundCheckLayer);
        crateDetected = Physics2D.Raycast(wallCheck.position, transform.right * facingDirection, wallCheckDistance, crateCheckLayer);

        if (PlayerDetected()){
            SwitchState(State.Alert);
        }
        else if (PlayerInRange())
        {
            Flip();
            SwitchState(State.Alert);
        }
        else if(!groundDetected || wallDetected || crateDetected)
        {
            SwitchState(State.Idle);
        }
        else
        {
            movement.Set(movementSpeed * facingDirection, enemyRb.velocity.y);
            enemyRb.velocity = movement;
        }

    }

    protected override void EnterIdleState(){
        idleTime = 2;
    }
    protected override void UpdateIdleState(){

        if (PlayerDetected())
        {
            SwitchState(State.Alert);
        }
        else if (PlayerInRange())
        {
            Flip();
            SwitchState(State.Alert);
        }
        else if (idleTime < 0){
            Flip();
            SwitchState(State.Move);
        }
        else
        {
            idleTime -= Time.deltaTime;
        }
    }

    protected override void UpdateAlertState(){
         alertTime -= Time.deltaTime;

        if(alertTime < 0){
            if(PlayerDetected() || PlayerInRange()){
                SwitchState(State.Attack);
            }
            else if (!groundDetected || wallDetected || crateDetected)
            {
                SwitchState(State.Idle);
            }
            else
            {
                SwitchState(State.Move);
            }
        }
    }

    protected override void EnterDeadState()
    {
        Debug.Log("Entered function");
        Instantiate(deathChunkParticle, enemy.transform.position, deathChunkParticle.transform.rotation);
        Instantiate(deathBloodParticle, enemy.transform.position, deathBloodParticle.transform.rotation);
        Destroy(gameObject);
    }

    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
        Gizmos.DrawLine(groundCheck.position, new Vector2(groundCheck.position.x, groundCheck.position.y - groundCheckDistance));
        Gizmos.DrawLine(wallCheck.position, new Vector2(wallCheck.position.x + wallCheckDistance, wallCheck.position.y));
    }

   





}

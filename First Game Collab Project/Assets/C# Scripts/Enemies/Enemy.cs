using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour
{

    protected enum State{
        Move,
        Idle,
        Alert,
        Attack,
        Search,
        Stun,
        Dead
    }
    private bool 
        canShoot,
        canFlip;

    public int 
        maxHealth,
        frontRayLength,
        behindRayLength;
    
    private float 
        coolDownTime;
    protected int facingDirection;

    protected float 
    alertTime,
    currentHealth,
    searchTime;

    private State currentState;

     [SerializeField]
    protected GameObject
        deathChunkParticle,
        deathBloodParticle,
        bulletPrefab;

    [SerializeField]
    private Transform
        shootPosition,
        frontRayPosition,
        behindRayPosition;
    protected GameObject enemy;
    protected Rigidbody2D enemyRb;
    protected Animator enemyAnim;

    protected void Initialise(string tagName){
        enemy = transform.Find(tagName).gameObject;
        enemyRb = enemy.GetComponent<Rigidbody2D>();
        enemyAnim = enemy.GetComponent<Animator>();

        currentHealth = maxHealth;
        facingDirection = 1;
    }

    protected void SetMaxHealth(int maxHealth){
        this.maxHealth = maxHealth;
    }

    // Update is called once per frame
    protected void Update()
    {
         switch (currentState)
        {
            case State.Move:
                UpdateMoveState();
                break;
            case State.Idle:
                UpdateIdleState();
                break;
            case State.Alert:
                UpdateAlertState();
                break;
            case State.Attack:
                UpdateAttackState();
                break;
            case State.Search:
                UpdateSearchState();
                break;
            case State.Stun:
                UpdateStunState();
                break;
            case State.Dead:
                break;
        }

    }

      protected void SwitchState(State state)
    {
        switch (state)
        {
           case State.Move:
                break;
            case State.Idle:
                EnterIdleState();
                break;
            case State.Alert:
                EnterAlertState();
                break;
            case State.Attack:
                EnterAttackState();
                break;
            case State.Search:
                EnterSearchState();
                break;
            case State.Stun:
                break;
            case State.Dead:
                break;
        }

        currentState = state;
    }

    public void ExecuteEnemyUpdate(){
        Update();
    }

    protected virtual void EnterAlertState(){
        alertTime = 0.5f;
    }
    protected virtual void EnterIdleState(){

    }
    protected virtual void EnterAttackState(){
        canShoot = true;
        coolDownTime = 1;
    }

    protected virtual void EnterSearchState(){
        searchTime = 4;
        canFlip = true;
    }
    protected virtual void EnterDeadState(){

    }
    protected virtual void UpdateMoveState(){

    }

    protected virtual void UpdateIdleState(){

    }

    protected virtual void UpdateAlertState(){
       
    }
    protected virtual void UpdateAttackState(){
        if(PlayerDetected()){
            Shoot();
        } 
        else if (PlayerInRange())
        {
            Flip();
            Shoot();
        }
        else
        {
            SwitchState(State.Search);
        }

        
    }

    protected virtual void UpdateSearchState(){
        searchTime -= Time.deltaTime;

        if(PlayerDetected()){
            SwitchState(State.Attack);
        }

        if(searchTime < 2 && canFlip){
            Flip();
            canFlip = false;
        }

        if(searchTime < 0){
            SwitchState(State.Move);
        }
    }

    protected virtual void UpdateStunState(){
       
    }
   protected virtual void ExitMoveState(){

    }
    protected virtual void ExitAlertState(){

    }
    protected virtual void ExitDeadState(){

    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        Debug.Log(currentHealth);

        if(currentHealth <= 0)
        {
            EnterDeadState();
        }
    }
    
    protected virtual bool PlayerDetected(){
        frontRayLength = 10;
        RaycastHit2D hit = Physics2D.Raycast(frontRayPosition.position, Vector2.right * facingDirection, frontRayLength);

        if (hit.collider != null)
        {
            if (hit.collider.name == "Player")
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            return false;
        }
    }

    protected virtual bool PlayerInRange()
    {
        behindRayLength = 3;
        RaycastHit2D hit = Physics2D.Raycast(behindRayPosition.position, Vector2.left * facingDirection, behindRayLength);

        if (hit.collider != null)
        {
            if (hit.collider.name == "Player")
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            return false;
        }
    }

    protected void Flip()
    {
        facingDirection *= -1;
        enemy.transform.Rotate(0.0f, 180.0f, 0.0f);

    }

    protected void Shoot()
    {
        if (canShoot)
        {
            Instantiate(bulletPrefab, shootPosition.position, shootPosition.rotation);
            canShoot = false;
        }

        coolDownTime -= Time.deltaTime;
        Debug.Log(coolDownTime);

        if (coolDownTime <= 0)
        {
            coolDownTime = 2;
            canShoot = true;
        }
    }

     protected virtual void OnDrawGizmos()
    {
        Gizmos.DrawLine(shootPosition.position, new Vector2(shootPosition.position.x + (frontRayLength * facingDirection), shootPosition.position.y));
        Gizmos.DrawLine(behindRayPosition.position, new Vector2(behindRayPosition.position.x - (behindRayLength * facingDirection), behindRayPosition.position.y));
    }
    
}

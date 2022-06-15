using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HurtKnockBack : MonoBehaviour
{
    Rigidbody2D playerRB;
    public HealthManager healthManager;
    public MeshRenderer meshRenderer;
    public PlayerMovement playerMovement;
    public LayerMask lavaObjects;

    public float knockBackAmount;
    public float knockBackdirectionForce;
    public float hurtNumber = 0;

    private bool hasBeenHurt = false;
    private bool isStandingOnLava;
    public bool isPlayerFacingRight;
    private bool isMoving;

    public int playerHurtDamage = 25;
    public Animator animator;
    public bool isCurrentlyKnockBacking = false;

    TimeManager timeManager;

    private void Start()
    {
        healthManager = this.GetComponent<HealthManager>();
        playerRB = playerMovement.getRigidbody2D();
        timeManager = GameObject.Find("TimeManager").GetComponent<TimeManager>();
    }


    void Update()
    {
        isStandingOnLava = Physics2D.OverlapCircle(playerMovement.getGroundCheck().position, playerMovement.getCheckRadius(), lavaObjects);
        isPlayerFacingRight = playerMovement.getPlayerFaceRight();

        if(playerMovement.getMovementDirection() != 0)
        {
            isMoving = true;
        }
        else
        {
            isMoving = false;
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy") && !hasBeenHurt)
        {
            StartCoroutine(HurtPlayer(collision.transform));          
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy") && !hasBeenHurt)
        {
            StartCoroutine(HurtPlayerTrigger(collision.transform));
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy") && !hasBeenHurt)
        {
            StartCoroutine(HurtPlayerTrigger(collision.transform));
        }
    }

    public IEnumerator HurtPlayer(Transform other)
    {
        healthManager.HurtPlayer(playerHurtDamage);

        if (!isCurrentlyKnockBacking)
        {
            KnockBack(other);
        }


        hasBeenHurt = true;
        yield return new WaitForSeconds(0.5f);
        hasBeenHurt = false;
    }

    public IEnumerator HurtPlayerTrigger(Transform other)
    {
        healthManager.HurtPlayer(playerHurtDamage);

        if (!isCurrentlyKnockBacking)
        {
            KnockBack(other);
        }
        


        hasBeenHurt = true;
        yield return new WaitForSeconds(0.5f);
        hasBeenHurt = false;
    }

    public void KnockBack(Transform otherPos)
    {
        isCurrentlyKnockBacking = true;

        animator.SetTrigger("KnockBack");
        if (healthManager.getCurrentHealth() >= 1)
        {
            StopPlayerWithKnockBackConstraints();
            StartCoroutine("StopTimeForKnockBack");
            timeManager.InvokeStopSlowMotion(0.005f);
            Invoke("RemoveConstraints", 0.35f);
        }
        Vector2 direction = (otherPos.transform.position - this.transform.position).normalized;

        if (playerMovement.getIsGrounded())
        {
            playerRB.AddForce(-direction * knockBackAmount, ForceMode2D.Impulse);
        }
        else
        {
            playerRB.AddForce(-direction * (knockBackAmount * 1.5f), ForceMode2D.Impulse);
        }

    }

    IEnumerator StopTimeForKnockBack()
    {
        timeManager.StartSlowMotion(0.001f);
        yield return new WaitForSeconds(0.0001f);
        timeManager.InvokeStopSlowMotion(0.0001f);
    }

    public bool getIsStandingOnLava()
    {
        return isStandingOnLava;
    }

    public void StopPlayerWithKnockBackConstraints()
    {
        PlayerMovement.canMove = false;
        if (playerMovement.getIsGrounded())
        {
            playerMovement.StopPlayer(true, false, true);
        }
        else
        {
            playerMovement.StopPlayer(false, false, true);
        }
        
        //playerRB.constraints = RigidbodyConstraints2D.FreezeRotation;
    }

    public void RemoveConstraints()
    {
        playerMovement.EnableMovement();
        PlayerMovement.canMove = true;
        //playerRB.constraints = PlayerMovement.originalConstraints;
        isCurrentlyKnockBacking = false;
    }

}

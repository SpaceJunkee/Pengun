using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HurtKnockBack : MonoBehaviour
{
    public Rigidbody2D playerRB;
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
            StartCoroutine(HurtPlayer(collision));          
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy") && !hasBeenHurt)
        {
            StartCoroutine(HurtPlayerTrigger(collision));
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy") && !hasBeenHurt)
        {
            StartCoroutine(HurtPlayerTrigger(collision));
        }
    }

    public IEnumerator HurtPlayer(Collision2D collision)
    {
        Vector2 direction = (this.transform.position - collision.transform.position).normalized;
        this.GetComponent<HealthManager>().HurtPlayer();

        if (isPlayerFacingRight)
        {
            KnockBack(direction);
        }
        else if(!isPlayerFacingRight && isStandingOnLava)
        {
            KnockBack(direction);
        }
        else
        {
            KnockBack(-direction);
        }
        

        hasBeenHurt = true;
        yield return new WaitForSeconds(1.5f);
        hasBeenHurt = false;
    }

    public IEnumerator HurtPlayerTrigger(Collider2D collision)
    {
        Vector2 direction = (this.transform.position - collision.transform.position).normalized;
        this.GetComponent<HealthManager>().HurtPlayer();

        if (isPlayerFacingRight)
        {
            KnockBack(direction);
        }
        else if (!isPlayerFacingRight && isStandingOnLava)
        {
            KnockBack(direction);
        }
        else
        {
            KnockBack(-direction);
        }


        hasBeenHurt = true;
        yield return new WaitForSeconds(1.5f);
        hasBeenHurt = false;
    }

    public void KnockBack(Vector2 direction)
    {
        if(healthManager.getCurrentHealth() >= 1 && playerMovement.getIsGrounded() == true)
        {
            StopPlayerWithKnockBackConstraints();
            Invoke("RemoveConstraints", 0.25f);
        }
        
        playerRB.AddForce(direction * knockBackAmount, ForceMode2D.Impulse);
        this.transform.Translate(direction * knockBackdirectionForce);       
        
    }

    public bool getIsStandingOnLava()
    {
        return isStandingOnLava;
    }

    public void StopPlayerWithKnockBackConstraints()
    {
        playerRB.velocity = Vector2.zero;
        playerRB.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;
    }

    public void RemoveConstraints()
    {
        playerRB.constraints = PlayerMovement.originalConstraints;
    }

}

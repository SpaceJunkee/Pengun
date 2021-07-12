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


    void Update()
    {
        isStandingOnLava = Physics2D.OverlapCircle(playerMovement.getGroundCheck().position, playerMovement.getCheckRadius(), lavaObjects);
        isPlayerFacingRight = playerMovement.getPlayerFaceRight();
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy") && !hasBeenHurt)
        {
            StartCoroutine(HurtPlayer(collision));          
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
        yield return new WaitForSeconds(2f);
        hasBeenHurt = false;
    }

    public void KnockBack(Vector2 direction)
    {
        playerRB.AddForce(direction * knockBackAmount, ForceMode2D.Impulse);
        this.transform.Translate(direction * knockBackdirectionForce);
    }

    public bool getIsStandingOnLava()
    {
        return isStandingOnLava;
    }

  
}

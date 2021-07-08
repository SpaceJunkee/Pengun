using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HurtKnockBack : MonoBehaviour
{
    public Rigidbody2D playerRB;
    public float knockBackAmount;
    public float knockBackdirectionForce;
    public float hurtNumber = 0;
    private bool hasBeenHurt = false;
    public MeshRenderer meshRenderer;
    public PlayerMovement playerMovement;
    private bool isStandingOnLava;
    public LayerMask lavaObjects;

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
        
        CameraShake.Instance.ShakeCamera(6f, 0.2f);
        StartCoroutine(Flash());
        hasBeenHurt = true;
        yield return new WaitForSeconds(2f);
        hasBeenHurt = false;
    }

    public IEnumerator Flash()
    {
        for(int i = 0; i < 4; i++)
        {
            meshRenderer.enabled = false;
            yield return new WaitForSeconds(0.07f);
            meshRenderer.enabled = true;
            yield return new WaitForSeconds(0.07f);
        }      
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

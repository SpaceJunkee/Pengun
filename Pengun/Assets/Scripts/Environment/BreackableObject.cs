using System.Collections;
using System.Collections.Generic;
using System.Security;
using UnityEngine;

public class BreackableObject : MonoBehaviour
{

    [SerializeField]
    protected GameObject breakingChunkParticle;
    LootSplash lootSplash;

    public int health = 10;
    public bool isBreakable = true;
    bool canBeHurt = true;

    //Handle force applied when player melee attacks them
    public bool canApplyDownWardForceOnHit;
    public bool canApplyUpWardForceOnHit;

    private void Start()
    {
        lootSplash = GetComponent<LootSplash>();
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isBreakable)
        {
            if (collision.gameObject.CompareTag("DashColliderChecker") && PlayerMovement.isDashing ||
                collision.tag == "Player" && PlayerMovement.isfalling ||
                collision.gameObject.CompareTag("Bullet")
                )
            {
                if (collision.gameObject.CompareTag("DashColliderChecker")){
                    Debug.Log("DashCol");
                }
                CameraShake.Instance.ShakeCamera(4f, 5f, 0.2f);
                BreakObject();
            }
        }
        
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (isBreakable)
        {
            if (collision.gameObject.CompareTag("DashColliderChecker") && PlayerMovement.isDashing)
            {
                if (collision.gameObject.CompareTag("DashColliderChecker"))
                {
                    Debug.Log("DashCol");
                }
                CameraShake.Instance.ShakeCamera(4f, 5f, 0.2f);
                BreakObject();
            }
        }
       
    }

    //On Trigger Objects here


    //Enemy to take damage
    public void TakeDamage(int damage)
    {
        if (canBeHurt)
        {
            StartCoroutine("CanBeHurtAgain");
            CameraShake.Instance.ShakeCamera(4f, 5f, 0.2f);
            health -= damage;

            if (health <= 0)
            {
                BreakObject();
            }

            if (lootSplash.containsLoot)
            {
                lootSplash.summonDrop();
            }
        }
        
    }

    IEnumerator CanBeHurtAgain()
    {
        canBeHurt = false;
        yield return new WaitForSeconds(0.25f);
        canBeHurt = true;
    }

    public void BreakObject()
    {
        Instantiate(breakingChunkParticle, gameObject.transform.position, breakingChunkParticle.transform.rotation);

        if (lootSplash.containsLoot)
        {
            lootSplash.summonDrop();
        }

        Destroy(gameObject);
    }

}

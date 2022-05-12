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

    private void Start()
    {
        lootSplash = GetComponent<LootSplash>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (isBreakable)
        {
            if (collision.collider.name == "Player" && PlayerMovement.isDashing || 
                collision.collider.name == "Player" && PlayerMovement.isfalling ||
                collision.gameObject.CompareTag("Bullet")
                )
            {
                CameraShake.Instance.ShakeCamera(4f, 5f, 0.2f);
                BreakObject();
            }
        }
        
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (isBreakable)
        {
            if (collision.collider.name == "Player" && PlayerMovement.isDashing || collision.collider.name == "Player" && PlayerMovement.isfalling)
            {
                CameraShake.Instance.ShakeCamera(4f, 5f, 0.2f);
                BreakObject();
            }
        }
       
    }

    //On Trigger Objects here


    //Enemy to take damage
    public void TakeDamage(int damage)
    {
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

using System.Collections;
using System.Collections.Generic;
using System.Security;
using UnityEngine;

public class BreackableCrate : MonoBehaviour
{

    [SerializeField]
    protected GameObject
        crateChunkParticle,
        crateDustParticle;

    public int health = 50;
    public bool isBreakable = true;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (isBreakable)
        {
            if (collision.collider.name == "Player" && PlayerMovement.isDashing || 
                collision.collider.name == "Player" && PlayerMovement.isfalling)
            {
                CameraShake.Instance.ShakeCamera(4f, 0.2f);
                BreakCrate();
            }
        }
        
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (isBreakable)
        {
            if (collision.collider.name == "Player" && PlayerMovement.isDashing || collision.collider.name == "Player" && PlayerMovement.isfalling)
            {
                CameraShake.Instance.ShakeCamera(4f, 0.2f);
                BreakCrate();
            }
        }
       
    }


    //Enemy to take damage
    public void TakeDamage(int damage)
    {
        CameraShake.Instance.ShakeCamera(4f, 0.2f);
        health -= damage;

        if (health <= 0)
        {
            BreakCrate();
        }
    }

    private void BreakCrate()
    {
        Instantiate(crateChunkParticle, gameObject.transform.position, crateChunkParticle.transform.rotation);
        Instantiate(crateDustParticle, gameObject.transform.position, crateDustParticle.transform.rotation);
        Destroy(gameObject);
    }

}

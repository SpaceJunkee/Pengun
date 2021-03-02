using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySmash : MonoBehaviour
{
    [SerializeField]
    protected GameObject crateChunkParticle, crateDustParticle;
    public TimeManager timeManager;
    public int health = 50;

    private void OnCollisionEnter2D(Collision2D collision)
    {

        if (collision.collider.name == "Player" && PlayerMovement.isDashing)
        {
            CameraShake.Instance.ShakeCamera(4f, 0.2f);
            timeManager.StartSlowMotion(0.1f);
            timeManager.Invoke("StopSlowMotion", 0.025f);
            BreakCrate();
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.collider.name == "Player" && PlayerMovement.isDashing)
        {
            CameraShake.Instance.ShakeCamera(4f, 0.2f);
            timeManager.StartSlowMotion(0.1f);
            timeManager.Invoke("StopSlowMotion", 0.025f);
            BreakCrate();
        }
    }


    //Enemy to take damage
    public void TakeDamage(int damage)
    {
        health -= damage;

        if (health <= 0)
        {
            BreakCrate();
        }
    }

    private void BreakCrate()
    {
        this.gameObject.GetComponent<SpriteRenderer>().enabled = false;
        Instantiate(crateChunkParticle, gameObject.transform.position, crateChunkParticle.transform.rotation);
        Instantiate(crateDustParticle, gameObject.transform.position, crateDustParticle.transform.rotation);
        Destroy(gameObject, 0.025f);
    }
}

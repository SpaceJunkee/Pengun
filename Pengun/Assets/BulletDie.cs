using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletDie : MonoBehaviour
{
    public GameObject bulletDeathParticleEffect;
    public float dieTimer;

    private void Start()
    {
        StartCoroutine(DestroyTimer());
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject collisionGameObject = collision.gameObject;

        if(collisionGameObject.tag != "Player")
        {
            Die();
        }
    }

    IEnumerator DestroyTimer()
    {
        yield return new WaitForSeconds(dieTimer);
        Die();
    }

    void Die()
    {
        if(bulletDeathParticleEffect != null)
        {
            Instantiate(bulletDeathParticleEffect, transform.position, Quaternion.identity);
        }

        Destroy(gameObject);
    }
}

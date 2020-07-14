using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OndestroyOpenDoor : MonoBehaviour
{
    // Start is called before the first frame update
    private ParticleSystem particle;
    private SpriteRenderer spriteRend;
    private BoxCollider2D boxCollider;

    public int health = 300;

    private static bool isDestroyed = false;

    private void Awake()
    {
        particle = GetComponentInChildren<ParticleSystem>();
        spriteRend = GetComponent<SpriteRenderer>();
        boxCollider = GetComponent<BoxCollider2D>();
    }


    //Enemy to take damage
    public void TakeDamage(int damage)
    {
        health -= damage;

        if (health <= 0)
        {
            StartCoroutine(BreakUp());
        }
    }

    private IEnumerator BreakUp()
    {
        particle.Play();

        spriteRend.enabled = false;
        boxCollider.enabled = false;

        yield return new WaitForSeconds(particle.main.startLifetime.constantMax);
        isDestroyed = true;
        Destroy(gameObject);
    }

    public static bool getIsDestroyed()
    {
        return isDestroyed;
    }
}

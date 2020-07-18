using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    protected float 
        speed,
        lifeTime;

    protected int bulletDamage;

    public Rigidbody2D rigidBody;

    protected void Update()
    {
        lifeTime -= Time.deltaTime;
        if (lifeTime <= 0)
        {
            Destroy(this.gameObject);
        }
    }

    protected void OnTriggerEnter2D(Collider2D collision)
    {
        BasicEnemy enemy = collision.GetComponentInParent<BasicEnemy>();
        BreackableCrate crate = collision.GetComponent<BreackableCrate>();

        if (enemy != null)
        {
            enemy.TakeDamage(bulletDamage);
        }

        if (crate != null)
        {
            crate.TakeDamage(bulletDamage);
        }

        destroyBullet();
    }

    protected void moveBullet()
    {
        //Use right instead of forward as we dont want to use z axis in 2d game
        rigidBody.transform.Rotate(0, 0, Random.Range(-2.5f, 2.5f));
        rigidBody.velocity = transform.right * speed;

    }

    protected void destroyBullet()
    {
        Destroy(gameObject);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChainGunBullets : MonoBehaviour
{
    public float speed = 50f;
    public float lifeTimeOfBullet;
    public Rigidbody2D rigidBody;
    public int bulletDamage = 100;

    // Start is called before the first frame update
    void Start()
    {
        moveBullet();

    }
    void Update()
    {
        lifeTimeOfBullet -= Time.deltaTime;
        if (lifeTimeOfBullet <= 0)
        {
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        EnemyScript1 enemy = collision.GetComponent<EnemyScript1>();
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

    void moveBullet()
    {
        //Use right instead of forward as we dont want to use z axis in 2d game
        rigidBody.transform.Rotate(0, 0, Random.Range(-2f, 2f));
        rigidBody.velocity = transform.right * speed;

    }

    void destroyBullet()
    {
        Destroy(gameObject);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{

    public float speed = 20f;
    public Rigidbody2D rigidBody;
    public int bulletDamage = 40;

    // Start is called before the first frame update
    void Start()
    {
        moveBullet();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
       EnemyScript1 enemy = collision.GetComponent<EnemyScript1>();

        if(enemy != null)
        {
            enemy.TakeDamage(bulletDamage);
        }
        Destroy(gameObject);
    }

    void moveBullet()
    {
        //Use right instead of forward as we dont want to use z axis in 2d game
        rigidBody.velocity = transform.right * speed;
    }
}
